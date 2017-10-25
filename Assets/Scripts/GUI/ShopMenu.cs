using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KyleStankovich.BattleDash
{
    public class ShopMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private SelectableCharacter[] m_PossibleCharacters = null;
        [SerializeField]
        private SelectableCharacter[] m_PlayerCharacters = null;

        [Header("UI References")]
        [SerializeField]
        private Text m_CharacterNameText = null;
        [SerializeField]
        private Text m_CharacterPriceText = null;
        [SerializeField]
        private Button m_PurchaseButton = null;

        private int m_CurrentCharacterIndex = 0;

        #region Methods

        #region Unity methods
        private void OnEnable()
        {
            SelectCharacter(0);
        }
        #endregion

        #region Public methods
        public void SelectPreviousCharacter()
        {
            if(m_CurrentCharacterIndex <= 0)
            {
                return;
            }

            SelectCharacter(--m_CurrentCharacterIndex);
        }

        public void SelectNextCharacter()
        {
            if(m_CurrentCharacterIndex >= m_PossibleCharacters.Length - 1)
            {
                return;
            }

            SelectCharacter(++m_CurrentCharacterIndex);
        }

        public void PurchaseCharacter()
        {
            SelectableCharacter character = m_PossibleCharacters[m_CurrentCharacterIndex];
            if(GameManager.Instance.TotalCoins >= character.Price)
            {
                GameManager.Instance.RemoveTotalCoins(character.Price);
                character.Unlock();
                SelectCharacter(m_CurrentCharacterIndex);
                ActivateCharacter();
            }
        }

        public void ActivateCharacter()
        {
            //Disable all character on player
            for(int i = 0; i < m_PlayerCharacters.Length; i++)
            {
                m_PlayerCharacters[i].gameObject.SetActive(false);
            }

            //Find a new character to enable
            for(int i = 0; i < m_PlayerCharacters.Length; i++)
            {
                if(m_PlayerCharacters[i].CharacterName == m_PossibleCharacters[m_CurrentCharacterIndex].CharacterName)
                {
                    m_PlayerCharacters[i].gameObject.SetActive(true);
                }
            }
        }
        #endregion

        #region Private methods
        private void SelectCharacter(int index)
        {
            if(m_PossibleCharacters.Length <= 0)
            {
                return;
            }

            //Disable all characters
            for(int i = 0; i < m_PossibleCharacters.Length; i++)
            {
                m_PossibleCharacters[i].gameObject.SetActive(false);
            }
            
            //Set new character
            m_CurrentCharacterIndex = index;

            //Update UI
            SelectableCharacter character = m_PossibleCharacters[m_CurrentCharacterIndex];
            m_CharacterNameText.text = character.CharacterName;
            m_CharacterPriceText.text = "$: " + character.Price.ToString();
            if(character.IsUnlocked)
            {
                m_PurchaseButton.GetComponentInChildren<Text>().text = "SELECT";
                m_CharacterPriceText.text = "$: 0";
                m_PurchaseButton.onClick.AddListener(ActivateCharacter);
            }
            else
            {
                m_PurchaseButton.GetComponentInChildren<Text>().text = "BUY";
                m_PurchaseButton.onClick.AddListener(PurchaseCharacter);
            }

            //Enable new character
            m_PossibleCharacters[m_CurrentCharacterIndex].gameObject.SetActive(true);
        }
        #endregion

        #endregion
    }
}
