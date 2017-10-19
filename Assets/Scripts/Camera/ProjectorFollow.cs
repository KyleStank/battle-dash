using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleStankovich.BattleDash
{
    public class ProjectorFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform m_FollowTarget = null;

        private Vector3 m_NewPos = Vector3.zero;

        #region Methods

        #region Unity methods
        private void Awake()
        {
            m_NewPos = transform.position;
        }

        private void Update()
        {
            FollowTarget();
        }
        #endregion

        #region Private methods
        private void FollowTarget()
        {
            if(m_FollowTarget == null)
            {
                Debug.LogWarning("Cannot follow target because no target was set in the Inspector!");
            }

            //Follow target by matching its X and Z coordinates
            m_NewPos.x = m_FollowTarget.position.x;
            m_NewPos.z = m_FollowTarget.position.z;
            transform.position = m_NewPos;
        }
        #endregion

        #endregion
    }
}
