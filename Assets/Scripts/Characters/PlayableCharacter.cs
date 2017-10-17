using UnityEngine;
using TurmoilStudios.Utils;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// This class should be extended for all possible types of playable characters.
    /// This allows us to add weird and different gameplay if needed.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayableCharacter : MonoBehaviour {
        protected Animator anim;
        protected new Collider collider;
        protected Vector3 initialPosition;
        protected bool canMove = false;
        protected bool isGrounded = false;
        protected float groundRayLength;
        
        public Vector3 Velocity;

        #region Properties
        /// <summary>
        /// Returns the initial position of the character.
        /// </summary>
        public Vector3 InitialPosition { get { return initialPosition; } }

        /// <summary>
        /// Tells us if the player is able to move or not.
        /// </summary>
        public bool CanMove {
            get { return canMove; }
            set { canMove = value; }
        }
        

        /// <summary>
        /// Returns the height of the character by getting the bounds of its collider.
        /// </summary>
        public float Height { get { return collider.bounds.size.y; } }

        /// <summary>
        /// Returns true if the character is grounded, false otherwise.
        /// </summary>
        public bool IsGrounded { get { return isGrounded; } }
        #endregion

        #region Methods

        #region Unity methods
        protected virtual void Awake() {
            Initialize();
        }

        protected virtual void Update() {
            isGrounded = CheckForGround();
        }

        protected virtual void OnEnable() {
            //Subscribe to events
            EventManager.StartListening(Constants.EVENT_GAMESTART, () => canMove = true);

            EventManager.StartListening(Constants.EVENT_INPUTUP, UpMotion);
            EventManager.StartListening(Constants.EVENT_INPUTDOWN, DownMotion);
            EventManager.StartListening(Constants.EVENT_INPUTLEFT, LeftMotion);
            EventManager.StartListening(Constants.EVENT_INPUTRIGHT, RightMotion);

            EventManager.StartListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, ResetXYPosition);
        }

        protected virtual void OnDisable() {
            //Unsubscribe from events
            EventManager.StopListening(Constants.EVENT_GAMESTART, () => canMove = true);

            EventManager.StopListening(Constants.EVENT_INPUTUP, UpMotion);
            EventManager.StopListening(Constants.EVENT_INPUTDOWN, DownMotion);
            EventManager.StopListening(Constants.EVENT_INPUTLEFT, LeftMotion);
            EventManager.StopListening(Constants.EVENT_INPUTRIGHT, RightMotion);

            EventManager.StopListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, ResetXYPosition);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Sets the initial position of the character.
        /// </summary>
        /// <param name="pos">The new initial position.</param>
        public virtual void SetInitialPosition(Vector3 pos) {
            initialPosition = pos;
        }

        /// <summary>
        /// Resets the character's position to the initial position.
        /// </summary>
        public virtual void ResetPosition() {
            transform.position = initialPosition;
        }

        /// <summary>
        /// Resets the character's X and Y position to the initial X and Y position.
        /// </summary>
        public virtual void ResetXYPosition() {
            Vector3 pos = transform.position;

            pos.x = initialPosition.x;
            pos.y = initialPosition.y;

            transform.position = pos;
        }
        
        /// <summary>
        /// Makes the character start moving.
        /// </summary>
        public virtual void StartMoving() {
            canMove = true;
        }

        /// <summary>
        /// Makes the character stop moving.
        /// </summary>
        public virtual void StopMoving() {
            canMove = false;
            Velocity = Vector3.zero;
        }

        #region Movement methods
        /// <summary>
        /// Invokes when the character moves up.
        /// </summary>
        public virtual void UpMotion() { }

        /// <summary>
        /// Invokes when the character moves down.
        /// </summary>
        public virtual void DownMotion() { }

        /// <summary>
        /// Invokes when the character moves to the left.
        /// </summary>
        public virtual void LeftMotion() { }

        /// <summary>
        /// Invokes when the character moves to the right.
        /// </summary>
        public virtual void RightMotion() { }
        #endregion

        #region Animation methods
        /// <summary>
        /// Sets a trigger for the animator on the character.
        /// </summary>
        /// <param name="triggerName">Name of the trigger to set.</param>
        public void SetTriggerAnimation(string triggerName) {
            if(anim != null)
                anim.SetTrigger(triggerName);
            else
                Debug.LogError("An animator was not found on the character!");
        }

        /// <summary>
        /// Sets a float value for the animator on the character.
        /// </summary>
        /// <param name="floatName">Name of the float</param>
        /// <param name="value">Value to set.</param>
        public void SetFloatAnimation(string floatName, float value) {
            if(anim != null)
                anim.SetFloat(floatName, value);
            else
                Debug.LogError("An animator was not found on the character!");
        }
        public void SetBooleanAnimation(string boolName, bool value) {
             if(anim != null)
                anim.SetBool(boolName, value);
            else
                Debug.LogError("An animator was not found on the character!");
         }
        #endregion

        #endregion

        #region Private methods
        /// <summary>
        /// Initializes some important things for the character.
        /// </summary>
        protected virtual void Initialize() {
            anim = GetComponent<Animator>();
            collider = GetComponent<Collider>();
            SetInitialPosition(transform.position);
            groundRayLength = collider.bounds.extents.y;
        }

        /// <summary>
        /// Detects for a ground below the character.
        /// </summary>
        protected virtual bool CheckForGround() {
            //Set ray's position
            Vector3 rayPos = transform.localPosition;
            rayPos.y = rayPos.y + (collider.bounds.extents.y);
            Vector3 rayDir = -(transform.up);

#if UNITY_EDITOR
            Debug.DrawRay(rayPos, rayDir * groundRayLength, Color.red, 0.01f);
#endif

            //Check if ray hit the ground
            RaycastHit hitInfo;
            if(Physics.Raycast(rayPos, rayDir, out hitInfo, groundRayLength))
                return hitInfo.transform.tag == "Ground";

            return false;
        }
        #endregion

        #endregion
    }
}
