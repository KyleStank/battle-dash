using UnityEngine;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Determines the type of the collectable.
    /// </summary>
    public enum CollectableType {
        Currency,
        Powerup
    }

    /// <summary>
    /// A spawn point for a collectable.
    /// </summary>
    public class CollectableSpawnPoint : MonoBehaviour {
        public CollectableType Type;
    }
}
