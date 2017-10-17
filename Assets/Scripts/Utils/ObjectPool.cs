/// <summary>
/// Class for creating an object pool. This allows you to reuse old objects instead of creating new ones.
/// </summary>
/// <typeparam name="T">The Type that the object pool will be.</typeparam>
[System.Serializable]
public abstract class ObjectPool<T> where T : new() {
    #region Properties
    /// <summary>
    /// The list of objects that are currently inside the object pool.
    /// </summary>
    public System.Collections.Generic.List<T> PooledObjects { get; set; }

    /// <summary>
    /// Number of objects that will be created per pool object. The value of this property will not matter if WillGrow is true.
    /// </summary>
    public int ObjectSize { get; set; }

    /// <summary>
    /// True if the object pool can go past its object size per object, false if it cannot.
    /// </summary>
    public bool WillGrow { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Create a pool of objects to save performance.
    /// </summary>
    /// <param name="objectSize">The amount of objects that will be created per pool object.</param>
    /// <param name="willGrow">Is the object pool allowed to expand past its object size for each object?</param>
    /// <param name="addEmptyObjects">Fills the entire object pool full of empty objects this is true.</param>
    public ObjectPool(int objectSize = 5, bool willGrow = false, bool addEmptyObjects = false) {
        //Assign the class variables
        ObjectSize = objectSize;
        WillGrow = willGrow;
        PooledObjects = new System.Collections.Generic.List<T>(objectSize);

        //Add new objects to object pool if allowed
        if(addEmptyObjects)
            for(int i = 0; i < ObjectSize; i++)
                AddObject();
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Deletes all objects that are currently in the object pool list.
    /// </summary>
    public virtual void DeleteAllObjects() {
        PooledObjects.Clear();
    }

    /// <summary>
    /// Randomly finds an object in the object pool, and returns it. If object returned is null, it will add a new object
    /// to the object pool if it is allowed.
    /// </summary>
    /// <returns>Random object from object pool.</returns>
    public virtual T GetRandomPooledObject() {
        //Loop through all items looking for a non-null object
        for(int i = 0; i < PooledObjects.Count; i++)
            if(PooledObjects[i] != null) return PooledObjects[i];

        //If the object pool can go past its limit, it will do so here
        if(WillGrow)
            return AddObject();

        return default(T);
    }
    
    /// <summary>
    /// Adds a new object to the object pool, and returns it if needed.
    /// </summary>
    /// <returns>The newly added object.</returns>
    public virtual T AddObject() {
        T obj = new T();
        PooledObjects.Add(obj);

        return obj;
    }
    #endregion
}
