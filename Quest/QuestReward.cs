namespace GS.Quest
{
    /// <summary>
    /// Quest reward to specify QuestRewards with type and amounts.
    /// </summary>
    [System.Serializable]
    public struct QuestReward
    {
        public GS.Data.ObjectData data;
        public QuestRewardType type;
        public int rewardSize;

        public QuestReward(GS.Data.ObjectData _data = null, QuestRewardType _type = QuestRewardType.Primary, int _rewardSize = 1)
        {
            data = _data;
            type = _type;
            rewardSize = _rewardSize;
        }
    }
}