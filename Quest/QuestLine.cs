using System.Collections.Generic;
using UnityEngine;

namespace GS.Quest
{
    [CreateAssetMenu(fileName = "New QuestLine", menuName = "GS/Quest/QuestLine")]
    [System.Serializable]
    public class QuestLine : ScriptableObject
    {
        public int index;
        public new string name;
        public string description;
        public bool repeatable = false;

        public QuestData[] quests;

        /// <summary>
        /// If quest chain has the quest, will return the position of it.
        /// </summary>
        /// <returns><c>true</c>, if quest was containsed, <c>false</c> otherwise.</returns>
        /// <param name="_quest">Quest.</param>
        /// <param name="_index">Index.</param>
        public bool ContainsQuest(QuestData _quest, out int _index)
        {
            List<QuestData> list = new List<QuestData>();
            list.AddRange(quests);
            _index = list.IndexOf(_quest);
            list.Clear();
            return _index > 0;
        }
    }
}
