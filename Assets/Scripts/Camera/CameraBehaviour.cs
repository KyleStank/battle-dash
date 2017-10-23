using UnityEngine;
using KyleStankovich.Utils;

namespace KyleStankovich.BattleDash {
    /// <summary>
    /// Class that handles the camera movement and rotation.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Battle Dash/Camera/Camera Behaviour")]
    public class CameraBehaviour : MonoBehaviour {
        [Header("Follow Target Settings")]
        [SerializeField]
        Transform target = null;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        float xSpeed = 0.1f;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        float zSpeed = 0.1f;

        [Header("Camera Settings")]
        [SerializeField]
        bool isFollowing = false;
        [SerializeField]
        Vector3 posOffset = Vector3.zero;
        [SerializeField]
        Vector3 rotation = Vector3.zero;
        
        float cameraVelocity = 0.0f;

        #region Methods

        #region Unity methods
        void Awake() {
            isFollowing = false;
        }

        void LateUpdate() {
            if(isFollowing) //Only follow if allowed
                HandleCameraMovementRotation();
        }

        void OnEnable() {
            //Subscribe to events
            EventManager.StartListening(Constants.EVENT_GAMESTART, StartFollowing);
        }

        void OnDisable() {
            //Unsubscribe from events
            EventManager.StopListening(Constants.EVENT_GAMESTART, StartFollowing);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Makes the camera start following the target.
        /// </summary>
        public void StartFollowing() {
            isFollowing = true;
        }

        /// <summary>
        /// Makes the camera stop following the target.
        /// </summary>
        public void StopFollowing() {
            isFollowing = false;
        }

        public void TranslateCamera(Transform target) {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Handles the camera's movement and rotation.
        /// </summary>
        void HandleCameraMovementRotation() {
            if(GameManager.Instance.IsPaused) //This needs to be here!
                return;

            //Get the camera's current position
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;

            //Change camera's position
            pos.x = Mathf.SmoothStep(pos.x, target.position.x, xSpeed);
            pos.x = Mathf.SmoothStep(pos.x, Mathf.Clamp(pos.x, -posOffset.x, posOffset.x), xSpeed);
            pos.y = Mathf.SmoothStep(pos.y, posOffset.y, 0.1f);
            pos.z = Mathf.SmoothDamp(pos.z, target.position.z - posOffset.z, ref cameraVelocity, zSpeed);

            //Change camera's rotation
            rot.eulerAngles = Vector3.Lerp(rot.eulerAngles, rotation, 0.1f);

            //Set position
            transform.position = pos;
            transform.rotation = rot;
        }
        #endregion

        #endregion
    }
}
