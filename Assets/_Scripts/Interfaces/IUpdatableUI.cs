using UnityEngine;

namespace App.Game.UI {
    /// <summary>
    /// 
    /// </summary>
    public interface IUpdatableUI {
        /// <summary>
        /// 
        /// </summary>
    // ? PARAMETERS=================================================================================================================================
        // * REFERENCES
        //[Header("References")]

        // * ATTRIBUTES
        //[Header("Attributes")]
        Component Target { get; }
        int ElementID { get; }
        
        // * INTERNALS
        //[Header("Internals")]

    // ? BASE METHODS===============================================================================================================================

    // ? CUSTOM METHODS=============================================================================================================================
        /// <summary>
        /// 
        /// </summary>
        void SyncUI();

    // ? EVENT METHODS==============================================================================================================================

    }
}