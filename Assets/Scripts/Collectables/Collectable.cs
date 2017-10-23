using System.Collections;
using UnityEngine;
using KyleStankovich.Utils;

namespace KyleStankovich.BattleDash {
    /// <summary>
    /// Abstract class that is used as the basis of any collectable throughout the game.
    /// </summary>
    public class Collectable : MonoBehaviour {
        [SerializeField]
        protected AudioClip pickupSound = null;
        [SerializeField]
        protected GameObject pickupFX = null;
        [SerializeField]
        [Tooltip("Amount of points that will be given to the player when this collectable is picked up.")]
        protected float scoreOnPickup = 100.0f;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        protected float destroyTime = 0.5f;

        #region Methods

        #region Unity methods
        protected virtual void OnTriggerEnter(Collider col) {
            if(col.transform.tag == "Player")
                OnPickup();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Invoked when object is picked up. Should be overridden for custom functionality.
        /// Make sure to include base.OnPickup() or else the sound and effects won't show/play.
        /// </summary>
        public virtual void OnPickup() {
            //Play sound
            if(pickupSound != null)
                AudioManager.Instance.PlayEffect(pickupSound);

            //Spawn pickup FX if possible
            if(pickupFX != null)
                Instantiate(pickupFX, transform.GetChild(0).transform.position, Quaternion.identity);

            //Increment score
            GameManager.Instance.AddPoints(scoreOnPickup);

            //Disable object after so long
            StartCoroutine(C_DisableObject());
        }
        #endregion

        #region IEnumerator methods
        /// <summary>
        /// Disables the game object after X amount of seconds.
        /// </summary>
        IEnumerator C_DisableObject() {
            yield return new WaitForSeconds(destroyTime);

            gameObject.SetActive(false);
        }
        #endregion

        #endregion
    }
}
