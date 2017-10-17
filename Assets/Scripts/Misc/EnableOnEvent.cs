using UnityEngine;
using TurmoilStudios.Utils;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Enables or disables this game object when an event is fired.
    /// </summary>
    [AddComponentMenu("Battle Dash/Misc/Enable on Event")]
    public class EnableOnEvent : MonoBehaviour {
        [SerializeField]
        string eventName;
        [SerializeField]
        bool enableGameObject = true;

        #region Methods

        #region Unity methods
        void Awake() {
            //Subscribe to events
            EventManager.StartListening(eventName, () => gameObject.SetActive(enableGameObject));
        }

        void OnDisable() {
            //Unsubscribe from events
            EventManager.StopListening(eventName, () => gameObject.SetActive(enableGameObject));
        }
        #endregion

        #endregion
    }
}
