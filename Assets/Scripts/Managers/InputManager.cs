/*
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using KyleStankovich.Utils;

namespace KyleStankovich.BattleDash {
    /// <summary>
    /// Handles all of the input retrieved from the user.
    /// </summary>
    [AddComponentMenu("Battle Dash/Managers/Input Manager")]
    public class InputManager : Singleton<InputManager> {
        [SerializeField]
        [Tooltip("Distance that user has to swipe his/her finger for the character to move.")]
        float switchLaneTouchDistance = 200.0f;
        [SerializeField]
        bool inputActive = false;

        bool touchedLastFrame = false;
        Vector3 touchStartPos = new Vector3(0.0f, 0.0f, 0.0f);
        float swipeDistHorizontal = 0.0f;
        float swipeDistVertical = 0.0f;

        #region Methods

        #region Unity methods
        protected virtual void Update()
        {
            if(GameManager.Instance.Status == GameManager.GameStatus.GameInProgress)
                HandleRunningInput();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Enables the input for the user.
        /// </summary>
        public void EnableInput() {
            inputActive = true;
            touchedLastFrame = false;
        }

        /// <summary>
        /// Disables the input for the user.
        /// </summary>
        public void DisableInput() {
            inputActive = false;
            touchedLastFrame = false;
        }
        #endregion

        #region Private methods

        #region Running input
        /// <summary>
        /// Handles the input when the player is running.
        /// </summary>
        void HandleRunningInput() {
            if(!inputActive) {
                touchedLastFrame = false;
                return;
            }

#if UNITY_EDITOR
            HandleKeyboard();
#endif

            if(Input.touchCount <= 0)
                return;

            Touch touch = Input.touches[0]; //Get the first/main touch

            //Switch through every phase of the touch
            switch(touch.phase) {
                case TouchPhase.Began:
                    if(touchedLastFrame)
                    {
                        break;
                    }

                    touchedLastFrame = true;
                    touchStartPos = touch.position;
                    break;
                    
                case TouchPhase.Ended:
                    if(!touchedLastFrame)
                    {
                        break;
                    }

                    touchedLastFrame = false;
                    HandleSwiping(touch.position);
                    break;
            }
        }

        private void HandleSwiping(Vector2 touchPos)
        {
            Vector2 swipeDistance = new Vector2(Mathf.Abs(touchPos.x - touchStartPos.x), Mathf.Abs(touchPos.y - touchStartPos.y));

            //Check if the user swiped the screen enough horizontally
            if(swipeDistance.x > switchLaneTouchDistance)
            {
                //Returns 1 if positive(swiped right), returns -1 if negative(swiped left).
                float swipeValue = Mathf.Sign(touchPos.x - touchStartPos.x);

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
            if(swipeDistance.y > switchLaneTouchDistance)
            {
                //Returns 1 if positive(swiped up), returns -1 if negative(swiped left).
                float swipeValue = Mathf.Sign(touchPos.y - touchStartPos.y);

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
        #endregion

        #region IEnumerator methods
        /// <summary>
        /// Enables the input after X amount of seconds.
        /// </summary>
        /// <param name="delay">Seconds to wait.</param>
        IEnumerator C_EnableInput(float delay) {
            yield return new WaitForSeconds(delay);

            inputActive = true;
            touchedLastFrame = false;
        }
        #endregion

#if UNITY_EDITOR
        void HandleKeyboard() {

            if(Input.GetKeyDown(KeyCode.RightArrow))
                EventManager.TriggerEvent(Constants.EVENT_INPUTRIGHT);

            if(Input.GetKeyDown(KeyCode.LeftArrow))
                EventManager.TriggerEvent(Constants.EVENT_INPUTLEFT);

            if(Input.GetKeyDown(KeyCode.A))
                EventManager.TriggerEvent(Constants.EVENT_INPUTLEFT);

            if(Input.GetKeyDown(KeyCode.D))
                EventManager.TriggerEvent(Constants.EVENT_INPUTRIGHT);

            if(Input.GetKeyDown(KeyCode.W))
                EventManager.TriggerEvent(Constants.EVENT_INPUTUP);

            if(Input.GetKeyDown(KeyCode.Space))
                EventManager.TriggerEvent(Constants.EVENT_INPUTUP);

            if(Input.GetKeyDown(KeyCode.UpArrow))
                EventManager.TriggerEvent(Constants.EVENT_INPUTUP);

            if(Input.GetKeyDown(KeyCode.S))
                EventManager.TriggerEvent(Constants.EVENT_INPUTDOWN);

            if(Input.GetKeyDown(KeyCode.DownArrow))
                EventManager.TriggerEvent(Constants.EVENT_INPUTDOWN);
        }
#endif
        #endregion

        #endregion
    }
}
*/