using System;
using KyleStankovich.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace KyleStankovich.BattleDash {
    /// <summary>
    /// Class responsible for handling the game's GUI.
    /// </summary>
    [AddComponentMenu("Battle Dash/Managers/GUI Manager")]
    public class GUIManager : Singleton<GUIManager> {
        [SerializeField]
        Canvas mainCanvas = null;
        
        [Header("Menus")]

        [Header("In-Game Menu")]
        [SerializeField]
        GameObject inGameMenu = null;

        [Header("Boss Battle Menu")]
        [SerializeField]
        GameObject bossBattleMenu = null;

        [Header("Victory Menu")]
        [SerializeField]
        GameObject victoryMenu = null;

        [Space(2)]
        [Header("Menu Specific GUI Items")]
        [Header("In-Game")]
        [SerializeField]
        Text inGameScoreText = null;
        [SerializeField]
        Text inGameCoinsText = null;

        [Header("Battle")]
        [SerializeField]
        Vector2 battleGUISpawnOffset = Vector3.zero;

        //Variables that will be loaded from the Resources folder
        Text swipeText = null;
        FloatingText floatingText = null;
        AttackArrow attackArrow = null;
        BlockingSlider blockingSlider = null;

        AttackArrow spawnedArrow = null;
        BlockingSlider spawnedBlockingSlider = null;

        enum AttackArrowRotationDirection {
            Right = 0,
            RightUp = 45,
            Up = 90,
            LeftUp = 135,
            Left = 180,
            LeftDown = 225,
            Down = 270,
            DownRight = 315
        }
        AttackArrowRotationDirection attackArrowRotationDir = AttackArrowRotationDirection.Right;

        #region Methods

        #region Unity methods
        new void Awake() {
            base.Awake();

            //Load some things from resources
            swipeText = (Resources.Load("GUI/Swipe Text") as GameObject).GetComponent<Text>();
            floatingText = (Resources.Load("GUI/Floating Text") as GameObject).GetComponent<FloatingText>();
            attackArrow = (Resources.Load("GUI/Attack Arrow") as GameObject).GetComponent<AttackArrow>();
            blockingSlider = (Resources.Load("GUI/Blocking Slider") as GameObject).GetComponent<BlockingSlider>();

            //Reference some things from the scene
            mainCanvas = GameObject.Find("Main Canvas").GetComponent<Canvas>();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Updates the in-game points GUI to match the current points accumulated.
        /// </summary>
        public void RefreshPoints() {
            if(inGameScoreText != null)
                inGameScoreText.text = Mathf.RoundToInt(GameManager.Instance.Points).ToString();
        }

        /// <summary>
        /// Updates the in-game coin GUI to match the current coins collected.
        /// </summary>
        public void RefreshCoins() {
            if(inGameCoinsText != null)
                inGameCoinsText.text = GameManager.Instance.InGameCoins.ToString();
        }

        /// <summary>
        /// Displays some text used for words after a swipe during boss battles.
        /// </summary>
        /// <param name="textContents">String information that will be displayed.</param>
        /// <param name="color">Color of the text.</param>
        /// <param name="pos">Position of the text.</param>
        /// <param name="destroyTime">Time to wait before text gets destroyed.</param>
        public void DisplaySwipeText(string textContents, Color color = default(Color), Vector2 pos = default(Vector2), float destroyTime = 1.0f) {
            if(swipeText == null)
                return;

            //Display the text
            Text text = Instantiate(swipeText, mainCanvas.transform, false).GetComponent<Text>();
            text.rectTransform.anchoredPosition = pos;
            text.text = textContents;
            text.color = color;

            //Destroy the text
            Destroy(text.gameObject, destroyTime);
        }

        /// <summary>
        /// Displays some floating text around a game object.
        /// </summary>
        /// <param name="spawnPoint">Transform where the floating text will spawn.</param>
        /// <param name="textContents">Contents of the text.</param>
        public void DisplayFloatingText(Transform spawnPoint, string textContents, bool parentIsCanvas = true) {
            if(spawnPoint == null)
                return;

            //Create the text
            FloatingText text = Instantiate(floatingText, mainCanvas.transform, false);
            Vector2 screenPosition =
                Camera.main.WorldToScreenPoint(new Vector2(spawnPoint.position.x + UnityEngine.Random.Range(-5.0f, 5.0f), spawnPoint.position.y + UnityEngine.Random.Range(0.0f, 5.0f)));

            //Set up the text
            text.transform.position = screenPosition;
            text.SetText(textContents);
        }

        /// <summary>
        /// Randomly spawns an attack arrow for boss battles.
        /// </summary>
        public AttackArrow SpawnAttackArrow() {
            //Instantiate arrow on screen
            if(mainCanvas != null)
                spawnedArrow = Instantiate(attackArrow, mainCanvas.transform, false) as AttackArrow;
            else
                spawnedArrow = Instantiate(attackArrow, GameObject.Find("Main Canvas").transform, false) as AttackArrow;

            if(spawnedArrow == null) {
                Debug.LogError("Could not spawn an attack arrow! Somehow it was null. Maybe check the file name in the Resources folder?");
                return null;
            }

            //Get the RectTransform
            RectTransform rTransform = spawnedArrow.gameObject.GetComponent<RectTransform>();

            //Set position with an offset
            rTransform.anchoredPosition = Vector2.zero + battleGUISpawnOffset;

            //Randomly choose a direction to face
            Array values = Enum.GetValues(typeof(AttackArrowRotationDirection));
            AttackArrowRotationDirection direction = (AttackArrowRotationDirection)values.GetValue(new System.Random().Next(values.Length));

            //Set rotation
            rTransform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, (int)direction));

            return spawnedArrow as AttackArrow;
        }

        /// <summary>
        /// Randomly spawns a blocking slider for boss battles.
        /// </summary>
        /// <returns></returns>
        public BlockingSlider SpawnBlockingSlider() {
            //Instantiate slider on screen
            if(mainCanvas != null)
                spawnedBlockingSlider = Instantiate(blockingSlider, mainCanvas.transform, false) as BlockingSlider;
            else
                spawnedBlockingSlider = Instantiate(blockingSlider, GameObject.Find("Main Canvas").transform, false) as BlockingSlider;

            if(spawnedBlockingSlider == null) {
                Debug.LogError("Could not spawn a blocking slider! Somehow it was null. Maybe check the file name in the Resources folder?");
                return null;
            }

            //Get the RectTransform
            RectTransform rTransform = spawnedBlockingSlider.gameObject.GetComponent<RectTransform>();

            //Set position with an offset
            rTransform.anchoredPosition = Vector2.zero + battleGUISpawnOffset;

            return spawnedBlockingSlider;
        }

        /// <summary>
        /// Updates a slider's value to a new value.
        /// </summary>
        /// <param name="slider">The slider to update.</param>
        /// <param name="value">Value to update to.</param>
        /// <param name="maxValue">The maximum possible value.</param>
        public void UpdateSlider(Slider slider, float value, float maxValue) {
            if(slider == null)
                return;

            float barVal = 0.0f;

            //Set bar value
            barVal = value / maxValue;
            barVal = Mathf.Clamp(barVal, 0.0f, 1.0f);
            slider.value = barVal;
        }

        /// <summary>
        /// Updates a slider's value to a new value smoothly.
        /// </summary>
        /// <param name="slider">The slider to update.</param>
        /// <param name="value">Value to update to.</param>
        /// <param name="maxValue">The maximum possible value.</param>
        public void UpdateSliderSmoothly(Slider slider, float value, float maxValue) {
            if(slider == null)
                return;

            float barVal = 0.0f;

            //Set bar value
            barVal = value / maxValue;
            barVal = Mathf.Clamp(barVal, 0.0f, 1.0f);
            slider.value = Mathf.Lerp(slider.value, barVal, 0.2f);
        }

        /// <summary>
        /// Disables the in-game menu.
        /// </summary>
        public void DisableInGameMenu() {
            DisableMenu(inGameMenu);
        }

        /// <summary>
        /// Enables the boss battle menu.
        /// </summary>
        public void EnableBossBattleMenu() {
            EnableMenu(bossBattleMenu);
        }

        /// <summary>
        /// Enables the victory menu.
        /// </summary>
        public void EnableVictoryMenu() {
            EnableMenu(victoryMenu);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Enables a menu.
        /// </summary>
        /// <param name="menu">Menu to enable.</param>
        void EnableMenu(GameObject menu) {
            if(menu == null)
                return;

            MenuToggler toggler = menu.GetComponent<MenuToggler>();

            if(toggler != null)
                toggler.EnableMenu();
            else
                Debug.LogError("The \"" + menu.name + "\" menu has not MenuToggler attached!");
        }

        /// <summary>
        /// Disables a menu.
        /// </summary>
        /// <param name="menu">Menu to disable.</param>
        void DisableMenu(GameObject menu) {
            if(menu == null)
                return;

            MenuToggler toggler = menu.GetComponent<MenuToggler>();

            if(toggler != null)
                toggler.DisableMenuWithAnimation(false);
            else
                Debug.LogError("The \"" + menu.name + "\" menu has not MenuToggler attached!");
        }
        #endregion

        #endregion
    }
}
