using UnityEngine;
using UnityEngine.Events;
using KyleStankovich.Utils;

namespace KyleStankovich.BattleDash {
    public class DieOnHit : MonoBehaviour {
        [SerializeField]
        UnityEvent OnHit = null;

        PlayableCharacter character;
        //Animator anim;
        bool isDead = false;

        /// <summary>
        /// Set to true when player gets bash powerup.
        /// </summary>
        public bool IsInvincible;

        #region Methods

        #region Unity methods
        void Awake() {
            //Get some references
            character = GetComponent<PlayableCharacter>();
            //anim = character.gameObject.GetComponent<Animator>();

            EventManager.StartListening(Constants.EVENT_GAMESTART, () => isDead = false);
        }
        
        private void OnTriggerEnter(Collider other) {
            if(!IsInvincible && !isDead && other.tag == "Obstacle") {
                isDead = true;
                GameManager.Instance.SetStatus(GameManager.GameStatus.ObstacleHit);
                EventManager.TriggerEvent(Constants.EVENT_OBSTACLEHIT);

                OnHit.Invoke();
            }
        }
        #endregion

        #endregion
    }
}
