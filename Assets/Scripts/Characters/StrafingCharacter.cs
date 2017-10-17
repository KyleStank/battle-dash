using UnityEngine;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Class of the character that moves left, right, up, and down.
    /// </summary>
    [AddComponentMenu("Battle Dash/Characters/Strafing Character")]
    public class StrafingCharacter : PlayableCharacter {
        [SerializeField]
        float laneSwitchAmount = 2.5f;

        public float LaneSwitchDuration;
        bool isSwitchingLanes;
        float currentLaneSwitchTimer;
        float laneSwitchStartX;
        float laneSwitchTargetX;

        public float SlideDuration;
        public CapsuleCollider DestinationCollider;
        public CapsuleCollider RunCollider;
        public CapsuleCollider SlideCollider;
        bool isSliding;
        float currentSlideTimer;

        float damping = 0.0f;
        
        public Transform GroundRayOrigin;
        public float GroundRayLength;
        public float GroundRayResolutionLength;
        public LayerMask GroundLayer;

        public float GravityAcceleration;

        public bool IsJumping;
        public float JumpSpeed;
        public float MoveSpeed = .01f;

        #region Methods

        #region Unity methods
        protected virtual void FixedUpdate() {
            if (canMove)
                HandleMovement();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Makes the character jump.
        /// </summary>
        public override void UpMotion() {
            base.UpMotion();

            //print("Jump!");
            if (!isGrounded)
                return;

            Velocity.y = JumpSpeed;
            IsJumping = true;
            isGrounded = false;

            //Set animation
            SetTriggerAnimation("jump");
        }

        /// <summary>
        /// Makes the character move left smoothly.
        /// </summary>
        public override void LeftMotion() {
            base.LeftMotion();

            if (isSwitchingLanes)
                return;

            //print("Move left!");
            Vector3 newPos = transform.position;
            newPos.x -= laneSwitchAmount;

            //Only move if there are no obstacles in the way
            if (!CheckForObstacle(Vector3.left, laneSwitchAmount) && transform.position.x > -laneSwitchAmount) {

                isSwitchingLanes = true;
                currentLaneSwitchTimer = 0;
                laneSwitchStartX = transform.position.x;
                laneSwitchTargetX = newPos.x;
                

                SetBooleanAnimation("movingLeft", true);
                //Play animation
                //PlayerManager.SetTriggerAnimation("switchLeft");
            } else {
                LeftObstacleHit();
            }
        }

        /// <summary>
        /// Make the character move right smoothly.
        /// </summary>
        public override void RightMotion() {
            base.RightMotion();
            
            if (isSwitchingLanes)
                return;

            //print("Move right!");
            Vector3 newPos = transform.position;
            newPos.x += laneSwitchAmount;

            //Only move if there are no obstacles in the way
            if (!CheckForObstacle(Vector3.right, laneSwitchAmount) && transform.position.x < laneSwitchAmount) {
                //transform.position = newPos;
                
                isSwitchingLanes = true;
                currentLaneSwitchTimer = 0;
                laneSwitchStartX = transform.position.x;
                laneSwitchTargetX = newPos.x;
                

                SetBooleanAnimation("movingRight", true);
                //Play animation
                //PlayerManager.SetTriggerAnimation("switchRight");
            } else {
                RightObstacleHit();
            }
        }

        public override void DownMotion() {
            base.DownMotion();
            if (IsJumping || isSliding)
                return;


            isSliding = true;
            currentSlideTimer = 0;
            CopyCapsuleCollider(SlideCollider,DestinationCollider);

            SetTriggerAnimation("slide");
        }


        protected override void Update() {
            //do not use base ground checking

            if (isSwitchingLanes) {
                currentLaneSwitchTimer += Time.deltaTime;
                float ipo = currentLaneSwitchTimer / LaneSwitchDuration;

                if (currentLaneSwitchTimer >= LaneSwitchDuration) {
                    isSwitchingLanes = false;
                    currentLaneSwitchTimer = 0;
                    ipo = 1;
                    SetBooleanAnimation("movingLeft", false);
                    SetBooleanAnimation("movingRight", false);
                }

                float ipoX = Mathf.Lerp(laneSwitchStartX, laneSwitchTargetX, ipo);
                transform.position = new Vector3(ipoX, transform.position.y, transform.position.z);
            }
            if (isSliding) {
                currentSlideTimer += Time.deltaTime;
                if (currentSlideTimer >= SlideDuration) {
                    isSliding = false;
                    CopyCapsuleCollider(RunCollider, DestinationCollider);
                }
            }
        }
        /// <summary>
        /// Makes the character's movement stop.
        /// </summary>
        public override void StopMoving() {
            base.StopMoving();
            
            Velocity = Vector3.zero;
            damping = 0.0f;
            anim.SetFloat("runSpeed", damping);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Handles the character movement.
        /// </summary>
        void HandleMovement() {
            Velocity += -transform.up * GravityAcceleration;

            //ensure player forward speed is consistent (always equal to MoveSpeed * damping)
            float forwardSpeed = Vector3.Dot(Velocity, transform.forward);
            Velocity -= forwardSpeed * transform.forward;
            Velocity += transform.forward * MoveSpeed * damping;

            transform.position += Velocity;
            UpdateGroundState();
            //Handle the animation
            HandleAnimation();
        }

        /// <summary>
        /// Handles the animation for the running of the character.
        /// </summary>
        void HandleAnimation() {
            //Smooth out the movement
            if (damping < 1.0f) {
                damping += Time.deltaTime;
                damping = Mathf.Clamp(damping, 0.0f, 1.0f);
            }

            //Play the run animation
            SetFloatAnimation("runSpeed", damping);
        }

        /// <summary>
        /// Checks if there is an obstacle in the given direction of the character. Uses a raycast to do this.
        /// </summary>
        /// <param name="rayDirection">Direction of ray.</param>
        /// <param name="maxRayLength">Length of ray.</param>
        /// <returns>True if there is an obstacle, false if not.</returns>
        bool CheckForObstacle(Vector3 rayDirection, float maxRayLength) {
            //Set ray's position
            Vector3 rayPos = transform.position;
            rayPos.y = Height / 2.0f;

#if UNITY_EDITOR
            Debug.DrawRay(rayPos, rayDirection * maxRayLength, Color.white, 1.5f);
#endif

            //Check if an obstacle is next to the character.
            RaycastHit hitInfo;
            if (Physics.Raycast(rayPos, rayDirection, out hitInfo, maxRayLength))
                return hitInfo.transform.tag == "Obstacle";

            return false;
        }

        /// <summary>
        /// Character hits an obstacle on the left.
        /// </summary>
        void LeftObstacleHit() {
            print("Left obstacle hit!");
        }

        /// <summary>
        /// Character hits an obstacle on the right.
        /// </summary>
        void RightObstacleHit() {
            print("Right obstacle hit!");
        }

        private void UpdateGroundState() {
            RaycastHit hit;
            if (Physics.Raycast(GroundRayOrigin.position, -Vector3.up, out hit, GroundRayLength, GroundLayer.value)) {
                /*
                 * When jumping, only resolve the ground collision if the player goes through the ground.
                 * When not jumping, snap or magnetize the player to the ground.
                 */
                if (!IsJumping || hit.distance < GroundRayResolutionLength) {

                    float resolveOverlapOffset = GroundRayResolutionLength - hit.distance;
                    transform.position += Vector3.up * resolveOverlapOffset;
                    Velocity.y = 0;
                    IsJumping = false;
                    isGrounded = true;
                }
            }
            else isGrounded = false;
        }
        private void OnDrawGizmosSelected() {
            if (GroundRayOrigin) {
                Debug.DrawRay(GroundRayOrigin.position, -Vector3.up * GroundRayResolutionLength, Color.green);
                Debug.DrawRay(GroundRayOrigin.position - Vector3.up * GroundRayResolutionLength, -Vector3.up * (GroundRayLength - GroundRayResolutionLength), Color.red);
            }
        }
        private void CopyCapsuleCollider(CapsuleCollider src,CapsuleCollider dst) {
            dst.radius = src.radius;
            dst.height = src.height;
            dst.direction = src.direction;
            dst.center = src.center;
        }
        #endregion

        #endregion
    }
}
