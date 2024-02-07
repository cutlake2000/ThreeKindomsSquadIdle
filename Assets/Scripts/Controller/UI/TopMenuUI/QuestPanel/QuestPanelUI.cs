using System;
using System.Linq;
using Controller.UI.BottomMenuUI;
using Data;
using Managers.BattleManager;
using Managers.BottomMenuManager.InventoryPanel;
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
                foreach (var t in QuestManager.Instance.questTargets.Where(t => t.questType == QuestManager.Instance.currentQuest.questType))
                {
                    if (t.questType == Enums.QuestType.AutoEquipSword)
                    {
                        foreach (var sword in InventoryManager.Instance.SwordsDictionary.Where(sword => sword.Value.isEquipped))
                        {
                            UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(sword.Value);
                        }
                    }
                    
                    foreach (var target in t.activeTarget)
                    {
                        target.SetActive(true);
                    }
                    foreach (var target in t.inactiveTarget)
                    {
                        target.SetActive(false);
                    }
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