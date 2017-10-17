using System.Linq;
using UnityEngine;
using TurmoilStudios.Utils;

namespace TurmoilStudios.BattleDash {
    /// <summary>
    /// Class that handles the boss battles.
    /// </summary>
    [AddComponentMenu("Battle Dash/Managers/Battle Manager")]
    public class BattleManager : Singleton<BattleManager> {
        Fighter playerCharacter = null;
        Fighter bossCharacter = null;
        AttackArrow attackArrow = null;
        BlockingSlider blockingSlider = null;
        bool isBattling = false;
        float timer = 0.0f;
        bool isTimerIncreasing = true;

        public float interactionSpawnInterval = 3.0f;

        #region Properties
        /// <summary>
        /// The current character that the user is playing as.
        /// </summary>
        public Fighter PlayerCharacter { get { return playerCharacter; } }

        /// <summary>
        /// The current boss that the user is battling.
        /// </summary>
        public Fighter BossCharacter { get { return bossCharacter; } }
        #endregion

        #region Methods

        #region Unity methods
        void Update() {
            if(GameManager.Instance.Status == GameManager.GameStatus.BossBattleInProgress && isBattling) {
                HandleUserControl();
            }
        }

        void OnEnable() {
            //Subscribe to events
            EventManager.StartListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, InitializeBattle);
        }

