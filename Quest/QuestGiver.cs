using System.Collections.Generic;
using UnityEngine;

namespace GS.Quest
{
    /// <summary>
    /// Quest indicate.
    /// Value type to check for which indicator should be shown.
    /// </summary>
    public enum QuestGiverIndicator
    {
        Nothing, StartQuest, EndQuest
    }

    /// <summary>
    /// Quest giver component for non player characters.
    /// Quests should be added through QuestLink.
    /// </summary>
    [RequireComponent(typeof(GS.Data.ObjectStatus))]
    public class QuestGiver : MonoBehaviour, GS.Controls.Interface.Interact
    {
        [SerializeField]
        QuestLink[] givableQuests = { };

        [SerializeField] private readonly float questCheckCooldown = 60f;
        private float checkCooldown = 0f;

        private GS.Data.ObjectData questGiver;

        /// <summary>
        /// Gets the indicator type.
        /// Open list or end quest or just show the QuestGiver if he has a quest for the player.
        /// </summary>
        /// <value>The indicator.</value>
        public QuestGiverIndicator indicator { get; private set; }

        private void Awake() {
            questGiver = GetComponent<GS.Data.ObjectStatus>().Object;
        }

        /// <summary>
        /// Updates the quests.
        /// Fetches all available quests using QuestManager.
        /// </summary>
        public void UpdateQuests()
        {
            if (checkCooldown < Time.time)
            {
                checkCooldown = Time.time + questCheckCooldown;
                List<QuestLink> _quests = new List<QuestLink>();
                _quests.AddRange(QuestManager.Instance.GetQuestGiverQuests(GetComponent<GS.Data.ObjectData>()));
                givableQuests = _quests.ToArray();
                IsQuestGiverEnabled();
            }
        }

        /// <summary>
        /// Checks if there's no givable quests and disables or enables itself.
        /// </summary>
        private void IsQuestGiverEnabled()
        {
            // If there's no quests, disable component if it's enabled.
            if (givableQuests.Length.Equals(0).Equals(this.enabled))
            {
                this.enabled = false;
            }
            // There's quests, was quest giver disabled? Then enable it.
            else if (this.enabled.Equals(false))
            {
                this.enabled = true;
            }
        }

        public void NearingQuestGiver()
        {
            UpdateQuests();

            if (this.enabled)
            {
                // Find quest to end, else find quest to give, else show nothing.
                List<QuestLink> links = new List<QuestLink>();
                links.AddRange(givableQuests);
                if (links.Find((obj) => obj.questEnd == questGiver))
                {
                    indicator = QuestGiverIndicator.EndQuest;
                }
                else if (links.Find((obj) => obj.questStart == questGiver))
                {
                    indicator = QuestGiverIndicator.StartQuest;
                }
                else { indicator = QuestGiverIndicator.Nothing; }
            }
        }

        public void LeavingQuestGiver()
        {
            if (this.enabled)
            {
                indicator = QuestGiverIndicator.Nothing;
            }
        }

        public QuestLink[] GetAvailableQuests()
        {
            return givableQuests;
        }
        public void QuestAccept(QuestLine _line, float _cooldown = -1f)
        {
            checkCooldown = Time.time + (_cooldown < 0 ? questCheckCooldown : _cooldown);
            QuestManager.Instance.QuestAccepted(_line);
        }
        public void QuestDecline(QuestLine _line, float _cooldown = -1f)
        {
            checkCooldown = Time.time + (_cooldown < 0 ? questCheckCooldown : _cooldown);
            QuestManager.Instance.QuestDeclined(_line);
        }
        public void QuestEnd(QuestLine _line, QuestRewardType[] _selection, float _cooldown = -1f)
        {
            checkCooldown = Time.time + (_cooldown < 0 ? questCheckCooldown : _cooldown);
            QuestManager.Instance.QuestComplete(_line, QuestList.Instance.GetSelectedRewards());
        }

        /// <summary>
        /// Interact this instance.
        /// </summary>
        public void Interact()
        {
            QuestList.Instance.StartQuestList(this);
        }
    }
}
