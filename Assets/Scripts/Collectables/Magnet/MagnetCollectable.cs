using UnityEngine;

namespace KyleStankovich.BattleDash {
    /// <summary>
    /// Collectable that acts as a magnet. This will attract any other collectable to the player for X amount of seconds.
    /// </summary>
    [AddComponentMenu("Battle Dash/Collectables/Magnet Collectable")]
    public class MagnetCollectable : Collectable {
        #region Methods

        #region Public methods
        /// <summary>
        /// Activates the magnet power up which attracts other power ups to the player.
        /// </summary>
        public override void OnPickup() {
            base.OnPickup();

            //Find the magnet logic on the player and activate it
            MagnetLogic mLogic = LevelManager.Instance.CurrentCharacter.GetComponentInChildren<MagnetLogic>();
            if(mLogic != null)
                mLogic.Activate();
        }
        #endregion

        #endregion
    }
}
