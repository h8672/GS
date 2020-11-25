using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GS.Quest
{
    [RequireComponent(typeof(QuestGiver))]
    public class QuestIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject startQuestPrefab = null;
        [SerializeField] private GameObject returnQuestPrefab = null;

        private QuestGiver giver;

        private void Awake()
        {
            if(!(startQuestPrefab == null || returnQuestPrefab == null))
            {
                this.enabled = true;
                Hide();
            }
            else
            {
                Debug.LogWarning("Missing indicator, load a preset if the same assets are used everytime.", this);
                this.enabled = false;
            }
            giver = GetComponent<QuestGiver>();
        }

        float cooldown = 0f;
        void Update()
        {
            if(cooldown < Time.time)
            {
                cooldown += 60f;
                if (giver.indicator.Equals(QuestGiverIndicator.EndQuest))
                {
                    Show(true);
                }
                if (giver.indicator.Equals(QuestGiverIndicator.StartQuest))
                {
                    Show(false);
                }
                else { Hide(); }
            }
        }

        public void Show(bool _returningQuest)
        {
            if (_returningQuest)
            {
                startQuestPrefab.SetActive(false);
                returnQuestPrefab.SetActive(true);
            }
            else
            {
                startQuestPrefab.SetActive(true);
                returnQuestPrefab.SetActive(false);
            }
            Debug.Log("Indicator ON", this);
        }
        public void Hide()
        {
            startQuestPrefab.SetActive(false);
            returnQuestPrefab.SetActive(false);
            Debug.Log("Indicator OFF", this);
        }
    }
}