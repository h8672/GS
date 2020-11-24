using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.RPG.Quest
{
    /// <summary>
    /// Quest state.
    /// </summary>
    public enum QuestState
    {
        Searching,  // Quest location unknown
        Started,    // Quest started
        Declined,   // Quest declined
        Dismissed,  // Quest dismissed
        Done,       // Quest complete
        Failure,    // Quest failed
        Locked      // Quest unavailable
    }

    /// <summary>
    /// Quest progress.
    /// Quest progress data file.
    /// </summary>
    [System.Serializable]
    public struct QuestProgress
    {
        public QuestLine line;
        public int currentQuest;
        public QuestState state;
        public QuestTask[] tasks;
    }
}
