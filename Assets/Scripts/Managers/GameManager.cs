using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using KyleStankovich.Utils;

namespace KyleStankovich.BattleDash {
    /// <summary>
    /// Manages a lot of information about the game.
    /// </summary>
    [AddComponentMenu("Battle Dash/Managers/Game Manager")]
    public class GameManager : Singleton<GameManager> {
        //Holds the game's current status
        public enum GameStatus {
            StartMenu,
            GameInProgress,
            GamePaused,
            ObstacleHit,
            BossBattleInProgress
        }
        [SerializeField]
        GameStatus status = GameStatus.StartMenu;

        //Data
        [SerializeField]
        int targetFPS = 60;
        [SerializeField]
        float pointsPerSecond = 10.0f;

        float points = 0.0f;
        int highScore = 0;
        int coins = 0;
        float timeScale = 1.0f;
        float savedTimeScale = 0.0f;
        bool canAddPoints = false;
        GameStatus statusBeforePause;

        #region Properties
        /// <summary>
        /// The current status of the game.
        /// </summary>
        public GameStatus Status { get { return status; } }

        /// <summary>
        /// Target FPS of the game.
        /// </summary>
        public int TargetFPS { get { return targetFPS; } }

        /// <summary>
        /// Tells us whether or not the game is paused based on the time scale.
        /// </summary>
        public bool IsPaused { get { return Time.timeScale <= 0.0f; } }

        /// <summary>
        /// Current amount of points that have been earned.
        /// </summary>
        public float Points { get { return points; } }

        /// <summary>
        /// The highest amount of points that have ever been scored.
        /// </summary>
        public int HighScore { get { return highScore; } }

        /// <summary>
        /// Amount of points that are added every second.
        /// </summary>
        public float PointsPerSecond { get { return pointsPerSecond; } }

        /// <summary>
        /// Returns the amount of coins that have been collected but not saved.
        /// </summary>
        public int InGameCoins {
            get {
                int savedCoins = PlayerPrefs.GetInt(Constants.PREF_CURRENCY);

                return Mathf.Abs(coins - savedCoins);
            }
        }
        #endregion

#if UNITY_EDITOR
        #region Debugging
        [SerializeField]
        [Range(0.0f, 1.0f)]
        float debugTimeScale = 1.0f;
        #endregion
#endif

        #region Methods

        #region Unity methods
        new void Awake() {
            base.Awake();

            LoadData();
        }

        void Start() {
            Application.targetFrameRate = targetFPS;
            savedTimeScale = timeScale;
            Time.timeScale = timeScale;
            statusBeforePause = status;
        }

        void OnEnable() {
            //Subscribe to events
            EventManager.StartListening(Constants.EVENT_OBSTACLEHIT, SaveData);
            EventManager.StartListening(Constants.EVENT_PLAYEROUTOFBOUNDS, () => EventManager.TriggerEvent(Constants.EVENT_OBSTACLEHIT));
        }

        void OnDisable() {
            //Unsubscribe to events
            EventManager.StopListening(Constants.EVENT_OBSTACLEHIT, SaveData);
            EventManager.StopListening(Constants.EVENT_PLAYEROUTOFBOUNDS, () => EventManager.TriggerEvent(Constants.EVENT_OBSTACLEHIT));
        }
        #endregion

        #region Public methods
#if UNITY_EDITOR
        void Update() {
            if(Status != GameStatus.GamePaused)
                Time.timeScale = debugTimeScale;
        }
#endif

        /// <summary>
        /// Loads important data for the game.
        /// </summary>
        public void LoadData() {
            coins = PlayerPrefs.GetInt(Constants.PREF_CURRENCY, 0);
            highScore = PlayerPrefs.GetInt(Constants.PREF_HIGHSCORE, 0);
        }

