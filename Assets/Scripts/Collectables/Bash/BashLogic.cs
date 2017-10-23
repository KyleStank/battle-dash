using UnityEngine;

//note: goes on Player GO
namespace KyleStankovich.BattleDash {
    [AddComponentMenu("Battle Dash/Collectables/Logic/Bash Logic")]
    public class BashLogic : PowerupLogic {
        
        #region Methods

        #region Unity methods
        void OnTriggerEnter(Collider col) {

            if(!isActive || !col.tag.Equals("Obstacle"))
                return;

            //todo: (wayde) spawn particle effects and stuff to make destruction look good
            Destroy(col.gameObject);
        }
        #endregion

        #region Public methods
        public override void Activate() {
            base.Activate();

            GetComponent<DieOnHit>().IsInvincible = true;
        }
        #endregion

        #region Protected methods
        public override void Deactivate() {
            base.Deactivate();
            
            GetComponent<DieOnHit>().IsInvincible = false;
        }
        #endregion

        #endregion
    }
}
