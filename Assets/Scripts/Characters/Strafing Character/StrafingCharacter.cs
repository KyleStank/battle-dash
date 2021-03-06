﻿using UnityEngine;
using KyleStankovich.Utils;

namespace KyleStankovich.BattleDash
{
    /// <summary>
    /// Class of the character that moves left, right, up, and down.
    /// </summary>
    [AddComponentMenu("Battle Dash/Characters/Strafing Character")]
    public class StrafingCharacter : PlayableCharacter
    {
        [Header("Strafing Character")]
        [SerializeField]
        private float m_MoveSpeedAccelerationTime = 60.0f;

        [SerializeField]
        private float m_LaneSwitchAmount = 2.5f;
        [SerializeField]
        private float m_LaneSwitchDuration = 0.2f;

        [SerializeField]
        private float m_SlideDuration = 0.2f;

        [SerializeField]
        private CapsuleCollider m_DestinationCollider = null;
        [SerializeField]
        private CapsuleCollider m_RunCollider = null;
        [SerializeField]
        private CapsuleCollider m_SlideCollider = null;

        private float m_MoveSpeedTime = 0.0f;
        private float m_LastVelocity = 0.0f;

        private bool m_IsSwitchingLanes = false;
        private float m_CurrentLaneSwitchTimer = 0.0f;
        private float m_LaneSwitchStartX = 0.0f;
        private float m_LaneSwitchTargetX = 0.0f;

        private bool m_IsSliding = false;
        private float m_CurrentSlideTimer = 0.0f;

        private float m_RunningDamping = 0.0f;
        private float m_Damping = 0.0f;

        #region Methods

        #region Unity methods
        protected override void Awake()
        {
            base.Awake();

            m_LastVelocity = m_MinMoveSpeed;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            //Subscribe to events
            EventManager.StartListening(Constants.EVENT_OBSTACLEHIT, () => m_LastVelocity = m_MinMoveSpeed);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            //Unsubscribe from events
            EventManager.StopListening(Constants.EVENT_OBSTACLEHIT, () => m_LastVelocity = m_MinMoveSpeed);
        }

