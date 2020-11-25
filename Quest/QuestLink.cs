using UnityEngine;

namespace GS.Quest
{
    [CreateAssetMenu(fileName = "New QuestLink", menuName = "GS/Quest/QuestLink")]
    [System.Serializable]
    public class QuestLink : ScriptableObject
    {
        public QuestLine line;
        public QuestData quest;
        public GS.Data.ObjectData questStart;
        public GS.Data.ObjectData questEnd;
    }
}
