using System;

namespace App.Events {
    /// <summary>
    /// 
    /// </summary>
    public static class Game {
        // * CONDITIONALS
        /// <summary>
        /// 
        /// </summary>
        public static Action OnGameOver;
        /// <summary>
        /// 
        /// </summary>
        //public static Action OnGameWon;
        /// <summary>
        /// 
        /// </summary>
        //public static Action OnGameReset;

        // * PROGRESS
        /// <summary>
        /// 
        /// </summary>
        //public static Action<string> OnMilestoneCompleted;
        
        // * INTERRUPTIONS
        /// <summary>
        /// 
        /// </summary>
        public static Action OnGamePaused;
        /// <summary>
        /// 
        /// </summary>
        public static Action OnGameUnPaused;
    }
}