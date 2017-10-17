using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/*note: (Wayde)
 * Test class that magnetizes the player to the ground but still supports jumping.
 * 
 * Will integrate with battle dash after I get this working.
 */
namespace TurmoilStudios.BattleDash {
    public class StrafingCharacter1 : MonoBehaviour {

        public bool OnGround;
        public Transform GroundRayOrigin;
        public float GroundRayLength;
        public float GroundRayResolutionLength;
        public LayerMask GroundLayer;

        public Vector3 Velocity;
        public float GravityAcceleration;

        public bool IsJumping;
        public float JumpSpeed;
        public float MoveSpeed = .01f;

        private void OnEnable() {

        }
        private void FixedUpdate() {

            Velocity += -transform.up * GravityAcceleration;
            transform.position += Velocity;


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
                    OnGround = true;
                }
            }
            else
                OnGround = false;
        }
        private void Update() {
            if (OnGround && Input.GetKeyDown(KeyCode.LeftShift)) {
                Velocity.y = JumpSpeed;
                IsJumping = true;
                OnGround = false;
            }

            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))*MoveSpeed;
            
            Velocity.x = movement.x;
            Velocity.z = movement.z;
            
        }
        private void OnDrawGizmosSelected() {
            if (GroundRayOrigin) {
                Debug.DrawRay(GroundRayOrigin.position, -Vector3.up * GroundRayResolutionLength, Color.green);
                Debug.DrawRay(GroundRayOrigin.position - Vector3.up * GroundRayResolutionLength, -Vector3.up * (GroundRayLength - GroundRayResolutionLength), Color.red);
            }
        }
    }
}