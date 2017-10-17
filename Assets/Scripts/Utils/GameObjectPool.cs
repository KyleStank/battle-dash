using UnityEngine;

namespace TurmoilStudios.Utils {
    /// <summary>
    /// Class for creating a object pool of Unity3D game objects.
    /// </summary>
    [System.Serializable]
    public class GameObjectPool : ObjectPool<GameObject> {
        [Header("Setup Variables(make sure these get set)")]
        [Tooltip("The array of objects that will fill up the object pool..")]
        [SerializeField]
        GameObject[] poolObjects = null;
        [SerializeField]
        [Tooltip("The parent of the object pool.")]
        GameObject poolHolder = null;

        [Header("Settings")]
        [SerializeField]
        [Tooltip("The number of objects that will be added to the object pool on start.")]
        int objectSize = 5;
        [SerializeField]
        [Tooltip("If checked, the object pool will grow if it needs to. Sometimes, leaving the maxSize at 0 and this checked would be the best option.")]
        bool willGrow = false;

        #region Properties
        /// <summary>
        /// The game object(s) that will fill the object pool.
        /// </summary>
        public GameObject[] PoolObjects {
            get { return poolObjects; }
            set { poolObjects = value; }
        }

        /// <summary>
        /// Number of objects that will be created per pool object. The value of this property will not matter if WillGrow is true.
        /// </summary>
        public new int ObjectSize {
            get { return objectSize; }
            set { objectSize = value; }
        }

        /// <summary>
        /// True if the object pool can go past its object size per object, false if it cannot.
        /// </summary>
        public new bool WillGrow {
            get { return willGrow; }
            set { willGrow = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new GameObjectPool object. Useful for when you have a lot of game objects to spawn in. To improve performance further,
        /// make sure the maxSize variable is as small as possible.
        /// </summary>
        /// <param name="poolObject">The game object that will fill the object pool.</param>
        /// <param name="objectSize">The amount of objects that will be created per pool object.</param>
        /// <param name="willGrow">Is the object pool allowed to expand past its object size for each object?</param>
        public GameObjectPool(GameObject[] poolObjects, int objectSize = 5, bool willGrow = false) : base(objectSize, willGrow, false) {
            this.poolObjects = poolObjects;

            //Assign class specific variables. These are only here to make things easier for Designers
            this.objectSize = objectSize;
            this.willGrow = willGrow;

            InitObjectPool();
        }

        /// <summary>
        /// Initiates the object pool. Should really only be called outside of the GameObjectPool class if the constructor
        /// won't get invoked for some reason.
        /// </summary>
        public virtual void InitObjectPool() {
            PooledObjects = new System.Collections.Generic.List<GameObject>(ObjectSize);

            //Make sure the object pool holder exists
            if(poolHolder == null)
                poolHolder = new GameObject("Object Pool of type, GameObject");

            //Create new instances of game object for the object pool
            for(int i = 0; i < PoolObjects.Length; i++)
                for(int f = 0; f < ObjectSize; f++)
                    AddObjectFrom(i, true);
        }
        #endregion

        #region Override methods
        /// <summary>
        /// Randomly finds a game object in the object pool, and returns it. If game object returned is null or in-active, it will add a new
        /// game object to the object pool if it is allowed.
        /// </summary>
        /// <returns>Random object from object pool.</returns>
        public override GameObject GetRandomPooledObject() {
            byte choice = (byte)Random.Range(0, PooledObjects.Count);

            //Loop through all items looking for a non-null and in-active game object.
            for(int i = 0; i < PooledObjects.Count; i++) {
                if(!PooledObjects[choice].activeInHierarchy)
                    return PooledObjects[choice];
                else
                    choice = (byte)Random.Range(0, PooledObjects.Count);
            }

            //This code is already in the ObjectPool class, but we want to add the Instantiate method to it here, so we rewrite it :)
            if(WillGrow)
                return AddRandomObject();

            return base.GetRandomPooledObject();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Adds a new game object to the object pool, and returns it if needed.
        /// </summary>
        /// <param name="disable">True to disable new game object, false to not disable game object.</param>
        /// <returns>The newly added game object.</returns>
        public GameObject AddRandomObject(bool disable = false) {
            if(PoolObjects == null) {
                Debug.LogError("\"poolObject\" is null! Please make sure it's assigned in the Unity Editor.");
                return null;
            }

            //Create GameObject
            GameObject obj = (GameObject)Object.Instantiate(PoolObjects[Random.Range(0, PoolObjects.Length)], poolHolder.transform);
            PooledObjects.Add(obj);

            //Set game object in-active if told to do so
            if(disable)
                obj.SetActive(false);

            return obj;
        }

        /// <summary>
        /// Adds a new game object to the object pool from a specified index of the PoolObjects; returns new game object if needed.
        /// </summary>
        /// <param name="index">Index of PoolObjects to get game object from.</param>
        /// <param name="disable">True to disable new game object, false to not disable game object.</param>
        /// <returns></returns>
        public GameObject AddObjectFrom(int index, bool disable = false) {
            if(PoolObjects == null) {
                Debug.LogError("\"poolObject\" is null! Please make sure it's assigned in the Unity Editor.");
                return null;
            }

            if(PoolObjects.Length < index) {
                Debug.LogError("The length of the PoolObjects variable is too short! Make sure you don't try to access something that is not there!");
                return null;
            }

            //Create GameObject
            GameObject obj = (GameObject)Object.Instantiate(PoolObjects[index], poolHolder.transform);
            PooledObjects.Add(obj);

            //Set game object in-active if told to do so
            if(disable)
                obj.SetActive(false);

            return obj;
        }
        #endregion
    }
}
