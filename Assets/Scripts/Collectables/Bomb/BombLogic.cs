using System.Collections.Generic;
using UnityEngine;
using KyleStankovich.Utils;


//note: goes on Player GO 
namespace KyleStankovich.BattleDash {

    [AddComponentMenu("Battle Dash/Collectables/Logic/Bomb Logic")]
    public class BombLogic : PowerupLogic {
        public BoxCollider BombCollider;

        #region Methods

        #region Unity methods


        #endregion

        #region Public methods
        public override void Activate() {
            base.Activate();

           RaycastHit[] collisions= Physics.BoxCastAll(BombCollider.bounds.center, BombCollider.bounds.extents, transform.forward, Quaternion.identity, 0);
            
            //todo: (wayde) spawn particle effects and stuff to make destruction look good
            foreach (RaycastHit hit in collisions) {
                if (hit.collider.gameObject.tag.Equals("Obstacle")) {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        #endregion

        #region Protected methods

        #endregion

        #endregion
    }
}
