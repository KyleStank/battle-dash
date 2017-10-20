using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleStankovich.BattleDash
{
    public class SpinObject : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_SpinSpeed = Vector3.zero;
        
        #region Methods

        #region Unity methods
        private void Update()
        {
            transform.Rotate(m_SpinSpeed);
        }
        #endregion

        #endregion
    }
}
