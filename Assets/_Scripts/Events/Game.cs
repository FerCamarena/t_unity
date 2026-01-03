using System;

namespace App.Events {
    public static class InGame {
        // * CONDITIONALS
        public static Action OnGameOver;
        //public static Action OnGameWon;
        //public static Action OnGameReset;

        // * PROGRESS
        //public static Action<string> OnMilestoneCompleted;
        
        // * INTERRUPTIONS
        public static Action OnGamePaused;
        public static Action OnGameUnPaused;
    }
}