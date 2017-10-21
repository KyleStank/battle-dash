using UnityEngine;

namespace TurmoilStudios.Utils {
    /// <summary>
    /// Used for invoking an event when an object passes through it's bounadaries.
    /// </summary>
    [RequireComponent(typeof(Collider))]
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
        private bool m_AlreadyActivated = false;

#if UNITY_EDITOR
        [Header("Debugging")]
        [SerializeField]
        Color boundaryColor = Color.red;
#endif

        #region Methods

        #region Unity methods
        private void Awake()
        {
            thisCollider = GetComponent<Collider>();

            if(thisCollider != null)
                thisCollider.isTrigger = true;
        }

        private void OnEnable()
        {
            m_AlreadyActivated = false;
        }

        private void OnTriggerEnter(Collider col)
        {
            if(m_AlreadyActivated)
            {
                return;
            }

            if(col.tag == objectTag || ignoreTag)
            {
                print("Trigger the \"" + eventName.ToString() + "\" event!");
                EventManager.TriggerEvent(eventName.ToString());
                m_AlreadyActivated = true;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            thisCollider = GetComponent<Collider>();

            Gizmos.color = boundaryColor;
            Gizmos.DrawWireCube(thisCollider.bounds.center, thisCollider.bounds.size);
        }
#endif
        #endregion

        #endregion
    }
}
