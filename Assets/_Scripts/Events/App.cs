using System;

namespace App.Events {
    /// <summary>
    /// 
    /// </summary>
    public static class Application {
        // * APP
        /// <summary>
        /// 
        /// </summary>
        public static Action OnAppOpened;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnAppClosed;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnAppPaused;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnAppResumed;
        /// <summary>
        /// 
        /// </summary>
        //public static Action OnAppCrashed;

        // * USER
        /// <summary>
        /// 
        /// </summary>
        public static Action OnFirstTimeOpened;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnFirstTimeLogged;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnPlayerLogged;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnPlayerUnlogged;
        /// <summary>
        /// 
        /// </summary>
        //public static Action OnFirstPurchaseMade;
        
        // * SESSION
        /// <summary>
        /// 
        /// </summary>
        public static Action OnNewGameSession; //NewGame button on main menu
        /// <summary>
        /// 
        /// </summary>
        public static Action OnSessionStarted; //Playing a game directly
        /// <summary>
        /// 
        /// </summary>
        public static Action OnSessionResumed; //Restored from crash/exit
        /// <summary>
        /// 
        /// </summary>
        public static Action OnSessionEnded; //Terminating a run
        
        /// <summary>
        /// 
        /// </summary>
        public static Action<string> OnShowSubtitle;

        public static Action OnMenuLoad;
    }
}