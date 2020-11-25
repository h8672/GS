using System.Collections.Generic;
using UnityEngine;

namespace GS.Quest
{
    public class QuestManager : MonoBehaviour
    {
        #region Singleton

        private static QuestManager manager;
        public static QuestManager Instance
        {
            get
            {
                //EventManager is not initialized
                if (!manager)
                {
                    manager = FindObjectOfType(typeof(QuestManager)) as QuestManager;

                    //No EventManager in scene
                    if (!manager)
                    {
                        Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene");
                    }
                    else
                    {
                        //Initialize EventManager
                        manager.Init();
                    }
                }
                //All good!
                return manager;
            }
        }

        // ProgressQuests is part of ProgressManager.
        // Collects the process of quests and handles the saves.
        // Acts as a backup and isn't used as quest manager.
        private ProgressQuests progress;
        private void Init()
        {
            if (manager == null)
            {
                manager = new QuestManager();

                progress = new ProgressQuests();
                GS.Data.ProgressManager.Instance.AddProgress(progress);
            }
        }

        #endregion // Singleton
        #region Values

        // DEBUG LOGS! DO NOT LOCALIZE THESE!
        public const string questAcceptedText = "Quest {0} was accepted!";
        public const string questCancelledText = "Quest {0} was cancelled!";
        public const string questCompleteText = "Quest {0} was completed and player returned for rewards";
        public const string questWasNotComplete = "Quest {0} was not complete!";

        // Publcic quest trigger names for event listeners.
        public const string QuestAcceptTrigger = "QuestAccepted";
        public const string QuestCancelTrigger = "QuestCancelled";
        public const string QuestCompleteTrigger = "QuestComplete";
        public const string QuestIncompleteTrigger = "QuestIncomplete";

        // This is Quest system stuff...
        // TODO Remake system to use QuestProgress data.
        //public QuestData[] activeQuests = { };
        public QuestProgress[] quests = { };

        // ProgressQuests wants to know what has changed.
        public enum QuestChange { None, Added, Removed, Change, Task }
        public struct QuestChanged
        {
            public QuestChange change;
            public QuestLine quest;
            public int currentQuest;
            public QuestState state;

            public QuestChanged(QuestChange _change, QuestLine _quest, int _currentQuest, QuestState _state)
            {
                change = _change;
                quest = _quest;
                currentQuest = _currentQuest;
                state = _state;
            }
        };
        public QuestChanged changedQuest = new QuestChanged(QuestChange.None, null, -1,  QuestState.Searching);

        #endregion // Values

        #region Quest Methods

        // QuestGiver doesn't need to know quests.
        // They are given to them either in the scene or using QuestFinder.
        //public void GetQuestGiverQuests(QuestGiver _giver) {  }
        public QuestLink[] GetQuestGiverQuests(GS.Data.ObjectData _object)
        {
            // Search QuestGiver through all QuestLinks.
            List<QuestLink> links = new List<QuestLink>();
            links.AddRange(Resources.FindObjectsOfTypeAll<QuestLink>());
            links = links.FindAll((obj) => (obj.questStart == _object || obj.questEnd == _object));

            List<QuestProgress> list = new List<QuestProgress>();
            list.AddRange(quests);
            list.AddRange(progress.Quests);
            list.TrimExcess();

            List<QuestProgress> knownQuests = new List<QuestProgress>();
            // Search links through known quests.
            // Take all quests as done or locked quests are parsed away later.
            foreach (QuestLink _link in links)
            {
                knownQuests.AddRange(list.FindAll((obj) => obj.line == _link.line));
            }

            // If some quests aren't known, Add them to list if they are available.
            if(links.Count != knownQuests.Count)
            {
                // Look through required quests and check if requirements are met using QuestData.
                list = new List<QuestProgress>();
                foreach(QuestLink _link in links)
                {
                    if(knownQuests.FindAll((obj) => (obj.line == _link.line)).Count.Equals(0))
                    {
                        QuestProgress _quest;
                        _quest.line = _link.line;
                        _quest.currentQuest = 0;
                        _quest.state = QuestState.Searching;
                        _quest.tasks = null;
                        list.Add(_quest);
                    }
                }

                // If quest meets the requirements.
                List<QuestProgress> removes = new List<QuestProgress>();
                List<QuestProgress> _list = new List<QuestProgress>();
                _list.AddRange(progress.Quests);
                foreach (QuestProgress _linkQuest in list)
                {
                    QuestProgress quest;
                    foreach(QuestData _data in _linkQuest.line.quests[_linkQuest.currentQuest].requiredQuests)
                    {
                        // Get the QuestLine QuestProgress.
                        quest = _list.Find((obj) => (obj.line.quests[obj.currentQuest] == _data));

                        // Are these required quests completed?
                        if (quest.line != null)
                        {
                            // QuestLine has been seen before.
                            if (quest.line.ContainsQuest(_data, out int position))
                            {
                                // If required quest is still not done.
                                if (quest.currentQuest <= position && !quest.state.Equals(QuestState.Done))
                                {
                                    removes.Add(_linkQuest);
                                    break;
                                }
                            }
                        }
                        // Never seen this QuestLine before.
                        else
                        {
                            removes.Add(_linkQuest);
                            break; 
                        }
                    }
                }

                // Remove quests from list as it's unavailable.
                foreach (QuestProgress _quest in removes)
                {
                    list.Remove(_quest);
                }
            }
            else { list = new List<QuestProgress>(); }

            // Combine known and unknown quests to single list.
            list.AddRange(
                // Remove completed or locked quests from the list.
                knownQuests.FindAll((obj) => !(obj.state == QuestState.Done || obj.state == QuestState.Locked))
            );

            // Remove links which doesn't meet the requirements.
            List<QuestLink> removeLink = new List<QuestLink>();
            foreach (QuestLink _link in links)
            {
                // If QuestLine isn't found, add it to remove list.
                if (list.FindAll((obj) => obj.line == _link.line).Count.Equals(0))
                {
                    removeLink.Add(_link);
                }
            }

            // Remove QuestLinks which QuestLine couldn't be started yet.
            foreach (QuestLink _link in removeLink)
            {
                links.Remove(_link);
            }

            // Finally get to return list of quest links...
            return links.ToArray();
        }

