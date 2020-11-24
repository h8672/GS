using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.RPG.Quest
{
    public class QuestRewardManager : MonoBehaviour
    {
        #region Singleton

        private static QuestRewardManager manager = null;
        public static QuestRewardManager Instance
        {
            get
            {
                //EventManager is not initialized
                if (!manager)
                {
                    manager = FindObjectOfType(typeof(QuestRewardManager)) as QuestRewardManager;

                    //No EventManager in scene
                    if (!manager)
                    {
                        Debug.LogError("There needs to be one active QuestRewardManager script on a GameObject in your scene");
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

        private void Init()
        {
            if (manager == null)
            {
                manager = new QuestRewardManager();
            }
        }

        #endregion // Singleton

        public void GiveQuestReward(QuestReward _link, bool _hiddenReward)
        {
            // Works C# 7 and above. I didn't know this...
            switch (_link.data)
            {
                case GS.RPG.Values.CharacterData c:
                    Debug.Log("Character reward found! " + c.name);
                    // ReputationSystem.AddReputation(c, _link.amount);
                    // BuddySystem.AddBuddy(c, _link.amount);
                    break;
                default:
                    Debug.Log("Missing reward type from list. " + _link.data.name);
                    break;
            }
            // TODO Trigger event for reward popup...?
            if(!_hiddenReward) { RewardPopUp(_link); }
        }

        private void RewardPopUp(QuestReward _link)
        {

        }
    }
}
