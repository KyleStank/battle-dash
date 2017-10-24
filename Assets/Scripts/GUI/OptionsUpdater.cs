using UnityEngine;
using UnityEngine.UI;
using KyleStankovich.Utils;

namespace KyleStankovich.BattleDash {
    /// <summary>
    /// Class that handles updating the options menu.
    /// </summary>
    [AddComponentMenu("Battle Dash/GUI/Options Menu Updater")]
    public class OptionsUpdater : MonoBehaviour {
        [Header("UI")]
        [SerializeField]
        Slider masterSlider = null;
        [SerializeField]
        Slider musicSlider = null;
        [SerializeField]
        Slider effectSlider = null;

        #region Methods

        #region Unity methods
        void Start() {
            //Load slider values
            //Master
            if(masterSlider != null)
                masterSlider.value = PlayerPrefs.GetFloat(Constants.PREF_MASTERAUDIOLEVEL, 1.0f);

            //Music
            if(musicSlider != null)
                musicSlider.value = PlayerPrefs.GetFloat(Constants.PREF_MUSICAUDIOLEVEL, 1.0f);

            //Sound effects
            if(effectSlider != null)
                effectSlider.value = PlayerPrefs.GetFloat(Constants.PREF_EFFECTAUDIOLEVEL, 1.0f);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Updates and saves the master volume to match the settings.
        /// </summary>
        public void UpdateMasterVolume() {
            if(masterSlider != null) {
                AudioListener.volume = masterSlider.value;
                PlayerPrefs.SetFloat(Constants.PREF_MASTERAUDIOLEVEL, masterSlider.value);
            }
        }

        /// <summary>
        /// Updates and saves the music volume to match the settings.
        /// </summary>
        public void UpdateMusicVolume() {
            if(musicSlider != null && AudioManager.Instance.MusicAudio != null) {
                AudioManager.Instance.MusicAudio.volume = musicSlider.value;
                PlayerPrefs.SetFloat(Constants.PREF_MUSICAUDIOLEVEL, musicSlider.value);
            }
        }

        /// <summary>
        /// Updates and saves the sound effect volume to match the settings.
        /// </summary>
        public void UpdateEffectVolume() {
            if(effectSlider != null && AudioManager.Instance.EffectAudio != null) {
                AudioManager.Instance.EffectAudio.volume = effectSlider.value;
                PlayerPrefs.SetFloat(Constants.PREF_EFFECTAUDIOLEVEL, effectSlider.value);
            }
        }
        #endregion

        #endregion
    }
}