        public void ActivateQuest(QuestProgress _quest)
        {
            List<QuestProgress> _quests = new List<QuestProgress>();
            _quests.AddRange(quests);
            _quests.Add(_quest);
            quests = _quests.ToArray();
            ActivateTask(_quests.IndexOf(_quest));
            //_quests.Clear();
            changedQuest = new QuestChanged(QuestChange.Added, _quest.line, _quest.currentQuest, _quest.state);
        }
        public void DeactivateQuest(QuestProgress _quest)
        {
            List<QuestProgress> _quests = new List<QuestProgress>();
            _quests.AddRange(quests);
            DeactivateTask(_quests.IndexOf(_quest));
            _quests.Remove(_quest);
            quests = _quests.ToArray();
            //_quests.Clear();
            changedQuest = new QuestChanged(QuestChange.Removed, _quest.line, _quest.currentQuest, _quest.state);
        }

        public void QuestAccepted(QuestLine _line)
        {
            // Accept quest with QuestProgress.
            QuestProgress quest;
            quest.line = null;
            quest.currentQuest = -1;
            quest.state = QuestState.Started;
            quest.tasks = null;

            // Go through active quests.
            foreach (QuestProgress _quest in quests)
            {
                if (_quest.line == _line)
                {
                    quest.line = _quest.line;
                    quest.currentQuest = _quest.currentQuest;
                    quest.state = _quest.state;
                    break;
                }
            }
            if (quest.line == null)
            {
                // Check if questline has ever appeared before.
                foreach (QuestProgress _quest in progress.Quests)
                {
                    if (_quest.line == _line)
                    {
                        quest.line = _quest.line;
                        quest.currentQuest = _quest.currentQuest;
                        quest.state = _quest.state;
                        break;
                    }
                }

                // No quest was found! Create new QuestProgress.
                if (quest.line == null)
                {
                    quest.line = _line;
                    quest.currentQuest = 0;
                }
            }

            // Populate tasks
            QuestData data = quest.line.quests[quest.currentQuest];
            switch (quest.state)
            {
                case QuestState.Started:
                    quest.tasks = data.tasks;
                    break;
                case QuestState.Declined:
                    quest.state = (data.isRepeatable && data.isDeclinable && data.canRestart ? QuestState.Started : QuestState.Locked);
                    quest.tasks = (quest.state == QuestState.Started ? data.tasks : null);
                    break;
                case QuestState.Dismissed:
                    quest.state = (data.isRepeatable && data.isDismissable && data.canRestart ? QuestState.Started : QuestState.Locked);
                    quest.tasks = (quest.state == QuestState.Started ? data.tasks : null);
                    break;
                case QuestState.Failure:
                    quest.state = (data.isRepeatable && data.canRestart ? QuestState.Started : QuestState.Locked);
                    quest.tasks = (quest.state == QuestState.Started ? data.tasks : null);
                    break;
                case QuestState.Locked:
                    // Quest has been permanently failed and QuestLine is lost.
                    quest.tasks = null;
                    break;
                default:
                    // Quest was already made. Need to next questline quest.
                    quest.currentQuest++;
                    quest.state = QuestState.Searching;
                    quest.tasks = null;
                    break;
            }

            // Add quest progress to active quest list.
            ActivateQuest(quest);

            Debug.Log(string.Format(questAcceptedText, quest.line.quests[quest.currentQuest].name), this);

            GS.Data.EventManager.TriggerEvent(QuestAcceptTrigger);
        }

