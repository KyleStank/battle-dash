using UnityEngine;

namespace TurmoilStudios.BattleDash {

    [AddComponentMenu("Battle Dash/Collectables/Bash Collectable")]
    public class BashCollectable : Collectable {
        #region Methods

        #region Public methods
        /// <summary>
        /// Activates the magnet power up which attracts other power ups to the player.
        /// </summary>
        public override void OnPickup() {
            base.OnPickup();

            //Find the magnet logic on the player and activate it
            BashLogic mLogic = LevelManager.Instance.CurrentCharacter.GetComponentInChildren<BashLogic>();
            if(mLogic != null)
                mLogic.Activate();
        }
        #endregion

        #endregion
    }
}
