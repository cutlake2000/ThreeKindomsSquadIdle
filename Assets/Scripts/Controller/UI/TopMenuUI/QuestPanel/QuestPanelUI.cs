using System;
using Controller.UI.BottomMenuUI;
using Data;
using Managers.BattleManager;
using Managers.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.TopMenuUI.QuestPanel
{
    public class QuestPanelUI : MonoBehaviour
    {
        [Header("--- 퀘스트 보상 창 ---")] public QuestResultPanelUI questResultPanelUI;
        
        public GameObject completedMark;
        public Image questRewardImage;
        public TMP_Text questRewardText;
        public TMP_Text questDescription;

        public void InitializeEventListeners()
        {
            questResultPanelUI.InitializeEventListeners();
            gameObject.GetComponent<Button>().onClick.AddListener(CheckQuestClear);
        }

        private void CheckQuestClear()
        {
            if (QuestManager.Instance.currentQuest.progress >= QuestManager.Instance.currentQuest.targetProgress)
            {
                QuestManager.Instance.TargetQuestClear();
            }
            else
            {
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