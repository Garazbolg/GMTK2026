namespace DevCore.ApplicationStates {
    /// <summary>
    /// Application values managed by states
    /// 
    /// To add an override:
    ///  1) Add the value type in Overrides
    ///  2) Set its default value in the Default constructor
    /// </summary>
    public struct ApplicationStateOverrides {
        #region Overrides
        public bool canPause;
        public float timeScale;
        public bool playerHasControl;
        public bool displayInGameUI;
        public bool isCursorVisible;

        // 1) Add any override you need in this field  
        //public bool test; 
        #endregion

        public static ApplicationStateOverrides Default => new ApplicationStateOverrides() {
            canPause = true,
            timeScale = 1f,
            playerHasControl = true,
            displayInGameUI = true,
            isCursorVisible = false
        
            // 2) Set the default value of the value here
            //test = true 
        };
    }
}
