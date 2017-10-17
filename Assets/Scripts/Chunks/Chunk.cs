﻿using System.Collections.Generic;
using UnityEngine;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Class responsible for handling chunks throughout the game.
    /// </summary>
    [AddComponentMenu("Battle Dash/Misc/Chunk")]
    public class Chunk : MonoBehaviour {
        [SerializeField]
        Collider lengthCollider = null;

        List<CollectableSpawnPoint> collectableSpawnPoints = new List<CollectableSpawnPoint>();

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
            collectableSpawnPoints.AddRange(transform.GetComponentsInChildren<CollectableSpawnPoint>());
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

            for (int i = 0; i < collectableSpawnPoints.Count; i++)
            {
                Collectable collectablePrefab = collectables.GetRandom(collectableSpawnPoints[i].Type);

                if (collectablePrefab != null)
                    Instantiate(collectablePrefab, collectableSpawnPoints[i].transform.position, Quaternion.identity, transform);
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
