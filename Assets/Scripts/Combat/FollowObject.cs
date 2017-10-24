using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleStankovich.BattleDash
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Target = null;

        #region Methods

        #region Unity methods
        private void Update()
        {
            transform.position = m_Target.position;
            //transform.localRotation = m_Target.rotation;
        }
        #endregion

        #endregion
    }
}
