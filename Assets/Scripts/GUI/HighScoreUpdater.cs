using UnityEngine;
using UnityEngine.UI;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Updates the high score menu.
    /// </summary>
    [AddComponentMenu("Battle Dash/GUI/High Score Menu Updater")]
    public sealed class HighScoreUpdater : MonoBehaviour {
        [Header("UI")]
        [SerializeField]
        Text highScoreText = null;

        #region Methods

        #region Unity methods
        void OnEnable() {
            if(highScoreText == null)
                return;

            //Set high score if it was beaten
            if(GameManager.Instance.Points >= GameManager.Instance.HighScore) {
                GameManager.Instance.SetHighScore((int)Mathf.Round(GameManager.Instance.Points));
                PlayerPrefs.SetInt(Constants.PREF_HIGHSCORE, GameManager.Instance.HighScore);
            }

            highScoreText.text = GameManager.Instance.HighScore.ToString();
        }

        void OnDisable() {
            //Reset in game score
            GameManager.Instance.SetPoints(0.0f);
        }
        #endregion

        #endregion
    }
}