        /// <summary>
        /// Saves important data about the game.
        /// </summary>
        public void SaveData() {
            PlayerPrefs.SetInt(Constants.PREF_CURRENCY, coins);
            PlayerPrefs.SetInt(Constants.PREF_HIGHSCORE, highScore);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Sets the status of the game.
        /// </summary>
        /// <param name="gameStatus">New status of the game.</param>
        public void SetStatus(GameStatus gameStatus) {
            status = gameStatus;
        }

        /// <summary>
        /// Exits the the game and returns to the main menu.
        /// </summary>
        public void ExitRun() {
            SaveData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
		/// Resets the game to start playing again.
		/// </summary>
		public void Reset() {
            LoadData(); //Reload so we don't keep the coins we have picked up
            points = 0;
            Time.timeScale = timeScale;
            SetStatus(GameStatus.GameInProgress);
            LevelManager.Instance.ResetLevel();
            LevelManager.Instance.CurrentCharacter.ResetXYPosition();
            LevelManager.Instance.CurrentCharacter.ResetRotation();
            SetPointsPerSecond(pointsPerSecond);
            StopAllCoroutines();
            AutoIncrementScore(true);
            EventManager.TriggerEvent(Constants.EVENT_GAMESTART);
            GUIManager.Instance.RefreshCoins();
            GUIManager.Instance.RefreshPoints();
        }

        /// <summary>
        /// Changes the amount of points that get added every second.
        /// </summary>
        /// <param name="points">Amount of points to be added per second.</param>
        public void SetPointsPerSecond(float points) {
            pointsPerSecond = 0.0f;
            pointsPerSecond = points;
        }

        /// <summary>
        /// Starts or stops auto incrementing the score.
        /// </summary>
        /// <param name="status">True to auto increment, false to stop.</param>
        public void AutoIncrementScore(bool status) {
            canAddPoints = status;

            if(status)
                StartCoroutine(IncrementScore());
            else
                StopCoroutine(IncrementScore());
        }

        /// <summary>
        /// Adds points to the current score.
        /// </summary>
        /// <param name="points">Amount of points to add.</param>
        public void AddPoints(float points) {
            this.points += points;

            //Update GUI
            GUIManager.Instance.RefreshPoints();
        }

        /// <summary>
        /// Sets the points to a certain number.
        /// </summary>
        /// <param name="points">Number to set points to.</param>
        public void SetPoints(float points) {
            this.points = points;

            //Update GUI
            GUIManager.Instance.RefreshPoints();
        }

        /// <summary>
        /// Sets the high score.
        /// </summary>
        /// <param name="highScore">The new high score.</param>
        public void SetHighScore(int highScore) {
            this.highScore = highScore;
        }

        /// <summary>
        /// Adds coins to the total amount of coins.
        /// </summary>
        /// <param name="coins">Amount of coins to add.</param>
        public void AddCoins(int coins) {
            this.coins += coins;

            //Update GUI
            GUIManager.Instance.RefreshCoins();
        }

        /// <summary>
        /// Sets the coins to a certain number.
        /// </summary>
        /// <param name="coins">Number to set coins to.</param>
        public void SetCoins(int coins) {
            this.coins = coins;

            //Update GUI
            GUIManager.Instance.RefreshCoins();
        }

        /// <summary>
        /// Sets the timescale.
        /// </summary>
        /// <param name="timeScale">New timescale value.</param>
        public void SetTimeScale(float timeScale) {
            savedTimeScale = Time.timeScale;
            Time.timeScale = timeScale;
        }

        /// <summary>
        /// Resets the time scale back to it's last saved time.
        /// </summary>
        public void ResetTimeScale() {
            Time.timeScale = savedTimeScale;
        }

        /// <summary>
		/// Pauses the game.
		/// </summary>
		public void Pause() {	
            if(Time.timeScale > 0.0f) {
                SetTimeScale(0.0f);
                statusBeforePause = Instance.status;
                SetStatus(GameStatus.GamePaused);
            } else
                UnPause();
        }

        /// <summary>
        /// Unpauses the game.
        /// </summary>
        public void UnPause() {
            ResetTimeScale();
            Instance.SetStatus(statusBeforePause);
        }
        #endregion

        #region IEnumerator methods
        /// <summary>
		/// Each 0.01 second, increments the score by 1/100th of the number of points it's supposed to increase each second
		/// </summary>
		/// <returns>The score.</returns>
        IEnumerator IncrementScore() {
            while(canAddPoints) {
                if(status == GameStatus.GameInProgress && pointsPerSecond != 0)
                    AddPoints(pointsPerSecond / 100);

                yield return new WaitForSeconds(0.01f);
            }
        }
        #endregion

        #endregion
    }
}
