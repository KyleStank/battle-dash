using UnityEngine;

namespace TurmoilStudios.Utils {
    /// <summary>
    /// Used for invoking an event when an object passes through it's bounadaries.
    /// </summary>
    public class OnTriggerEventActivate : MonoBehaviour {
        [SerializeField]
        [Tooltip("Name of the event to invoke.")]
        protected string eventName = "";
        [SerializeField]
        [Tooltip("Tag of the object that will trigger event when passed through.")]
        protected string objectTag = "";
        [SerializeField]
        protected bool disableOnActivation = true;
        [SerializeField]
        [Tooltip("If true, the tag above will be ignored and anything that passes through the object will trigger the event.")]
        protected bool ignoreTag = false;

        protected Collider thisCollider;

#if UNITY_EDITOR
        [Header("Debugging")]
        [SerializeField]
        Color boundaryColor = Color.red;
#endif

        #region Methods

        #region Unity methods
        void Awake() {
            thisCollider = GetComponent<Collider>();

            if(thisCollider != null)
                thisCollider.isTrigger = true;
        }

        void OnTriggerEnter(Collider col) {
            if(col.tag == objectTag || ignoreTag) {
                print("Trigger the \"" + eventName.ToString() + "\" event!");
                EventManager.TriggerEvent(eventName.ToString());

                if(disableOnActivation)
                {
                    gameObject.SetActive(false);
                }
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos() {
            thisCollider = GetComponent<Collider>();

            Gizmos.color = boundaryColor;
            Gizmos.DrawWireCube(thisCollider.bounds.center, thisCollider.bounds.size);
        }
#endif
        #endregion

        #endregion
    }
}
