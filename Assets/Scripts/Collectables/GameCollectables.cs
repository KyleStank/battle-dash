using System.Collections.Generic;
using UnityEngine;

namespace TurmoilStudios.BattleDash {
    [System.Serializable]
    [CreateAssetMenu(fileName = "GameCollectables.asset", menuName = "Battle Dash/Game Collectables")]
    public class GameCollectables : ScriptableObject {
        [SerializeField]
        List<CollectableGroup> CollectableGroups = new List<CollectableGroup>();
        //since there is only ever one GameCollectables.asset, we might as well make it a singleton.
        private static GameCollectables instance;
        public static GameCollectables Instance {
            get {
                if (!instance) instance = (GameCollectables)Resources.Load<GameCollectables>("GameCollectables");
                return instance;
            }
        }
        /// <summary>
        /// Initialized at runtime. Used to simpify searching for the collectable group based on the type.
        /// </summary>
        private Dictionary<CollectableType, CollectableGroup> RuntimeCollectableGroups;

        #region Methods

        #region Unity methods
        private void OnEnable() {
            //runtime init
            uint totalFrequency;
            foreach (CollectableGroup group in CollectableGroups) {
                totalFrequency = 0;

                foreach (CollectableSpawnProbability item in group.PossibleCollectables)
                    totalFrequency += item.Frequency;

                group.TotalFrequency = totalFrequency;
            }

            RuntimeCollectableGroups = new Dictionary<CollectableType, CollectableGroup>();
            foreach (CollectableGroup group in CollectableGroups)
                RuntimeCollectableGroups.Add(group.Type, group);

        }
        #endregion

        #region Public methods
        public Collectable GetRandom(CollectableType type) {
            if (!RuntimeCollectableGroups.ContainsKey(type))
                return null;

            CollectableGroup group = RuntimeCollectableGroups[type];
            CollectableSpawnProbability[] possibleCollectables = group.PossibleCollectables;
            Collectable result = null;

            uint randomBucket = (uint)Random.Range(0, group.TotalFrequency);

            uint curBucketMin = 0;
            for (int i = 0, end = possibleCollectables.Length; i < end; i++) {
                if (curBucketMin <= randomBucket &&
                   randomBucket < curBucketMin + possibleCollectables[i].Frequency) {
                    result = possibleCollectables[i].CollectablePrefab;
                }

                curBucketMin += possibleCollectables[i].Frequency;
            }

            return result;
        }
        #endregion

        #endregion
    }

    [System.Serializable]
    public class CollectableSpawnProbability {
        public Collectable CollectablePrefab;
        public uint Frequency;
    }
    /// <summary>
    /// Foreach collectable type, there is a set of collectable prefabs.
    /// A spawnpoint references the collectable type and instantiates
    /// one of the prefabs, based on its frequency.
    /// 
    /// An example group:
    ///     Type: currency
    ///     Possible Collectables: ({CoinPrefab, 9},{DiamonPrefab, 1})
    ///     TotalFrequency: 10
    /// 
    /// When SpawnRandom(Type=Currency) is called, the coin has a 90% chance of spawning. The diamon has 10% chance of spawning.
    /// </summary>
    [System.Serializable]
    public class CollectableGroup {
        public CollectableType Type;
        public CollectableSpawnProbability[] PossibleCollectables;
        [HideInInspector]
        public uint TotalFrequency;
    }
}