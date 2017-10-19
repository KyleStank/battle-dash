using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Class that handles attacking the boss.
    /// </summary>
    public sealed class AttackArrow : Slider, IBeginDragHandler, IDragHandler, IEndDragHandler {
        [SerializeField]
        float screenTime = 1.5f;

        float startTime = 0.0f;
        float timer = 0.0f;
        Vector2 beginPos = Vector2.zero;
        Vector2 endPos = Vector2.zero;
        bool hasAttacked = false;

        public Color perfectColor = Color.green;
        public Color goodColor = Color.blue;
        public Color okayColor = Color.yellow;
        public Color missColor = Color.red;

        #region Methods

        #region Unity methods
        void Update() {
            timer += Time.deltaTime;

            //If attack arrow should be disabled
            if(timer > screenTime)
                Destroy(gameObject);
        }

        protected override void OnEnable() {
            base.OnEnable();

            //Get the starting time
            startTime = Time.time;
        }

        protected override void OnDisable() {
            base.OnDisable();

            //If the player has swiped the arrow
            if(!hasAttacked) {
                //Display swipe text
                GUIManager.Instance.DisplaySwipeText("Miss!", missColor);

                //Damage the player
                BattleManager.Instance.BossCharacter.StartAttack();
            }

            //Start the timer again
            BattleManager.Instance.ResetTimer();
            BattleManager.Instance.StartTimer();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// When arrow has begun to be swiped.
        /// </summary>
        /// <param name="eventData">Swipe data.</param>
        public void OnBeginDrag(PointerEventData eventData) {
            if(!interactable)
                return;
            
            beginPos = eventData.position;
        }

        /// <summary>
        /// When arrow is being swiped.
        /// </summary>
        /// <param name="eventData">Swipe data.</param>
        public new void OnDrag(PointerEventData eventData) {
            base.OnDrag(eventData);
            
            if(!interactable)
                return;

            //If arrow has been swiped enough, attack the boss
            if(value >= (maxValue * 0.75f)) {
                //Fill the rest of the slider. This is simply for visual effects
                value = maxValue;

                //Change the damage based on the time
                float curTime = Mathf.Abs(Time.time - startTime);
                if(curTime <= screenTime * 0.4f) { //Perfect swipe
                    BattleManager.Instance.PlayerCharacter.DamageMultiplyer = 1.0f;
                    GUIManager.Instance.DisplaySwipeText("Perfect!", perfectColor);
                } else if(curTime <= screenTime * 0.6f) { //Good swipe
                    BattleManager.Instance.PlayerCharacter.DamageMultiplyer = 0.75f;
                    GUIManager.Instance.DisplaySwipeText("Good!", goodColor);
                } else { //Okay swipe
                    BattleManager.Instance.PlayerCharacter.DamageMultiplyer = 0.5f;
                    GUIManager.Instance.DisplaySwipeText("Okay!", okayColor);
                }

                //Notify this script that we have attacked
                hasAttacked = true;

                //Attack the boss
                Debug.Log("Start attack!");
                BattleManager.Instance.PlayerCharacter.StartAttack();

                //Disable the arrow
                interactable = false;

                //Destroy arrow
                Destroy(gameObject, 0.1f);

                return;
            }
        }

        /// <summary>
        /// When arrow is not being swiped any more.
        /// </summary>
        /// <param name="eventData">Swipe data.</param>
        public void OnEndDrag(PointerEventData eventData) {
            if(!interactable)
                return;

            //Player has missed the swipe
            if(value <= (maxValue * 0.75f)) {
                //Make the player stumble/miss a swipe
                BattleManager.Instance.BossCharacter.DamageMultiplyer = 0.5f;

                //Destroy arrow
                Destroy(gameObject);
            }

            //Disable the arrow
            interactable = false;
        }
        #endregion

        #endregion
    }
}
