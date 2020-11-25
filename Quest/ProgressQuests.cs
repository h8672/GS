using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.Quest
{
    /// <summary>
    /// Progress quests.
    /// Saves all quests and their current progress.
    /// </summary>
    [System.Serializable]
    public class ProgressQuests : GS.Data.ProgressSave
    {
        [SerializeField]
        private QuestProgress[] quests = { };
        public QuestProgress[] Quests { get { return quests; } }

        [SerializeField]
        private QuestProgress[] activeQuests = { };

        private void LoadQuestChange()
        {
            if (QuestManager.Instance.changedQuest.change.Equals(QuestManager.QuestChange.None)) return;

            switch (QuestManager.Instance.changedQuest.change)
            {
                case QuestManager.QuestChange.Added:
                    for (int i = 0; i < quests.Length; i++)
                    {
                        // Fetch current QuestProgress.
                        if (quests[i].line.Equals(QuestManager.Instance.changedQuest.quest))
                        {
                            quests[i].currentQuest = QuestManager.Instance.changedQuest.currentQuest;
                            quests[i].state = QuestManager.Instance.changedQuest.state;

                            // Fetch current quest task progress.
                            // If there's no save after quest start, after load, there would be no tasks to do.
                            foreach (QuestProgress _quest in QuestManager.Instance.quests)
                            {
                                if (_quest.line.Equals(quests[i].line))
                                {
                                    quests[i].tasks = _quest.tasks;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                case QuestManager.QuestChange.Change:
                    for (int i = 0; i < quests.Length; i++)
                    {
                        // Fetch current QuestProgress.
                        if (quests[i].line.Equals(QuestManager.Instance.changedQuest.quest))
                        {
                            quests[i].currentQuest = QuestManager.Instance.changedQuest.currentQuest;
                            quests[i].state = QuestManager.Instance.changedQuest.state;
                            break;
                        }
                    }
                    break;
                case QuestManager.QuestChange.Task:
                    for (int i = 0; i < quests.Length; i++)
                    {
                        if (quests[i].line.Equals(QuestManager.Instance.changedQuest.quest))
                        {
                            // Fetch current quest progress.
                            foreach (QuestProgress _quest in QuestManager.Instance.quests)
                            {
                                if (_quest.line.Equals(quests[i].line))
                                {
                                    quests[i].tasks = _quest.tasks;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                default:
                    // Removed quest from QuestManager active list.
                    //case QuestManager.QuestChange.Removed:
                    for (int i = 0; i < quests.Length; i++)
                    {
                        // Fetch current QuestProgress.
                        if (quests[i].line.Equals(QuestManager.Instance.changedQuest.quest))
                        {
                            quests[i].currentQuest = QuestManager.Instance.changedQuest.currentQuest;
                            quests[i].state = QuestManager.Instance.changedQuest.state;
                            quests[i].tasks = null;
                            break;
                        }
                    }
                    break;
            }
            QuestProgress[] _quests = QuestManager.Instance.quests;

            // Clear quest change.
            QuestManager.Instance.changedQuest.change = QuestManager.QuestChange.None;
        }

        public override void ProcessAfterLoad()
        {
            QuestManager.Instance.quests = activeQuests;
            QuestManager.Instance.ActivateTasks();
        }

        public override void ProcessForSave()
        {
            QuestManager.Instance.DeactivateTasks();
            activeQuests = QuestManager.Instance.quests;
        }

        public override void ProcessUpdate()
        {
            LoadQuestChange();
        }
    }
}
