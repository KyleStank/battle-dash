using System.Collections.Generic;
using UnityEngine;
using TurmoilStudios.Utils;

//add to GO: Player.PowerUpColliders.MagnetCollider.
//note: PowerUpCollider GO has a rigidbody to prevent the Player collider from receiving the magnet collider's trigger event.
namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Takes care of the logic for the magnet that attracts other power ups to the player during runtime.
    /// </summary>
    [AddComponentMenu("Battle Dash/Collectables/Logic/Magnet Logic")]
    public class MagnetLogic : PowerupLogic {
        [SerializeField]
        Transform attractionPoint = null;

        [SerializeField]
        [Range(0, 1)]
        public float lerpPerStep = 0.1f;

        List<Transform> attractedCollectables = new List<Transform>();

        #region Methods

        #region Unity methods
        void OnTriggerEnter(Collider col) {
            //Remove all null entries from the attracted collectables list
            RemoveNullsFromAttraction();

            if(!isActive || col.GetComponent<Collectable>() == null || attractedCollectables.Contains(col.transform)) {
                return;
            }

            //Attract the collided with collectable
            attractedCollectables.Add(col.transform);
        }
        #endregion

        #region Protected methods
        protected override void PowerupUpdate() {
            //Lerp items towards attraction point
            for(int i = 0; i < attractedCollectables.Count; i++) {
                //TODO this gives a null error. Reproduce it, then fix it.
                attractedCollectables[i].position = Vector3.Lerp(attractedCollectables[i].position, attractionPoint.position, lerpPerStep);
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Removes any null entries inside of the attracted collectables.
        /// </summary>
        void RemoveNullsFromAttraction() {
            for(int i = attractedCollectables.Count - 1; i >= 0; i--) {
                if(attractedCollectables[i] == null) {
                    attractedCollectables.RemoveAt(i);
                }
            }
        }
        #endregion

        #endregion
    }
}
