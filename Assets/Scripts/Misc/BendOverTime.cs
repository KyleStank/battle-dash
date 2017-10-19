using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurmoilStudios.BattleDash;
using TurmoilStudios.Utils;
using VacuumShaders.CurvedWorld;

namespace KyleStankovich.BattleDash
{
    [RequireComponent(typeof(CurvedWorld_Controller))]
    public class BendOverTime : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_BendFadeAmount = Vector3.zero;
        [SerializeField]
        private bool m_FadeXBend = false;
        [SerializeField]
        private bool m_FadeYBend = true;
        [SerializeField]
        private bool m_FadeZBend = false;
        [SerializeField]
        private float m_BendFadeSpeed = 2.0f;

        private CurvedWorld_Controller m_CurvedWorldController = null;
        private Vector3 m_StartBend = Vector3.zero;
        private Vector3 m_NewBend = Vector3.zero;

        #region Methods

        #region Unity methods
        private void Awake()
        {
            m_CurvedWorldController = GetComponent<CurvedWorld_Controller>();
            m_StartBend = new Vector3(m_CurvedWorldController._V_CW_Bend_X, m_CurvedWorldController._V_CW_Bend_Y, m_CurvedWorldController._V_CW_Bend_Z);
        }

        private void Update()
        {
            if(GameManager.Instance.Status != GameManager.GameStatus.GameInProgress)
            {
                return;
            }

            //Set new bend position as the original position
            m_NewBend = m_StartBend;
            
            //Fade X bend
            if(m_FadeXBend)
            {
                m_NewBend.x = m_BendFadeAmount.x * Mathf.Sin(Time.time * m_BendFadeSpeed);
            }

            //Fade Y bend
            if(m_FadeYBend)
            {
                m_NewBend.y = m_BendFadeAmount.y * Mathf.Sin(Time.time * m_BendFadeSpeed);
            }

            //Fade Z bend
            if(m_FadeZBend)
            {
                m_NewBend.z = m_BendFadeAmount.z * Mathf.Sin(Time.time * m_BendFadeSpeed);
            }

            //Set new world bend
            m_CurvedWorldController.SetBend(m_NewBend);
        }
        #endregion

        #endregion
    }
}
