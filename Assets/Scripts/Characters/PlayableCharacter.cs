using UnityEngine;
using KyleStankovich.Utils;

namespace KyleStankovich.BattleDash
{
    /// <summary>
    /// This class should be extended for all possible types of playable characters.
    /// This allows us to add weird and different gameplay if needed.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayableCharacterInput))]
    public class PlayableCharacter : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Animator that the character will use for Animation.")]
        [SerializeField]
        protected Animator m_Animator = null;

        protected CharacterController m_CharacterController = null;
        protected Collider m_Collider = null;
        protected Vector3 m_Velocity = Vector3.zero;
        protected bool m_CanMove = false;

        [Header("Base Settings")]
        [SerializeField]
        protected Vector3 m_InitialPosition = Vector3.zero;
        [SerializeField]
        protected Quaternion m_InitialRotation = Quaternion.identity;
        [SerializeField]
        protected float m_MinMoveSpeed = 10.0f;
        [SerializeField]
        protected float m_MaxMoveSpeed = 10.0f;
        [SerializeField]
        protected float m_JumpSpeed = 10.0f;
        [SerializeField]
        protected bool m_IsJumping = false;

        #region Properties
        /// <summary>
        /// Returns the initial position of the character.
        /// </summary>
        public Vector3 InitialPosition
        {
            get { return m_InitialPosition; }
        }

        /// <summary>
        /// Returns the initial rotation of the character.
        /// </summary>
        public Quaternion InitialRotation
        {
            get { return m_InitialRotation; }
        }

        /// <summary>
        /// Tells us if the player is able to move or not.
        /// </summary>
        public bool CanMove
        {
            get { return m_CanMove; }
            set { m_CanMove = value; }
        }

        /// <summary>
        /// Returns the height of the character by getting the bounds of its collider.
        /// </summary>
        public float Height
        {
            get { return m_Collider.bounds.size.y; }
        }
        #endregion

        #region Methods

        #region Unity methods
        protected virtual void Awake()
        {
            /* Initialize character */
            //Grab references
            m_Collider = GetComponent<Collider>();
            m_CharacterController = GetComponent<CharacterController>();

            //Null checks
            if(m_Animator == null)
            {
                Debug.LogError("No Animator was assigned! Make sure to assign it in the Inspector!");
            }

            if(m_Collider == null)
            {
                Debug.LogError("No collider was found! Make sure to attach on to the GameObject in the inspector!");
            }
        }

        protected virtual void OnEnable()
        {
            //Subscribe to events
            EventManager.StartListening(Constants.EVENT_GAMESTART, () => StartMoving());

            EventManager.StartListening(Constants.EVENT_INPUTUP, UpMotion);
            EventManager.StartListening(Constants.EVENT_INPUTDOWN, DownMotion);
            EventManager.StartListening(Constants.EVENT_INPUTLEFT, LeftMotion);
            EventManager.StartListening(Constants.EVENT_INPUTRIGHT, RightMotion);

            EventManager.StartListening(Constants.EVENT_OBSTACLEHIT, StopMoving);

            EventManager.StartListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, StopMoving);
            EventManager.StartListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, ResetXYPosition);
        }

        protected virtual void OnDisable()
        {
            //Unsubscribe from events
            EventManager.StopListening(Constants.EVENT_GAMESTART, () => StartMoving());

            EventManager.StopListening(Constants.EVENT_INPUTUP, UpMotion);
            EventManager.StopListening(Constants.EVENT_INPUTDOWN, DownMotion);
            EventManager.StopListening(Constants.EVENT_INPUTLEFT, LeftMotion);
            EventManager.StopListening(Constants.EVENT_INPUTRIGHT, RightMotion);

            EventManager.StopListening(Constants.EVENT_OBSTACLEHIT, StopMoving);

            EventManager.StopListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, StopMoving);
            EventManager.StopListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, ResetXYPosition);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Resets the character's position to the initial position.
        /// </summary>
        public virtual void ResetPosition()
        {
            transform.position = m_InitialPosition;
        }

        /// <summary>
        /// Resets the character's X and Y position to the initial X and Y position.
        /// </summary>
        public virtual void ResetXYPosition()
        {
            Vector3 pos = transform.position;

            pos.x = m_InitialPosition.x;
            pos.y = m_InitialPosition.y;

            transform.position = pos;
        }

        /// <summary>
        /// Resets the character's rotation to the initial rotation.
        /// </summary>
        public virtual void ResetRotation()
        {
            transform.rotation = m_InitialRotation;
        }

        /// <summary>
        /// Makes the character start moving.
        /// </summary>
        public virtual void StartMoving()
        {
           m_CanMove = true;
        }

        /// <summary>
        /// Makes the character stop moving.
        /// </summary>
        public virtual void StopMoving()
        {
            m_CanMove = false;
        }

        #region Movement methods
        /// <summary>
        /// Invokes when the character moves up.
        /// </summary>
        public virtual void UpMotion()
        {
            
        }

        /// <summary>
        /// Invokes when the character moves down.
        /// </summary>
        public virtual void DownMotion()
        {
            
        }

        /// <summary>
        /// Invokes when the character moves to the left.
        /// </summary>
        public virtual void LeftMotion()
        {
            
        }

        /// <summary>
        /// Invokes when the character moves to the right.
        /// </summary>
        public virtual void RightMotion()
        {
            
        }
        #endregion

        #endregion

        #endregion
    }
}
