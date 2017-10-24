using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KyleStankovich.BattleDash;

namespace KyleStankovich.BattleDash
{
    [RequireComponent(typeof(StrafingCharacter))]
    public class StrafingCharacterAnimationEvents : MonoBehaviour
    {
        [SerializeField]
        private float m_ScorePerStep = 2.0f;

        #region Methods

        #region Animation Event methods
        /// <summary>
        /// Invoked when animation steps on right foot.
        /// </summary>
        public void FootR()
        {
            //Increase score
            GameManager.Instance.AddPoints(m_ScorePerStep);
        }

        /// <summary>
        /// Invoked when animation steps on right foot.
        /// </summary>
        public void FootL()
        {
            //Increase score
            GameManager.Instance.AddPoints(m_ScorePerStep);
        }
        #endregion

        #endregion
    }
}
