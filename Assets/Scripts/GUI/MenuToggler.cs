using UnityEngine;

public class MenuToggler : MonoBehaviour {
    [SerializeField]
    GameObject backMenuImage = null;

    Animator anim = null;
    GameObject instantiatedBackMenu = null;

    #region Unity methods
    void Awake() {
        anim = GetComponent<Animator>();
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Toggles the UI menu.
    /// </summary>
    public void ToggleMenu() {
        //If menu is not active, make it active. This is needed for animating the menu.
        bool isActive = gameObject.activeInHierarchy;
        if(!isActive)
            gameObject.SetActive(true);
        
        //Toggle menu if animating isn't an option
        if(anim == null) {
            gameObject.SetActive(!isActive);
            return;
        }
        
        //Animate the menu
        anim.SetTrigger(Constants.ANIM_UI_TOGGLEMENU);
    }

    /// <summary>
    /// Enables the UI menu.
    /// </summary>
    public void EnableMenu() {
        gameObject.SetActive(true);
        //CreateBackMenu();

        //Animate the menu
        if(anim != null)
            anim.SetTrigger(Constants.ANIM_UI_FADEINMENU);
    }

    /// <summary>
    /// Disables the UI menu.
    /// </summary>
    public void DisableMenu() {
        gameObject.SetActive(false);
        Destroy(instantiatedBackMenu);
    }

    /// <summary>
    /// Disables the UI menu with an animation. For this to work, an AnimationEvent must be at the end of an animation that disables the menu.
    /// If it is not, the menu will just stay on screen.
    /// </summary>
    /// <param name="disableBackMenu">If true, the back menu will be disabled too(as long as there is one present).</param>
    public void DisableMenuWithAnimation(bool disableBackMenu = true) {
        if(anim != null)
            anim.SetTrigger(Constants.ANIM_UI_FADEOUTMENU);

        //Disable back menu
        if(disableBackMenu)
            Destroy(instantiatedBackMenu);
    }

    /// <summary>
    /// Creates the background image of the menu.
    /// </summary>
    public void CreateBackMenu() {
        //Create background image
        if(backMenuImage != null) {
            instantiatedBackMenu = Instantiate(backMenuImage, transform.parent);
            
            //Set new index
            instantiatedBackMenu.transform.SetSiblingIndex(transform.GetSiblingIndex());

            //Set transform
            instantiatedBackMenu.transform.localPosition = Vector3.zero;
            instantiatedBackMenu.transform.localRotation = Quaternion.Euler(Vector3.zero);
            instantiatedBackMenu.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
    #endregion
}
