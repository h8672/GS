using System.Collections.Generic;
using UnityEngine;

namespace GS.Quest
{
    [CreateAssetMenu(fileName = "New QuestData", menuName = "GS/Quest/QuestData")]
    [System.Serializable]
    public class QuestData : ScriptableObject
    {
        #region Quest values

        public int index;
        public new string name;
        public string description;

        public bool isRepeatable = true;
        public bool isDeclinable = true;
        public bool isDismissable = true;

        [Tooltip("Upon failure, decline or cancel the quest cannot be restarted.")]
        public bool canRestart = true;

        // TODO Requirement format for better requirements. Level, completed quest, discovered area, million undead kills?
        // TODO Possibly similar to QuestTask or QuestReward format?
        [Tooltip("Quests that are to be completed before this quest can be accessed.")]
        public QuestData[] requiredQuests;

        [SerializeField]
        public QuestReward[] rewards = { };

        [SerializeField]
        public QuestTask[] tasks = { };

        #endregion // Quest values

        /// <summary>
        /// Awards all rewards for the quest that fit those selected QuestRewardTypes.
        /// </summary>
        /// <param name="_types">Flag that tells QuestReward which options were selected for rewarding.</param>
        public void AwardRewards(QuestRewardType[] _types)
        {
            for(int i = 0; i < rewards.Length; i++)
            {
                // Give quest reward if the it's selected.
                if (_types[i].HasFlag(rewards[i].type))
                {
                    QuestRewardManager.Instance.GiveQuestReward(rewards[i], _types[i].HasFlag(QuestRewardType.Hidden));
                }
            }
        }

        /// <summary>
        /// Gets the public reward data as an array.
        /// Supports multiple rewards and reward groups.
        /// </summary>
        public QuestReward[] GetRewardData()
        {
            List<QuestReward> list = new List<QuestReward>();
            list.AddRange(rewards);
            return list.FindAll((obj) => !obj.type.HasFlag(QuestRewardType.Hidden)).ToArray();
        }
    }
}