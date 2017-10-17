using UnityEngine;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Collectable that is the coin, one of the most essential collectables in the game.
    /// </summary>
    [AddComponentMenu("Battle Dash/Collectables/Coin Collectable")]
    public class CoinCollectable : Collectable {
        [SerializeField]
        int coinsToAdd = 1;

        #region Methods

        #region Public methods
        /// <summary>
        /// Picks up a coin(s).
        /// </summary>
        public override void OnPickup() {
            base.OnPickup();

            //Add the coin(s)
            GameManager.Instance.AddCoins(coinsToAdd);
        }
        #endregion

        #endregion
    }
}
