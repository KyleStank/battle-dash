using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KyleStankovich.Utils;

namespace KyleStankovich.BattleDash
{
    public class PlayableCharacterInput : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Distance that user has to swipe his/her finger for the character to move.")]
        private float m_SwipeDistance = 100.0f;
        [SerializeField]
        private bool m_InputActive = false;

        private bool m_TouchedLastFrame = false;
        private Vector3 m_TouchStartPos = Vector3.zero;

        #region Methods

        #region Unity methods
        private void OnEnable()
        {
            //Subscribe to events
            EventManager.StartListening(Constants.EVENT_GAMESTART, EnableInput);
        }

        private void OnDisable()
        {
            //Unsubscribe from events
            EventManager.StopListening(Constants.EVENT_GAMESTART, EnableInput);
        }

        private void Update()
        {
            if(GameManager.Instance.Status == GameManager.GameStatus.GameInProgress)
                HandleRunningInput();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Enables input.
        /// </summary>
        public void EnableInput()
        {
            m_InputActive = true;
            m_TouchedLastFrame = false;
        }

        /// <summary>
        /// Disables input.
        /// </summary>
        public void DisableInput()
        {
            m_InputActive = false;
            m_TouchedLastFrame = false;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Detects when there is input. If there is, take care of it.
        /// </summary>
        private void HandleRunningInput()
        {
            if(!m_InputActive)
            {
                m_TouchedLastFrame = false;
                return;
            }

#if UNITY_EDITOR
            HandleKeyboard();
#endif

            if(Input.touchCount <= 0)
                return;

            Touch touch = Input.touches[0]; //Get the first/main touch

            //Switch through every phase of the touch
            switch(touch.phase)
            {
                case TouchPhase.Began:
                    if(m_TouchedLastFrame)
                    {
                        break;
                    }

                    m_TouchedLastFrame = true;
                    m_TouchStartPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    if(!m_TouchedLastFrame)
                    {
                        break;
                    }

                    m_TouchedLastFrame = false;
                    HandleSwiping(touch.position);
                    break;
            }
        }

        /// <summary>
        /// Handles the logic behind swiping. This includes detecting if it was a horizontal or vertical swipe.
        /// </summary>
        /// <param name="touchPos">Position of the touch.</param>
        private void HandleSwiping(Vector2 touchPos)
        {
            //Calculate the distance from when the swipe began to when the swipe has ended (which is right now).
            Vector2 swipeDistance = new Vector2(Mathf.Abs(touchPos.x - m_TouchStartPos.x), Mathf.Abs(touchPos.y - m_TouchStartPos.y));

            //Check if the user swiped the screen enough horizontally
            if(swipeDistance.x > m_SwipeDistance)
            {
                //Returns 1 if positive(swiped right), returns -1 if negative(swiped left).
                float swipeValue = Mathf.Sign(touchPos.x - m_TouchStartPos.x);

                //Right Swipe
                if(swipeValue > 0.0f)
                {
                    EventManager.TriggerEvent(Constants.EVENT_INPUTRIGHT);
                }
                else //Left Swipe
                {
                    EventManager.TriggerEvent(Constants.EVENT_INPUTLEFT);
                }

                return;
            }

            //Check if the user swiped the screen enough vertically
            if(swipeDistance.y > m_SwipeDistance)
            {
                //Returns 1 if positive(swiped up), returns -1 if negative(swiped left).
                float swipeValue = Mathf.Sign(touchPos.y - m_TouchStartPos.y);

                //Up Swipe
                if(swipeValue > 0.0f)
                {
                    EventManager.TriggerEvent(Constants.EVENT_INPUTUP);
                }
                else //Down Swipe
                {
                    EventManager.TriggerEvent(Constants.EVENT_INPUTDOWN);
                }

                return;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Handles input from the keyboard. Useful for testing purposes.
        /// </summary>
        private void HandleKeyboard()
        {
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                EventManager.TriggerEvent(Constants.EVENT_INPUTRIGHT);
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                EventManager.TriggerEvent(Constants.EVENT_INPUTLEFT);
            }

            if(Input.GetKeyDown(KeyCode.A))
            {
                EventManager.TriggerEvent(Constants.EVENT_INPUTLEFT);
            }

            if(Input.GetKeyDown(KeyCode.D))
            {
                EventManager.TriggerEvent(Constants.EVENT_INPUTRIGHT);
            }

            if(Input.GetKeyDown(KeyCode.W))
            {
                EventManager.TriggerEvent(Constants.EVENT_INPUTUP);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                EventManager.TriggerEvent(Constants.EVENT_INPUTUP);
            }

            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                EventManager.TriggerEvent(Constants.EVENT_INPUTUP);
            }

            if(Input.GetKeyDown(KeyCode.S))
            {
                EventManager.TriggerEvent(Constants.EVENT_INPUTDOWN);
            }

            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                EventManager.TriggerEvent(Constants.EVENT_INPUTDOWN);
            }
        }
#endif
        #endregion

        #endregion
    }
}
