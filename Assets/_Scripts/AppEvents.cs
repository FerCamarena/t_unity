using System;

namespace App.Events {
    public static class Application {
        // * APP
        public static Action OnAppOpened;
        public static Action OnAppClosed;
        public static Action OnAppPaused;
        public static Action OnAppResumed;
        //public static Action OnAppCrashed;
        // * USER
        public static Action OnFirstTimeOpened;
        public static Action OnFirstTimeLogged;
        public static Action OnPlayerLogged;
        public static Action OnPlayerUnlogged;
        //public static Action OnFirstPurchaseMade;
        // * SESSION
        public static Action OnSessionStarted;
        public static Action OnSessionEnded;
        public static Action OnSessionResumed;
    }
}