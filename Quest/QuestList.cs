using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.Quest
{
    public class QuestList : MonoBehaviour
    {
        #region Singleton

        private static QuestList manager;
        public static QuestList Instance
        {
            get
            {
                //EventManager is not initialized
                if (!manager)
                {
                    manager = FindObjectOfType(typeof(QuestList)) as QuestList;

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
        // TODO enable Quest progress again?
        //private ProgressQuests progress;

        private void Init()
        {
            if (manager == null)
            {
                manager = new QuestList();

                // Perhaps create custom progress saver?
                // Or QuestManager creates it and it handles this save.
                //progress = new ProgressQuests();
                //GS.Data.ProgressManager.Instance.AddProgress(progress);
            }
        }

        #endregion // Singleton

        //private GS.RPG.Quest.QuestGiver giver;
        private QuestRewardType[] rewardTypes = { };
        private QuestRewardType[] selectedRewardTypes = { };
        private QuestLink[] quests = { };
        private int selectedQuest = 0;

        public UnityEngine.UI.Button startQuest, declineQuest, endQuest;

        public void GetSelected(out QuestLink _link, out QuestRewardType _types)
        {
            _link = quests[selectedQuest];
            _types = selectedRewardTypes[selectedQuest];
        }
        public void GetSelectedQuest(out QuestLink _link)
        {
            _link = quests[selectedQuest];
        }
        public string[] GetQuestNames()
        {
            List<string> names = new List<string>();
            foreach(QuestLink _link in quests)
            {
                names.Add(_link.quest.name);
            }
            return names.ToArray();
        }
        public string GetQuestDescription(int _quest)
        {
            return quests[_quest].quest.description;
        }
        public string[] GetQuestTasks(int _quest)
        {
            List<string> tasks = new List<string>();
            foreach (QuestTask _task in quests[_quest].quest.tasks)
            {
                tasks.Add(_task.GetTaskData());
            }
            return tasks.ToArray();
        }

        public void StartQuestList(QuestGiver _giver)
        {
            //giver = _giver;
            startQuest.onClick.AddListener( () => {
                GetSelectedQuest(out QuestLink _link);
                _giver.QuestAccept(_link.line);
                CloseQuestList();
            });

            declineQuest.onClick.AddListener( () => {
                GetSelectedQuest(out QuestLink _link);
                _giver.QuestDecline(_link.line);
                CloseQuestList();
            });

            endQuest.onClick.AddListener( () => {
                GetSelectedQuest(out QuestLink _link);
                _giver.QuestEnd(_link.line, GetSelectedRewards());
                CloseQuestList();
            });

            quests = _giver.GetAvailableQuests();

            List<QuestRewardType> availableTypes = new List<QuestRewardType>();
            List<QuestRewardType> selectedTypes = new List<QuestRewardType>();
            List<QuestReward> rewards = new List<QuestReward>();

            // Get all rewards for each quest.
            foreach (QuestLink _link in quests)
            {
                QuestReward[] _rewards = _link.quest.GetRewardData();

                // Get each RewardType flag for the quest.
                availableTypes.Add(QuestRewardType.Nothing);
                selectedTypes.Add(QuestRewardType.Nothing);
                for(int i = 0; i < _rewards.Length; i++)
                {
                    // Quest might have many available selections.
                    availableTypes[i] |= rewards[i].type;
                }

                rewards.AddRange(_rewards);
            }

            rewardTypes = availableTypes.ToArray();
            selectedRewardTypes = selectedTypes.ToArray();
        }
        public void CloseQuestList()
        {
            quests = null;
            selectedQuest = 0;
            rewardTypes = null;
            selectedRewardTypes = null;
        }

        public void SelectQuest(int _quest)
        {
            selectedQuest = _quest;
        }
        public void SelectQuestReward(int _reward, QuestRewardType _type)
        {
            if (!rewardTypes[_reward].HasFlag(_type))
            {
                Debug.LogError("Wrong selection for this quest reward!");
                return;
            }

            //Bitwise And Not(_type) operation with clear flag to ignore previous Primary/Secondary flag.
            if (selectedRewardTypes[_reward].HasFlag(_type))
            {
                selectedRewardTypes[_reward] &= ~(QuestRewardType.Clear_Flag);
            }
            selectedRewardTypes[_reward] |= _type;
        }
        public QuestRewardType[] GetSelectedRewards()
        {
            List<QuestRewardType> _types = new List<QuestRewardType>();
            _types.AddRange(selectedRewardTypes);

            for (int i = 0; i < _types.Count; i++)
            {
                _types[i] |= QuestRewardType.Hidden;
            }

            return _types.ToArray();
        }
    }
}