        protected void Update()
        {
            ApplyGravity();

            if(m_CanMove)
            {
                //Determine if the player is jumping or not
                m_IsJumping = !(m_CharacterController.isGrounded);

                HandleMovement();

                //Update animations
                m_RunningDamping += (m_Velocity.z) * Time.deltaTime;
                m_RunningDamping = Mathf.Clamp(m_RunningDamping, 0.0f, 1.0f);
            }
            else
            {
                m_RunningDamping = 0.0f;
            }

            //Set aniamtion parameters
            if(m_Animator != null)
            {
                m_Animator.SetFloat("MoveSpeed", m_RunningDamping);
                m_Animator.SetBool("IsJumping", m_IsJumping);
            }

            //Switch lanes
            if(m_IsSwitchingLanes)
            {
                m_CurrentLaneSwitchTimer += Time.deltaTime;
                float ipo = m_CurrentLaneSwitchTimer / m_LaneSwitchDuration;

                if(m_CurrentLaneSwitchTimer >= m_LaneSwitchDuration)
                {
                    m_IsSwitchingLanes = false;
                    m_CurrentLaneSwitchTimer = 0;
                    ipo = 1;
                }

                float ipoX = Mathf.Lerp(m_LaneSwitchStartX, m_LaneSwitchTargetX, ipo);
                transform.position = new Vector3(ipoX, transform.position.y, transform.position.z);
            }

            //Slide
            if(m_IsSliding)
            {
                m_CurrentSlideTimer += Time.deltaTime;
                if(m_CurrentSlideTimer >= m_SlideDuration)
                {
                    m_IsSliding = false;
                    CopyCapsuleCollider(m_RunCollider, m_DestinationCollider);
                }
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Makes the character start moving forward.
        /// </summary>
        public override void StartMoving()
        {
            //Set the velocity
            m_Velocity.z = m_LastVelocity;
            m_MoveSpeedTime = 0.0f;

            base.StartMoving();
        }

        /// <summary>
        /// Makes the character's movement stop.
        /// </summary>
        public override void StopMoving()
        {
            m_LastVelocity = m_Velocity.z;
            m_Velocity = Vector3.zero;
            m_Damping = 0.0f;

            base.StopMoving();
        }

        /// <summary>
        /// Makes the character jump.
        /// </summary>
        public override void UpMotion()
        {
            base.UpMotion();

            //print("Jump!");
            if(!m_CharacterController.isGrounded)
                return;

            m_Velocity.y = m_JumpSpeed;
            m_IsJumping = true;
        }

        /// <summary>
        /// Makes the character move left smoothly.
        /// </summary>
        public override void LeftMotion()
        {
            base.LeftMotion();

            if(m_IsSwitchingLanes)
                return;

            //print("Move left!");
            Vector3 newPos = transform.position;
            newPos.x -= m_LaneSwitchAmount;

            //Only move if there are no obstacles in the way
            if(!CheckForObstacle(Vector3.left, m_LaneSwitchAmount) && transform.position.x > -m_LaneSwitchAmount)
            {

                m_IsSwitchingLanes = true;
                m_CurrentLaneSwitchTimer = 0;
                m_LaneSwitchStartX = transform.position.x;
                m_LaneSwitchTargetX = newPos.x;

                //Play animation
                if(m_Animator != null)
                {
                    m_Animator.ResetTrigger("RollLeft");
                    m_Animator.ResetTrigger("RollRight");
                    m_Animator.SetTrigger("RollLeft");
                }
            }
            else
            {
                LeftObstacleHit();
            }
        }

        /// <summary>
        /// Make the character move right smoothly.
        /// </summary>
        public override void RightMotion()
        {
            base.RightMotion();

            if(m_IsSwitchingLanes)
                return;

            //print("Move right!");
            Vector3 newPos = transform.position;
            newPos.x += m_LaneSwitchAmount;

            //Only move if there are no obstacles in the way
            if(!CheckForObstacle(Vector3.right, m_LaneSwitchAmount) && transform.position.x < m_LaneSwitchAmount)
            {
                //transform.position = newPos;

                m_IsSwitchingLanes = true;
                m_CurrentLaneSwitchTimer = 0;
                m_LaneSwitchStartX = transform.position.x;
                m_LaneSwitchTargetX = newPos.x;
                
                //Play animation
                if(m_Animator != null)
                {
                    m_Animator.ResetTrigger("RollLeft");
                    m_Animator.ResetTrigger("RollRight");
                    m_Animator.SetTrigger("RollRight");
                }
            }
            else
            {
                RightObstacleHit();
            }
        }

        public override void DownMotion()
        {
            base.DownMotion();
            if(m_IsJumping || m_IsSliding)
                return;


            m_IsSliding = true;
            m_CurrentSlideTimer = 0;
            CopyCapsuleCollider(m_SlideCollider, m_DestinationCollider);

            //SetTriggerAnimation("slide");
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Applies gravity to the character.
        /// </summary>
        private void ApplyGravity()
        {
            if(!m_CharacterController.isGrounded)
            {
                m_Velocity.y += Physics.gravity.y * Time.deltaTime;
            }
        }

        
        /// <summary>
        /// Handles the character movement.
        /// </summary>
        private void HandleMovement()
        {
            //currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, time / accelerationTime );
            m_Velocity.z = Mathf.SmoothStep(m_LastVelocity, m_MaxMoveSpeed, m_MoveSpeedTime / m_MoveSpeedAccelerationTime);
            m_CharacterController.Move(m_Velocity * Time.deltaTime);
            m_MoveSpeedTime += Time.deltaTime;

            //ensure player forward speed is consistent (always equal to MoveSpeed * damping)
            //float forwardSpeed = Vector3.Dot(m_Velocity, transform.forward);
            //m_Velocity -= forwardSpeed * transform.forward;
            //m_Velocity += transform.forward * m_MoveSpeed * m_Damping;

            //Set the new position
            //transform.position += m_Velocity * Time.fixedDeltaTime;

            //Make sure character sticks to ground when needed
            //StickToGround();

            //Handle the animation
            //HandleAnimation();
        }

        /// <summary>
        /// Handles the animation for the running of the character.
        /// </summary>
        private void HandleAnimation()
        {
            //Smooth out the movement
            if(m_Damping < 1.0f)
            {
                m_Damping += Time.deltaTime;
                m_Damping = Mathf.Clamp(m_Damping, 0.0f, 1.0f);
            }

            //Play the run animation
            //SetFloatAnimation("runSpeed", damping);
        }

        /// <summary>
        /// Checks if there is an obstacle in the given direction of the character. Uses a raycast to do this.
        /// </summary>
        /// <param name="rayDirection">Direction of ray.</param>
        /// <param name="maxRayLength">Length of ray.</param>
        /// <returns>True if there is an obstacle, false if not.</returns>
        private bool CheckForObstacle(Vector3 rayDirection, float maxRayLength)
        {
            //Set ray's position
            Vector3 rayPos = transform.position;
            rayPos.y = Height / 2.0f;

#if UNITY_EDITOR
            Debug.DrawRay(rayPos, rayDirection * maxRayLength, Color.white, 1.5f);
#endif

            //Check if an obstacle is next to the character.
            RaycastHit hitInfo;
            if(Physics.Raycast(rayPos, rayDirection, out hitInfo, maxRayLength))
                return hitInfo.transform.tag == "Obstacle";

            return false;
        }

        /// <summary>
        /// Character hits an obstacle on the left.
        /// </summary>
        private void LeftObstacleHit()
        {
            print("Left obstacle hit!");
        }

        /// <summary>
        /// Character hits an obstacle on the right.
        /// </summary>
        private void RightObstacleHit()
        {
            print("Right obstacle hit!");
        }

        private void CopyCapsuleCollider(CapsuleCollider src, CapsuleCollider dst)
        {
            dst.radius = src.radius;
            dst.height = src.height;
            dst.direction = src.direction;
            dst.center = src.center;
        }
        #endregion

        #endregion
    }
}
