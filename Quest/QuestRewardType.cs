namespace GS.Quest
{
    /// <summary>
    /// Reward types for QuestReward.
    /// Used to manage QuestReward listing.
    /// </summary>
    [System.Flags]
    public enum QuestRewardType
    {
        // Explanation:
        // - Primary and Secondary can be grouped up to 3 different reward groups.
        // - Add Group1 to Group3 if the reward is optional, else GroupNone.
        // - Hidden can't be shown on lists as QuestReward wont tell about them.
        // - Decline if declining rewards might give some other rewards.
        //
        // Test importance:
        // - GroupNone > Group1 > Group2 > Group3
        // - Primary > Secondary
        // - Hidden > Decline

        // Default Search values
        Nothing = 0x00,
        Primary = 0x01,
        Secondary = 0x02,
        Decline = 0x04,
        Hidden = 0x08,
        Group1 = 0x10,
        Group2 = 0x20,
        Group3 = 0x40,

        // If you want to specify you didn't want any reward at all.
        GroupNone = 0x80,

        // Clear flags.
        Clear_GroupFlag = Group1 | Group2 | Group3 | GroupNone,
        Clear_RewardFlag = Primary | Secondary,
        Clear_Flag = Clear_GroupFlag | Clear_RewardFlag,

    }
}
    /*/ Found these enums unnessessary. It just grew the pickup selection too much and can't hide most of them.
    // With System.Flags attribute UnityEditor allows you to pick each flag at a time.

    // Normal rewards
    Set_PrimaryNone = GroupNone | Primary,
    Set_Primary1 = Group1 | Primary,
    Set_Primary2 = Group2 | Primary,
    Set_Primary3 = Group3 | Primary,
    Set_SecondaryNone = GroupNone | Secondary,
    Set_Secondary1 = Group1 | Secondary,
    Set_Secondary2 = Group2 | Secondary,
    Set_Secondary3 = Group3 | Secondary,
    Set_All = GroupNone | Primary | Secondary,
    Set_All_Primary = GroupNone | Primary,
    Set_All_Secondary = GroupNone | Secondary,

    // Declined rewards
    Set_Decline_PrimaryNone = Set_PrimaryNone | Decline,
    Set_Decline_Primary1 = Set_Primary1 | Decline,
    Set_Decline_Primary2 = Set_Primary2 | Decline,
    Set_Decline_Primary3 = Set_Primary3 | Decline,
    Set_Decline_Secondary = Set_SecondaryNone | Decline,
    Set_Decline_Secondary1 = Set_Secondary1 | Decline,
    Set_Decline_Secondary2 = Set_Secondary2 | Decline,
    Set_Decline_Secondary3 = Set_Secondary3 | Decline,
    Set_Decline_All = Set_All | Decline,
    Set_Decline_All_Primary = Set_All_Primary | Decline,
    Set_Decline_All_Secondary = Set_All_Secondary | Decline,

    // Hidden rewards
    Set_Hidden_PrimaryNone = Set_PrimaryNone | Hidden,
    Set_Hidden_Primary1 = Set_Primary1 | Hidden,
    Set_Hidden_Primary2 = Set_Primary2 | Hidden,
    Set_Hidden_Primary3 = Set_Primary3 | Hidden,
    Set_Hidden_SecondaryNone = GroupNone | Secondary | Hidden,
    Set_Hidden_Secondary1 = Group1 | Secondary | Hidden,
    Set_Hidden_Secondary2 = Group2 | Secondary | Hidden,
    Set_Hidden_Secondary3 = Group3 | Secondary | Hidden,
    Set_Hidden_All = Set_All | Hidden,
    Set_Hidden_All_Primary = Set_All_Primary | Hidden,
    Set_Hidden_All_Secondary = Set_All_Secondary | Hidden,

    // Hidden declined rewards
    Set_Hidden_Decline_PrimaryNone = Set_Hidden_PrimaryNone | Decline,
    Set_Hidden_Decline_Primary1 = Set_Hidden_Primary1 | Decline,
    Set_Hidden_Decline_Primary2 = Set_Hidden_Primary2 | Decline,
    Set_Hidden_Decline_Primary3 = Set_Hidden_Primary3 | Decline,
    Set_Hidden_Decline_Secondary = Set_Hidden_SecondaryNone | Decline,
    Set_Hidden_Decline_Secondary1 = Set_Hidden_Secondary1 | Decline,
    Set_Hidden_Decline_Secondary2 = Set_Hidden_Secondary2 | Decline,
    Set_Hidden_Decline_Secondary3 = Set_Hidden_Secondary3 | Decline,
    Set_Hidden_Decline_All = Set_Hidden_All | Decline,
    Set_Hidden_Decline_All_Primary = Set_Hidden_All_Primary | Decline,
    Set_Hidden_Decline_All_Secondary = Set_Hidden_All_Secondary | Decline,
    // */