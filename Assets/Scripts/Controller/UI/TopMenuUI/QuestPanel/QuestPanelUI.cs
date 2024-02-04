using System;
using Data;
using Managers.BattleManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.TopMenuUI.QuestPanel
{
    public class QuestPanelUI : MonoBehaviour
    {
        public GameObject completedMark;
        public Image questRewardImage;
        public TMP_Text questRewardText;
        public TMP_Text questDescription;

        public void InitializeEventListeners()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(CheckQuestClear);
        }

        private void CheckQuestClear()
        {
            if (QuestManager.Instance.quests[QuestManager.Instance.questLevel % 5].progress >= QuestManager.Instance.quests[QuestManager.Instance.questLevel % 5].targetProgress)
            {
                QuestManager.Instance.TargetQuestClear();
            }
            else
            {
                if (QuestManager.Instance.questLevel % 5 == 4) return;
                
                foreach (var target in QuestManager.Instance.questTargets[QuestManager.Instance.questLevel % 5].activeTarget)
                {
                    target.SetActive(true);
                }
                foreach (var target in QuestManager.Instance.questTargets[QuestManager.Instance.questLevel % 5].inactiveTarget)
                {
                    target.SetActive(false);
                }
            }
        }

        public void UpdateQuestPanelUI(Sprite sprite, string reward, string description)
        {
            questRewardImage.sprite = sprite;
            questRewardText.text = reward;
            questDescription.text = description;
        }
    }
}