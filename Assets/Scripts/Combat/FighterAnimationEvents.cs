using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KyleStankovich.BattleDash;

namespace KyleStankovich.BattleDash
{
    [RequireComponent(typeof(Fighter))]
    public class FighterAnimationEvents : MonoBehaviour
    {
        private Fighter m_Fighter = null;

        #region Methods

        #region Unity methods
        private void Awake()
        {
            m_Fighter = GetComponent<Fighter>();
        }
        #endregion

        #region Animation Event methods
        /// <summary>
        /// Invoked when attack animation makes contact.
        /// </summary>
        public void Hit()
        {
            //Attack opponent
            m_Fighter.AttackOpponent();
            m_Fighter.SpawnAttackParticles();
        }

        /// <summary>
        /// Invoked when the animation unsheathes a weapon.
        /// </summary>
        public void Unsheathe()
        {
            m_Fighter.EnableWeapon();
        }

        /// <summary>
        /// Invoked when the animation sheathes a weapon.
        /// </summary>
        public void Sheathe()
        {
            m_Fighter.DisableWeapon();
        }
        #endregion

        #endregion
    }
}
