using UnityEngine;

namespace KyleStankovich.BattleDash {

    [AddComponentMenu("Battle Dash/Collectables/Bomb Collectable")]
    public class BombCollectable : Collectable {
        #region Methods

        #region Public methods
        /// <summary>
        /// Activates the magnet power up which attracts other power ups to the player.
        /// </summary>
        public override void OnPickup() {
            base.OnPickup();

            //Find the magnet logic on the player and activate it
            BombLogic mLogic = LevelManager.Instance.CurrentCharacter.GetComponentInChildren<BombLogic>();
            if(mLogic != null)
                mLogic.Activate();
        }
        #endregion

        #endregion
    }
}
