using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.Data {

    /// <summary>
    /// Saves and reads progress data.
    /// Contains base implementation of various progresses.
    /// Handled by ProgressManager.
    /// </summary>
    [System.Serializable]
    public abstract class ProgressSave
    {
        /// <summary>
        /// Ready the script for the save process.
        /// Data is saved in ProgressManager.
        /// </summary>
        public abstract void ProcessForSave();

        /// <summary>
        /// Loads the required data after it's read from file.
        /// There might be some data that's not saved.
        /// Data is read in ProgressManager.
        /// </summary>
        public abstract void ProcessAfterLoad();

        /// <summary>
        /// Processes the update.
        /// </summary>
        public abstract void ProcessUpdate();
    }
}