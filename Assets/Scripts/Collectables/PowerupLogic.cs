using TurmoilStudios.Utils;
using UnityEngine;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Handles the duration and boss battle pausing of the powerup.
    /// </summary>
    public class PowerupLogic : MonoBehaviour {
#if UNITY_EDITOR
        [Tooltip("Toggle to activate/deactivate powerup correctly from Unity editor.")]
        [SerializeField]
        bool _debugUnityActivateToggle;
#endif

        [SerializeField]
        float duration = 5.0f;

        float curTime = 0.0f;
        bool currentlyInBossFight = false;

        public bool isActive = false;

        #region Methods

        #region Unity methods
        void Start() {
            curTime = duration;

            //Temp deactivate powerup when a boss battle begins
            EventManager.StartListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, () => { currentlyInBossFight = true; });
            EventManager.StartListening(Constants.EVENT_BOSSBATTLEENDCOMBAT, () => { currentlyInBossFight = false; });
        }
        
        void Update() {
#if UNITY_EDITOR
            // (wayde) subtle note, keep this block of code so that we can debug the powerup from Unity editor by just toggling _debugUnityActivateToggle
            if (_debugUnityActivateToggle) {
                _debugUnityActivateToggle = false;

                if(!isActive) {
                    Activate();
                } else {
                    Deactivate();
                }
            }
#endif

            //Check if character is in a boss fight
            if (currentlyInBossFight)
                return;

            PowerupUpdate();

            //Deactivate after so long
            curTime -= Time.deltaTime;
            if (curTime <= 0) {
                Deactivate();
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Activates the powerup.
        /// </summary>
        public virtual void Activate() {
            isActive = true;
            curTime = duration;
        }

        /// <summary>
        /// Deactivates the powerup.
        /// </summary>
        public virtual void Deactivate() {
            curTime = 0;
            isActive = false;
        }
        #endregion

        #region Protected methods
        protected virtual void PowerupUpdate() { }
        #endregion

        #endregion
    }
}