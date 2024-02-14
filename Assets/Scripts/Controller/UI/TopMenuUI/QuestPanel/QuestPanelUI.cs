using System;
using System.Linq;
using Controller.UI.BottomMenuUI;
using Data;
using Managers.BattleManager;
using Managers.BottomMenuManager.InventoryPanel;
using Managers.BottomMenuManager.SquadPanel;
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
            if (QuestManager.Instance.initialQuestMark.activeInHierarchy) QuestManager.Instance.initialQuestMark.SetActive(false);
            
            if (QuestManager.Instance.isCurrentQuestClear)
            {
                if (QuestManager.Instance.currentQuestTarget.targetMarks != null)
                {
                    foreach (var tm in QuestManager.Instance.currentQuestTarget.targetMarks)
                    {
                        tm.SetActive(false);
                    }
                }
                
                QuestManager.Instance.isCurrentQuestClear = false;
                QuestManager.Instance.TargetQuestClear();
            }
            else if (QuestManager.Instance.currentQuest.questType != Enums.QuestType.StageClear)
            {
                QuestManager.Instance.backboardPanel.SetActive(true);
                
                foreach (var tm in QuestManager.Instance.currentQuestTarget.targetMarks)
                {
                    tm.SetActive(true);
                }
                
                switch (QuestManager.Instance.currentQuestTarget.questType)
                {
                    case Enums.QuestType.AutoEquipSword:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.SwordsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.AutoEquipBow:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.BowsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.AutoEquipStaff:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.StaffsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.AutoEquipHelmet:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.HelmetsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.AutoEquipArmor:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.ArmorsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.AutoEquipGauntlet:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.GauntletsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.SummonWeapon10:
                        UIManager.Instance.summonPanelUI.SetScrollViewVerticalPosition(0.5f);
                        break;
                    case Enums.QuestType.SummonGear10:
                        UIManager.Instance.summonPanelUI.SetScrollViewVerticalPosition(0f);
                        break;
                    case Enums.QuestType.SummonSquad10:
                        UIManager.Instance.summonPanelUI.SetScrollViewVerticalPosition(1f);
                        break;
                    case Enums.QuestType.SummonWeapon100:
                        UIManager.Instance.summonPanelUI.SetScrollViewVerticalPosition(0.5f);
                        break;
                    case Enums.QuestType.SummonGear100:
                        UIManager.Instance.summonPanelUI.SetScrollViewVerticalPosition(0f);
                        break;
                    case Enums.QuestType.EquipSquad:
                        UIManager.Instance.squadPanelUI.squadConfigurePanelUI.currentSelectedSquadConfigurePanelItem = SquadConfigureManager.Instance.WarriorDictionary.Where(keyValuePair => keyValuePair.Value.characterId == "Rare_Warrior").ToList()[0].Value;
                        UIManager.Instance.squadPanelUI.squadConfigurePanelUI.UpdateSquadConfigurePanelSelectedCharacterInfoUI(UIManager.Instance.squadPanelUI.squadConfigurePanelUI.currentSelectedSquadConfigurePanelItem);
                        break;
                    case Enums.QuestType.CompositeSword:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.SwordsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.CompositeBow:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.BowsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.CompositeStaff:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.StaffsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.CompositeHelmet:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.HelmetsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.CompositeArmor:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.ArmorsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.CompositeGauntlet:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.GauntletsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.LevelUpCharacter:
                        break;
                    case Enums.QuestType.AttackTalentLevel:
                        break;
                    case Enums.QuestType.HealthTalentLevel:
                        break;
                    case Enums.QuestType.StageClear:
                        break;
                    case Enums.QuestType.PlayGoldDungeon:
                        break;
                    case Enums.QuestType.PlayEnhanceStoneDungeon:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                foreach (var target in QuestManager.Instance.currentQuestTarget.activeTarget)
                {
                    target.SetActive(true);
                }
                foreach (var target in QuestManager.Instance.currentQuestTarget.inactiveTarget)
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