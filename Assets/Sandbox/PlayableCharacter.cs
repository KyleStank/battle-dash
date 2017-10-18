using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleStankovich.BattleDash
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayableCharacter : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Animator that the character will use for Animation.")]
        protected Animator animator = null;

        protected Rigidbody rb = null;

        #region Methods

        #region Unity methods
        protected void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        #endregion

        #region Public methods

        #endregion

        #region Private methods

        #endregion

        #endregion
    }
}
