using System.Collections.Generic;
using UnityEngine;
using TurmoilStudios.Utils;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Class responsible for generating the random levels.
    /// </summary>
    [AddComponentMenu("Battle Dash/Managers/Level Manager")]
    public class LevelManager : Singleton<LevelManager> {
        [Header("Object Pools")]
        [SerializeField]
        [Tooltip("The pool of chunks that the level generator will use to.")]
        GameObjectPool chunksPool;
        [SerializeField]
        [Tooltip("The pool of boss chunks that the level generator will use.")]
        GameObjectPool bossChunksPool;

        [Header("General Generation Settings")]
        [SerializeField]
        Chunk startingChunk = null;
        [SerializeField]
        [Tooltip("The offset added to the end of a chunk. When a player passed the end of a chunk plus this value, it gets disabled.")]
        float generationOffeset = 3.0f;
        [SerializeField]
        [Tooltip("The initial amount of chunks that will be generated when the player begins a run.")]
        int initialChunksAmount = 3;

        [Header("Boss Generation Settings")]
        [SerializeField]
        [Tooltip("The minimum number of chunks required to pass before a boss is spawned.")]
        int minChunksPerBoss = 3;
        [SerializeField]
        [Tooltip("The maximum number of chunks required to pass before a boss is spawned.")]
        int maxChunksPerBoss = 5;

        [Header("Character Settings")]
        [SerializeField]
        PlayableCharacter currentCharacter;

        Chunk lastChunk = null;
        List<Chunk> activeChunks = new List<Chunk>();
        int randomChunksPerBoss = 0;
        int chunksPassed = 0;

#if UNITY_EDITOR
        [Header("Debugging Options")]
        [SerializeField]
        bool disableAllColliders = false;
        [SerializeField]
        bool noBosses = false;
        [SerializeField]
        bool onlyBosses = false;
#endif

        #region Properties
        /// <summary>
        /// The level's chunk that the game starts with.
        /// </summary>
        public Chunk StartingChunk { get { return startingChunk; } }

        /// <summary>
        /// The current character that is being played.
        /// </summary>
        public PlayableCharacter CurrentCharacter { get { return currentCharacter; } }
        #endregion

        #region Methods

        #region Unity methods
        new void Awake() {
            base.Awake();

            //Randomly choose how many chunks between bosses
            RandomizeChunksPerBoss();
        }

        void Start() {
            //Inititalize object pools
            if(chunksPool != null)
                chunksPool.InitObjectPool();

            if(bossChunksPool != null)
                bossChunksPool.InitObjectPool();
        }

        void Update() {
            if(GameManager.Instance.Status == GameManager.GameStatus.GameInProgress)
                HandleGeneration();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Generates a randomly choosen chunk.
        /// </summary>
        public void GenerateChunk(bool isBossChunk) {
            Chunk chunk;

            //Grab a random chunk
            if(isBossChunk)
                chunk = bossChunksPool.GetRandomPooledObject().GetComponent<Chunk>();
            else
                chunk = chunksPool.GetRandomPooledObject().GetComponent<Chunk>();

#if UNITY_EDITOR
            if(disableAllColliders) {
                Collider[] colliders = chunk.GetComponentsInChildren<Collider>();
                for(int i = 0; i < colliders.Length; i++) {
                    Collider col = colliders[i];

                    if(col.gameObject.tag != "Ground")
                        colliders[i].enabled = false;
                }
            }
#endif

            //Make sure the chunk is not null
            if(chunk == null) {
                Debug.LogError("The LevelGenerator found a null chunk! Make sure there are no blank fields in the object pools for the LevelGenerator.");
                return;
            }

            chunk.gameObject.SetActive(true); //Set the chunk active before we set position. Reason? No clue, lmao. Just leave it.
                                              // If the instance of the GO is not active, it will not receive any update info, also, trigger events will not happen also.

            //Set initial position of chunk
            Vector3 newPos = lastChunk.transform.position;

            //Actually set position of chunk now
            newPos.x = 0.0f;
            newPos.y = 0.0f;
            newPos.z = lastChunk.transform.position.z + chunk.ColliderFullLength;
            chunk.transform.position = newPos;

            //Make this chunk the last chunk
            lastChunk = chunk;

            //Add chunk to the active chunks list
            activeChunks.Add(chunk);
        }

        /// <summary>
        /// Creates starting chunks for the level.
        /// </summary>
        public void GenerateStartingChunks() {
            lastChunk = startingChunk;

            //Create the beginning chunks
            for(int i = 0; i < initialChunksAmount; i++)
                GenerateChunk(false);
        }

        /// <summary>
        /// Destroys and removes the oldest chunk on the active chunks list (index 0).
        /// </summary>
        public void DestroyOldestChunk() {
            activeChunks[0].gameObject.SetActive(false);
            activeChunks.RemoveAt(0);

            //Disable starting chunk as well
            if(startingChunk.gameObject.activeInHierarchy)
                startingChunk.gameObject.SetActive(false);
        }

        /// <summary>
        /// Destroys all active chunks.
        /// </summary>
        public void DestroyAllChunks() {
            //Destroy chunks
            for(int i = 0; i < activeChunks.Count; i++)
                activeChunks[i].gameObject.SetActive(false);

            activeChunks.Clear();
        }

        /// <summary>
        /// Resets all of the active chunks' position closer to 0.
        /// </summary>
        public void ResetChunksPosition() {
            Camera mainCam = Camera.main;

            //Parent camera to player and stop following player for a second to ensure smooth set back
            if(mainCam != null)
                mainCam.transform.SetParent(currentCharacter.transform);

            //Get distance traveled so far
            float disTraveled = currentCharacter.transform.position.z - currentCharacter.InitialPosition.z;
            
            //Reset character's position
            Vector3 newPos = currentCharacter.transform.position;
            newPos.z = currentCharacter.InitialPosition.z;
            currentCharacter.transform.position = newPos;
            newPos = Vector3.zero;

            //Reset all of the chunks' position
            for(int i = 0; i < activeChunks.Count; i++) {
                newPos = activeChunks[i].transform.position;
                newPos.z -= disTraveled;
                activeChunks[i].transform.position = newPos;
            }

            //Unparent the camera and start following the player again
            if(mainCam != null)
                mainCam.transform.SetParent(null);
        }

        /// <summary>
        /// Resets the entire level without needing to load.
        /// </summary>
        public void ResetLevel() {
            //Move all chunks and the character back
            ResetChunksPosition();

            //Re-enable the starting chunk
            startingChunk.gameObject.SetActive(true);

            //Destroy all active chunks
            DestroyAllChunks();

            //Make the last chunk the very first chunk
            lastChunk = startingChunk;

            //Create the beginning chunks
            for(int i = 0; i < initialChunksAmount; i++)
                GenerateChunk(false);
        }
        #endregion

        #region Private methods
        //Checks to see if a new chunk should be generated
        void HandleGeneration() {
            if(activeChunks.Count <= 0) {
                Debug.LogError("There are no active chunks! Make sure you set up the chunks in the inspector.");
                return;
            }

            //If another chunk should be generated
            if(currentCharacter.transform.position.z >= (activeChunks[0].transform.position.z + activeChunks[0].ColliderHalfLength) + generationOffeset) {
#if UNITY_EDITOR
                //Generate no boss chunks
                if(noBosses) {
                    GenerateChunk(false);
                    DestroyOldestChunk();
                    return;
                }

                //Generate only boss chunks
                if(onlyBosses) {
                    GenerateChunk(true);
                    DestroyOldestChunk();
                    return;
                }
#endif

                //Check if boss should be spawned
                if(chunksPassed >= randomChunksPerBoss) {
                    GenerateChunk(true);
                    ResetChunksPosition();

                    //Reset boss chunk data
                    chunksPassed = 0;
                    RandomizeChunksPerBoss();
                } else
                    GenerateChunk(false);

                chunksPassed++;
                DestroyOldestChunk();
            }
        }

        /// <summary>
        /// Randomly chooses a new number of chunks that will need to be passed before a boss chunk is spawned.
        /// </summary>
        void RandomizeChunksPerBoss() {
            randomChunksPerBoss = Random.Range(minChunksPerBoss, maxChunksPerBoss + 1);
        }
        #endregion

        #endregion
    }
}
