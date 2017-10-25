using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleStankovich.BattleDash
{
    public class SelectableCharacter : MonoBehaviour
    {
        [SerializeField]
        private string m_CharacterName = "";
        [SerializeField]
        private int m_Price = 1000;

        private bool m_IsUnlocked = false;

        #region Properties
        public string CharacterName
        {
            get { return m_CharacterName; }
        }

        public int Price
        {
            get { return m_Price; }
        }

        public bool IsUnlocked
        {
            get {
                return PlayerPrefs.GetInt(m_CharacterName, -1) == 1;
            }
        }
        #endregion

        #region Methods

        #region Public methods
        public void Unlock()
        {
            m_IsUnlocked = true;
            m_Price = 0;
            PlayerPrefs.SetInt(m_CharacterName, 1);
        }
        #endregion

        #endregion
    }
}
