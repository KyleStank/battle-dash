using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TurmoilStudios.Utils;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Class that handles blocking an attack from a boss.
    /// </summary>
	public class BlockingSlider : Slider {
        [SerializeField]
        [Range(0.0f, 5.0f)]
        float slideSpeed = 1.0f;
        [SerializeField]
        [Tooltip("Amount of speed that slider can be randomly changed by, both by increasing the speed or decreasing the speed.")]
        float speedRandomizerOffset = 0.5f;
        [SerializeField]
        [Tooltip("Amount of time the player will have to block before the game does by itself.")]
        float reactionTime = 2.5f;
        [SerializeField]
        [Tooltip("Amount of time that is given to the player between each block.")]
        float timeBetweenBlocks = 1.0f;

        public Image perfectArea = null;
        public Image goodArea = null;
        public Image okayArea = null;
        public int amountOfBlocks = 2;

        RectTransform rTransform = null;
        float perfectPercentage = 0.1f;
        float goodPercentage = 0.3f;
        float okayPercentage = 1.0f;
        float moveAmount = 1.0f;
        int currentBlock = 0;
        bool canMove = true;

        #region Methods

        #region Unity methods
        new void Awake() {
            base.Awake();

            rTransform = GetComponent<RectTransform>();

            //Get percentages needed from the size of the boxes
            perfectPercentage = perfectArea.rectTransform.rect.width / rTransform.rect.width;
            goodPercentage = goodArea.rectTransform.rect.width / rTransform.rect.width;
            okayPercentage = okayArea.rectTransform.rect.width / rTransform.rect.width;

            //Randomly choose a speed
            RandomizeSpeed();

            //Set the variable that will actually modify the value of the slider
            int choice = Random.Range(0, 2);
            if(choice == 0)
                moveAmount = Mathf.Abs(slideSpeed);
            else
                moveAmount = -Mathf.Abs(slideSpeed);

            //Start reaction tiemr
            StartCoroutine(StartReactionTimer());
        }

        protected override void OnEnable() {
            base.OnEnable();

            //Subscribe to events
            EventManager.StartListening(Constants.EVENT_BOSSBATTLELOST, () => Destroy(gameObject, 0.1f));
        }

        protected override void OnDisable() {
            base.OnDisable();

            //Start the timer again
            BattleManager.Instance.ResetTimer();
            BattleManager.Instance.StartTimer();

            //Unsubscribe from events
            EventManager.StopListening(Constants.EVENT_BOSSBATTLELOST, () => Destroy(gameObject, 0.1f));
        }

        void Update() {
#if UNITY_EDITOR
            if(!EditorApplication.isPlaying)
                return;
#endif

            if(!canMove)
                return;

            //Move the slider handle
            value += Time.deltaTime * moveAmount;

            //Check if the value is to the left or right all the way
            if(value >= maxValue || value <= minValue)
                moveAmount = -moveAmount;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Attempts to block a boss's attack.
        /// </summary>
        public void Block() {
            //Stop reaction timer and reset it.
            StopAllCoroutines();
            StartCoroutine(StartReactionTimer());
            StartCoroutine(DelayMovement());
            currentBlock++;

            //Randomize the speed
            RandomizeSpeed();

            float absVal = Mathf.Abs(value);

            //Perfect block
            if(absVal <= Mathf.Abs(perfectPercentage)) {
                BattleManager.Instance.BossCharacter.DamageMultiplyer = 0.0f;
                print("<color=green><b>Perfect!</b></color>");
            } else if(absVal <= Mathf.Abs(goodPercentage)) { //Good block
                BattleManager.Instance.BossCharacter.DamageMultiplyer = 0.5f;
                print("<color=blue><b>Good!</b></color>");
            } else { //Okay block
                BattleManager.Instance.BossCharacter.DamageMultiplyer = 1.0f;
                print("<color=yellow><b>Okay!</b></color>");
            }

            //Attack the player
            BattleManager.Instance.PlayerCharacter.Defend();
            BattleManager.Instance.BossCharacter.StartAttack();
            BattleManager.Instance.CheckForLoss();

            //Check if blocking slider should be disabled
            if(currentBlock >= amountOfBlocks) {
                //Destroy slider
                Destroy(gameObject, 0.1f);

                //Reset and start the timer again
                BattleManager.Instance.ResetTimer();
                BattleManager.Instance.StartTimer();
            }

            return;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Randomizes the speed at which the ball will travel.
        /// </summary>
        void RandomizeSpeed() {
            //Check if the move amount is a positive or negative number
            byte speedNumType = 0; //0 means positive, 1 means negative
            if(moveAmount > 0)
                speedNumType = 0;
            else
                speedNumType = 1;

            //Randomize the speed
            float speedChange = Random.Range(-speedRandomizerOffset, speedRandomizerOffset);
            slideSpeed += speedChange;

            //Set the speed
            if(speedNumType == 0)
                moveAmount = Mathf.Abs(slideSpeed);
            else
                moveAmount = -Mathf.Abs(slideSpeed);
        }
        #endregion

        #region IEnumerator methods
        /// <summary>
        /// Starts a corutine that will take care of how long the player has to block before blocking for him/her.
        /// </summary>
        /// <returns></returns>
        IEnumerator StartReactionTimer() {
            yield return new WaitForSeconds(reactionTime);

            Block();
        }

        /// <summary>
        /// Delays the movement of the slider to stop for a little bit.
        /// </summary>
        /// <returns></returns>
        IEnumerator DelayMovement() {
            canMove = false;

            yield return new WaitForSeconds(timeBetweenBlocks);

            canMove = true;
        }
        #endregion

        #endregion
    }
}