        // To reduce repeat code.
        private void QuestDeactivator(int _quest, QuestState _state, string _logFormatText, string _trigger)
        {
            QuestProgress quest = quests[_quest];
            Debug.Log(string.Format(_logFormatText, quest.line.quests[quest.currentQuest].name));
            quest.state = _state;

            DeactivateQuest(quest);
            GS.Data.EventManager.TriggerEvent(_trigger);
        }
        public void QuestDeclined(QuestLine _line)
        {
            List<QuestProgress> list = new List<QuestProgress>();
            list.AddRange(quests);

            QuestDeactivator(
                list.FindIndex((obj) => obj.line == _line),
                QuestState.Declined, questCancelledText, QuestCancelTrigger
            );
        }
        private int GetQuestIndex(QuestLine _line)
        {
            List<QuestProgress> _quests = new List<QuestProgress>();
            _quests.AddRange(QuestManager.Instance.quests);
            return _quests.FindIndex((obj) => obj.line.Equals(_line));
        }
        private void QuestInComplete(int _index)
        {
            // If quest state is wrong. Perhaps it was ended to make some space for other quests...
            Debug.Log(string.Format(questWasNotComplete, quests[_index].line.quests[quests[_index].currentQuest].name));
            GS.Data.EventManager.TriggerEvent(QuestIncompleteTrigger);
        }
        public void QuestComplete(QuestLine _line, QuestRewardType[] _types)
        {
            int _index = GetQuestIndex(_line);
            // Quest tasks are handled by QuestData.
            if (IsQuestComplete(GetQuestIndex(_line)))
            {
                QuestProgress quest = quests[_index];
                // If quest is started.
                if (quest.state == QuestState.Started)
                {
                    // Quest completed.
                    quest.currentQuest++;

                    // Is quest line complete or is there more quests?
                    quest.state = (
                        (quest.line.quests.Length > quest.currentQuest)
                        ? QuestState.Searching : QuestState.Done
                    );

                    // Fixing current quest index.
                    if (quest.line.quests.Length == quest.currentQuest)
                    {
                        quest.currentQuest--;
                    }

                    // Each QuestReward type work differently based on RewardType.
                    quest.line.quests[quest.currentQuest].AwardRewards(_types);

                    // TODO Tell player which quest was completed.
                    // Or to save and load progress...
                    // Or to QuestTracker (local, online, session etc.)
                    // Perhaps through event manager?
                    // New idea: EventManager told and QuestProgress checks the quest list.
                    // Quest data is saved by ProgressManager, which handles SaveProgress<T> classes.

                    // Deactivate quest.
                    Debug.Log(string.Format(questCompleteText, quest.line.quests[quest.currentQuest].name));
                    DeactivateQuest(quest);
                    GS.Data.EventManager.TriggerEvent(QuestCompleteTrigger);
                    return;
                }
            }
            QuestInComplete(_index);
        }
        public void QuestDismissed(QuestLine _line)
        {
            QuestDeactivator(GetQuestIndex(_line), QuestState.Dismissed, questCancelledText, QuestCancelTrigger);
        }
        public void QuestFailed(QuestLine _line)
        {
            QuestDeactivator(GetQuestIndex(_line), QuestState.Failure, questCancelledText, QuestCancelTrigger);
        }

        #endregion // Quest Methods

        #region Task Methods

        // Private task methods
        private bool IsQuestComplete(int _quest)
        {
            int tasksComplete = 0;
            QuestProgress questProgress = quests[_quest];
            foreach (QuestTask task in questProgress.tasks)
            {
                if (task.IsTaskCompleted()) { tasksComplete++; }
            }
            return tasksComplete.Equals(questProgress.tasks.Length);
        }
        private void ActivateTask(int _quest)
        {
            foreach (QuestTask task in quests[_quest].line.quests[quests[_quest].currentQuest].tasks)
            {
                task.ActivateTask();
            }
        }
        private void DeactivateTask(int _quest)
        {
            foreach (QuestTask task in quests[_quest].line.quests[quests[_quest].currentQuest].tasks)
            {
                task.DeactivateTask();
            }
        }

        // Public task methods
        // TODO Localize progress format texts. If null, use these?
        // Format strings used in Quest methods.
        public static string questProgressText = "Quest {0} progress:\n";
        public static string questTaskCompleteText = "- {0} completed!\n";
        public static string questTaskFormatText = "- {0}";

        public string GetQuestProgress(int _quest)
        {
            string data = string.Format(questProgressText, name);
            foreach (QuestTask task in quests[_quest].tasks)
            {
                if (task.IsTaskCompleted())
                {
                    data += string.Format(questTaskCompleteText, task.GetTaskData());
                }
                else
                {
                    data += string.Format(questTaskFormatText, task.GetTaskData());
                }
            }
            return data;
        }
        public QuestTask[] GetQuestTasks(int _quest)
        {
            return quests[_quest].line.quests[quests[_quest].currentQuest].tasks;
        }
        public void ActivateTasks()
        {
            foreach (QuestProgress quest in quests)
            {
                foreach (QuestTask task in quest.line.quests[quest.currentQuest].tasks)
                {
                    task.ActivateTask();
                }
            }
        }
        public void DeactivateTasks()
        {
            foreach (QuestProgress quest in quests)
            {
                foreach (QuestTask task in quest.line.quests[quest.currentQuest].tasks)
                {
                    task.DeactivateTask();
                }
            }
        }

        #endregion // Task Methods
    }
}