        void OnDisable() {
            bossCharacter = null;
            playerCharacter = null;

            //Unsubscribe from events
            EventManager.StopListening(Constants.EVENT_BOSSBATTLEBEGINCOMBAT, InitializeBattle);
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Initializes the boss battle.
        /// </summary>
        public void InitializeBattle() {
            GameManager.Instance.SetStatus(GameManager.GameStatus.BossBattleInProgress);
            GUIManager.Instance.DisableInGameMenu();
            GUIManager.Instance.EnableBossBattleMenu();
            isBattling = true;

            //Set the current character
            playerCharacter = LevelManager.Instance.CurrentCharacter.GetComponent<Fighter>();

            //Find all bosses in the game, and get the distance of them from the character
            GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
            float[] distances = new float[bosses.Length];

            if(bosses.Length <= 0) {
                Debug.LogError("No bosses were found in the scene! Do the boss prefabs have a <b>Boss</b> tag assigned to them?");
                return;
            }

            if(playerCharacter != null) {
                for(int i = 0; i < bosses.Length; i++) { //Get the distance of each boss from the character
                    distances[i] = Vector3.Distance(playerCharacter.transform.position, bosses[i].transform.position);
                }
            } else {
                Debug.LogError("The current character doesn't have a Figher script attached!");
            }

            float minDis = distances.Min(); //Get the closest boss

            //Loop through all bosses again, checking to see which is closest
            for(int i = 0; i < bosses.Length; i++) {
                if(distances[i] == minDis) {
                    Fighter boss = bosses[i].GetComponent<Fighter>();

                    if(boss != null)
                        bossCharacter = boss; //Set the current boss
                    else
                        Debug.LogError("The closest boss doesn't have a Figher script attached!");

                    break;
                }
            }

            if(playerCharacter == null || bossCharacter == null) {
                Debug.LogError("Player or Boss Character was null.\n" + "Player Character: " + playerCharacter + "\nBoss Character: " + bossCharacter);
                return;
            }

            //Set the two fighters to be opponents of each other
            playerCharacter.SetOpponent(bossCharacter);
            bossCharacter.SetOpponent(playerCharacter);

            //Restore the health of both characters
            playerCharacter.RestoreHealth();
            bossCharacter.RestoreHealth();
        }

        /// <summary>
        /// Resumes the running of the game.
        /// </summary>
        public void ResumeRun() {
            GameManager.Instance.SetStatus(GameManager.GameStatus.GameInProgress);
            LevelManager.Instance.CurrentCharacter.StartMoving();
            EventManager.TriggerEvent(Constants.EVENT_BOSSBATTLEENDCOMBAT);
        }

        /// <summary>
        /// Initializes the beginning of the end for the boss battle.
        /// </summary>
        public void InitBattleFinish() {
            //Set the boss's health to the lowest health it should be
            bossCharacter.Health = (bossCharacter.MaxHealth * bossCharacter.healthDefeatMultiplyer);

            //End the actual fighting
            isBattling = false;

            //Spawn final attack button
            SpawnFinalAttackButton();
        }

        /// <summary>
        /// Checks to see if the user won the battle.
        /// </summary>
        public void CheckForWin() {
            if(bossCharacter.Health <= (bossCharacter.MaxHealth * bossCharacter.healthDefeatMultiplyer)) {
                print("You have won the boss battle!");

                GUIManager.Instance.EnableVictoryMenu();
                EventManager.TriggerEvent(Constants.EVENT_BOSSBATTLEWON);
                InitBattleFinish();

                /*
                EndBattle();
				// Newbie stuff incoming...
				// Get and Activate the Button that needs to press and n amount of times
				// to finish the boss.
				GameObject button = GameObject.Find ("Main Canvas").transform.FindChild ("FinishBossButton").gameObject;
				button.SetActive (true);
				// The button itself its a GO parented to the Main Canvas and has the UIInfoHolder class attached to it.
				// this class exist for 2 porpuses, to pick the click event and withdraw it from the counter, second
				// once counter reaches 0, send the messages to the GameManager Instance and call the method BossFinished
				// located at the end of this class on the Public Methods Regions


                //GUIManager.Instance.EnableVictoryMenu();
                //EventManager.TriggerEvent(Constants.EVENT_BOSSBATTLEWON);
                */
            }
        }

        /// <summary>
        /// Checks to see if the user lost the battle.
        /// </summary>
        public void CheckForLoss() {
            if(playerCharacter.Health <= 0) {
                print("You have lost the boss battle!");

                EventManager.TriggerEvent(Constants.EVENT_BOSSBATTLELOST);
                print("<b><color=red>EndBattle();</color></b>");
                //EndBattle();
            }
        }

        /// <summary>
		/// UI for finish button
		/// </summary>
		public void BossFinished() {
            // This function is called by the ClickEvent on the FinishBossButton
            GUIManager.Instance.EnableVictoryMenu();
            EventManager.TriggerEvent(Constants.EVENT_BOSSBATTLEWON);
            GameObject button = GameObject.Find("Main Canvas").transform.Find("FinishBossButton").gameObject;
            button.SetActive(false);
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer() {
            isTimerIncreasing = true;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void StopTimer() {
            isTimerIncreasing = false;
        }

        /// <summary>
        /// Resets the timer that spawns the interactions.
        /// </summary>
        public void ResetTimer() {
            timer = 0.0f;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Handles the user's control.
        /// </summary>
        void HandleUserControl() {
            if(isTimerIncreasing)
                timer += Time.deltaTime;

            //Check if the player has won or lost
            CheckForWin();
            CheckForLoss();

            //Spawn an attack arrow or a blocking slider
            if(timer >= interactionSpawnInterval) {
                int choice = Random.Range(0, 4);

                if(choice == 2) //Chances of getting a blocking slider is less
                    SpawnBlockingSlider();
                else
                    SpawnAttackArrow();

                //Reset the timer
                StopTimer();
                ResetTimer();
            }
        }

        /// <summary>
        /// Handles spawning an attack arrow.
        /// </summary>
        void SpawnAttackArrow() {
            Debug.Log("<b>[Battle]</b> Spawn Attack Arrow");

            //Reset the damage multiplyers
            playerCharacter.DamageMultiplyer = 1.0f;
            bossCharacter.DamageMultiplyer = 1.0f;

            //Spawn the arrow
            attackArrow = GUIManager.Instance.SpawnAttackArrow();
        }

        /// <summary>
        /// Handles spawning a blocking slider.
        /// </summary>
        void SpawnBlockingSlider() {
            Debug.Log("<b>[Battle]</b> Spawn Blocking Slider");

            //Reset the damage multiplyers
            playerCharacter.DamageMultiplyer = 1.0f;
            bossCharacter.DamageMultiplyer = 1.0f;

            //Spawn the slider
            blockingSlider = GUIManager.Instance.SpawnBlockingSlider();
        }

        /// <summary>
        /// Spawns the button that will be tapped to finish off the boss.
        /// </summary>
        void SpawnFinalAttackButton() {

        }
        #endregion

        #endregion
    }
}
