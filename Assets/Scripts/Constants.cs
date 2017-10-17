/// <summary>
/// Class that contains all of the game's constants.
/// </summary>
public static class Constants {
    #region Player Prefs
    //Player events
    public const string PREF_CURRENCY = "playerCurrency";
    public const string PREF_HIGHSCORE = "playerHighScore";

    //Audio events
    public const string PREF_MASTERAUDIOLEVEL = "masterVolume";
    public const string PREF_MUSICAUDIOLEVEL = "musicVolume";
    public const string PREF_EFFECTAUDIOLEVEL = "effectVolume";
    #endregion

    #region Events
    //Main game events
    public const string EVENT_GAMESTART = "GameStart";
    public const string EVENT_GAMEEND = "GameEnd";
    public const string EVENT_OBSTACLEHIT = "ObstacleHit";
    public const string EVENT_PLAYEROUTOFBOUNDS = "PlayerOutOfBounds";

    //Input events
    public const string EVENT_INPUTUP = "InputOnSwipeUp";
    public const string EVENT_INPUTDOWN = "InputOnSwipeDown";
    public const string EVENT_INPUTLEFT = "InputOnSwipeLeft";
    public const string EVENT_INPUTRIGHT = "InputOnSwipeRight";

    //Boss battle events
    public const string EVENT_BOSSBATTLEBEGINCOMBAT = "BossBattleBeginCombat";
    public const string EVENT_BOSSBATTLEENDCOMBAT = "BossBattleEndCombat";
    public const string EVENT_BOSSBATTLEWON = "BossBattleWon";
    public const string EVENT_BOSSBATTLELOST = "BossBattleLost";
    #endregion

    #region Animation

    #region UI
    public const string ANIM_UI_TOGGLEMENU = "uiToggle";
    public const string ANIM_UI_FADEINMENU = "uiFadeIn";
    public const string ANIM_UI_FADEOUTMENU = "uiFadeOut";
    #endregion

    #endregion
}
