using UnityEngine;
using UnityEngine.UI;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Class that handles updating the boss battle menu.
    /// </summary>
    [AddComponentMenu("Battle Dash/GUI/Boss Battle Menu Updater")]
    public class BattleUpdater : MonoBehaviour {
        [Header("UI")]
        [SerializeField]
        Slider characterHealthSlider = null;
        [SerializeField]
        Slider bossHealthSlider = null;

        #region Methods

        #region Unity methods
        void Update() {
            if(GameManager.Instance.Status == GameManager.GameStatus.BossBattleInProgress)
                UpdateBossBattleUI();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Updates the boss battle's GUI.
        /// </summary>
        void UpdateBossBattleUI() {
            //Update the current character
            if(BattleManager.Instance.PlayerCharacter != null) {
                float currentHealth = BattleManager.Instance.PlayerCharacter.Health;
                float maxHealth = BattleManager.Instance.PlayerCharacter.MaxHealth;
                GUIManager.Instance.UpdateSliderSmoothly(characterHealthSlider, currentHealth, maxHealth);
            }

            //Update the current boss
            if(BattleManager.Instance.BossCharacter != null) {
                float currentHealth = BattleManager.Instance.BossCharacter.Health;
                float maxHealth = BattleManager.Instance.BossCharacter.MaxHealth;
                GUIManager.Instance.UpdateSliderSmoothly(bossHealthSlider, currentHealth, maxHealth);
            }
        }
        #endregion

        #endregion
    }
}
