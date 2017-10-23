using System.Collections.Generic;
using UnityEngine;

namespace KyleStankovich.BattleDash {
    /// <summary>
    /// Class responsible for handling chunks throughout the game.
    /// </summary>
    [AddComponentMenu("Battle Dash/Misc/Chunk")]
    public class Chunk : MonoBehaviour
    {
        [SerializeField]
        private Collider lengthCollider = null;

        private List<CollectableSpawnPoint> m_CollectableSpawnPoints = new List<CollectableSpawnPoint>();
        private Vector3 m_InitialPosition = Vector3.zero;

        #region Properties
        public float ColliderFullLength { get { return lengthCollider.bounds.size.z; } }
        public float ColliderHalfLength { get { return lengthCollider.bounds.extents.z; } }
        #endregion

        #region Methods

        #region Unity methods
        void Awake()
        {
            InitChunk();
        }

        private void OnEnable()
        {
            PopulateChunk();
        }

        private void OnDisable()
        {
            DeleteCollectables();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Initializes the chunk.
        /// </summary>
        private void InitChunk()
        {
            //Add spawn point to the list
            m_CollectableSpawnPoints.AddRange(transform.GetComponentsInChildren<CollectableSpawnPoint>());
        }

        /// <summary>
        /// Randomly spawns collectables throughout the chunk.
        /// </summary>
        private void PopulateChunk()
        {
            GameCollectables collectables = GameCollectables.Instance;

            if (collectables == null)
            {
                //Debug.LogError("The <b>collectables</b> variable for the \"" + gameObject.name + "\" chunk is null! Did you assign it in the Editor?");
                return;
            }

            for (int i = 0; i < m_CollectableSpawnPoints.Count; i++)
            {
                Collectable collectablePrefab = collectables.GetRandom(m_CollectableSpawnPoints[i].Type);

                if (collectablePrefab != null)
                    Instantiate(collectablePrefab, m_CollectableSpawnPoints[i].transform.position, Quaternion.identity, transform);
            }
        }

        /// <summary>
        /// Deletes all Collectables on Chunk.
        /// </summary>
        private void DeleteCollectables()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Collectable collectable = transform.GetChild(i).GetComponent<Collectable>();
                if(collectable != null)
                {
                    Destroy(collectable.gameObject);
                }
            }
        }
        #endregion

        #endregion
    }
}
