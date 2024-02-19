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
                
                QuestManager.Instance.UpdateQuestRewardPanelUI();
            }
            else if (QuestManager.Instance.currentQuest.questType != Enums.QuestType.InitialQuest
                     && QuestManager.Instance.currentQuest.questType != Enums.QuestType.StageClear
                     && QuestManager.Instance.currentQuest.questType != Enums.QuestType.TouchChallengeButton
                     && QuestManager.Instance.currentQuest.questType != Enums.QuestType.TouchLoopButton
                     && QuestManager.Instance.currentQuest.questType != Enums.QuestType.TouchAutoSkillButton
                     && QuestManager.Instance.currentQuest.questType != Enums.QuestType.ArcherCamera
                     && QuestManager.Instance.currentQuest.questType != Enums.QuestType.WarriorCamera)
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
                        UIManager.Instance.inventoryPanelUI.UpdateAutoEquipButton(InventoryManager.Instance.canAutoEquip[0]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(0);
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.AutoEquipBow:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.BowsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateAutoEquipButton(InventoryManager.Instance.canAutoEquip[1]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(1);
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.AutoEquipStaff:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.StaffsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateAutoEquipButton(InventoryManager.Instance.canAutoEquip[2]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(2);
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.AutoEquipHelmet:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.HelmetsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateAutoEquipButton(InventoryManager.Instance.canAutoEquip[3]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(3);
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.AutoEquipArmor:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.ArmorsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateAutoEquipButton(InventoryManager.Instance.canAutoEquip[4]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(4);
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.AutoEquipGauntlet:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.GauntletsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateAutoEquipButton(InventoryManager.Instance.canAutoEquip[5]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(5);
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
                        UIManager.Instance.inventoryPanelUI.UpdateAllCompositeButton(InventoryManager.Instance.canAllComposite[0]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(0);
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.CompositeBow:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.BowsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateAllCompositeButton(InventoryManager.Instance.canAllComposite[1]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(1);
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.CompositeStaff:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.StaffsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateAllCompositeButton(InventoryManager.Instance.canAllComposite[2]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(2);
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.CompositeHelmet:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.HelmetsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateAllCompositeButton(InventoryManager.Instance.canAllComposite[3]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(3);
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.CompositeArmor:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.ArmorsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateAllCompositeButton(InventoryManager.Instance.canAllComposite[4]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(4);
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.CompositeGauntlet:
                        UIManager.Instance.inventoryPanelUI.selectEquipment = InventoryManager.Instance.GauntletsDictionary.Where(keyValuePair => keyValuePair.Value.isEquipped).ToList()[0].Value;
                        UIManager.Instance.inventoryPanelUI.UpdateAllCompositeButton(InventoryManager.Instance.canAllComposite[5]);
                        UIManager.Instance.inventoryPanelUI.UpdateEquipmentTypeUI(5);
                        UIManager.Instance.inventoryPanelUI.UpdateSelectedEquipmentUI(UIManager.Instance.inventoryPanelUI.selectEquipment);
                        break;
                    case Enums.QuestType.LevelUpCharacter:
                        UIManager.Instance.squadPanelUI.squadConfigurePanelUI.currentSelectedSquadConfigurePanelItem = SquadConfigureManager.Instance.FindEquippedCharacter(Enums.CharacterType.Warrior);
                        UIManager.Instance.squadPanelUI.squadConfigurePanelUI.UpdateSquadConfigurePanelSelectedCharacterInfoUI(UIManager.Instance.squadPanelUI.squadConfigurePanelUI.currentSelectedSquadConfigurePanelItem);
                        break;
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
            
            QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.InitialQuest, 1);
        }

        public void UpdateQuestPanelUI(Sprite sprite, string reward, string description)
        {
            questRewardImage.sprite = sprite;
            questRewardText.text = reward;
            questDescription.text = description;
        }
    }
}