using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controller.UI.BottomMenuUI;
using Controller.UI.BottomMenuUI.BottomMenuPanel.InventoryPanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadConfigurePanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.SummonPanel;
using Creature.Data;
using Data;
using Function;
using Managers.BattleManager;
using Managers.BottomMenuManager.InventoryPanel;
using Managers.BottomMenuManager.SquadPanel;
using Managers.GameManager;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Managers.BottomMenuManager.SummonPanel
{
    public class SummonManager : MonoBehaviour
    {
        public static SummonManager Instance;

        public SummonSo summonSo;
        public Enums.SummonType currentSummonType;
        public readonly Dictionary<string, int> SummonedItemDictionary = new();

        public SummonLevel SquadSummonLevel;
        public SummonLevel WeaponSummonLevel;
        public SummonLevel GearSummonLevel;

        private Enums.CharacterType[] squadType;
        private Enums.EquipmentType[] weaponType;
        private Enums.EquipmentType[] gearType;
        
        private WeightedRandomPicker<string> squadSummoner;
        private WeightedRandomPicker<string> weaponSummoner;
        private WeightedRandomPicker<string> gearSummoner;
        
        private readonly WaitForSeconds summonWaitForSeconds = new(0.05f);

        private void Awake()
        {
            Instance = this;
        }

        public void InitSummonManager()
        {
            InitializeSummonLevel();
            InitializeSummoner();
        }

        private void InitializeSummonLevel()
        {
            SquadSummonLevel = new SummonLevel(Enums.SummonType.Squad);
            WeaponSummonLevel = new SummonLevel(Enums.SummonType.Weapon);
            GearSummonLevel = new SummonLevel(Enums.SummonType.Gear);
            
            SquadSummonLevel.InitializeData();
            WeaponSummonLevel.InitializeData();
            GearSummonLevel.InitializeData();

            UIManager.Instance.summonPanelUI.summonPanelScrollViewItems[0].GetComponent<SummonPanelItemUI>().UpdateSummonPanelItemUI(SquadSummonLevel.CurrentSummonLevel, SquadSummonLevel.CurrentSummonExp, SquadSummonLevel.TargetSummonExp);
            UIManager.Instance.summonPanelUI.summonPanelScrollViewItems[1].GetComponent<SummonPanelItemUI>().UpdateSummonPanelItemUI(WeaponSummonLevel.CurrentSummonLevel, WeaponSummonLevel.CurrentSummonExp, WeaponSummonLevel.TargetSummonExp);
            UIManager.Instance.summonPanelUI.summonPanelScrollViewItems[2].GetComponent<SummonPanelItemUI>().UpdateSummonPanelItemUI(GearSummonLevel.CurrentSummonLevel, GearSummonLevel.CurrentSummonExp, GearSummonLevel.TargetSummonExp);
        }
        
        private void InitializeSummoner()
        {
            squadSummoner = new WeightedRandomPicker<string>();
            weaponSummoner = new WeightedRandomPicker<string>();
            gearSummoner = new WeightedRandomPicker<string>();
            
            squadType = new[] { Enums.CharacterType.Warrior, Enums.CharacterType.Archer, Enums.CharacterType.Wizard };
            weaponType = new[] { Enums.EquipmentType.Sword, Enums.EquipmentType.Bow, Enums.EquipmentType.Staff };
            gearType = new[] { Enums.EquipmentType.Helmet, Enums.EquipmentType.Armor, Enums.EquipmentType.Gauntlet };
            
            for (var i = 0 ; i < summonSo.SummonGears[SquadSummonLevel.CurrentSummonLevel - 1].SummonProbabilities.Length; i++)
            {
                squadSummoner.Add($"{(Enums.CharacterRarity) i}", summonSo.SummonGears[SquadSummonLevel.CurrentSummonLevel - 1].SummonProbabilities[i]);
            }
            
            for (var i = 0 ; i < summonSo.SummonWeapons[WeaponSummonLevel.CurrentSummonLevel - 1].SummonProbabilities.Length; i++)
            {
                weaponSummoner.Add($"{(Enums.EquipmentRarity) i}", summonSo.SummonWeapons[SquadSummonLevel.CurrentSummonLevel - 1].SummonProbabilities[i]);
            }
            
            for (var i = 0 ; i < summonSo.SummonGears[GearSummonLevel.CurrentSummonLevel - 1].SummonProbabilities.Length; i++)
            {
                gearSummoner.Add($"{(Enums.EquipmentRarity) i}", summonSo.SummonGears[GearSummonLevel.CurrentSummonLevel - 1].SummonProbabilities[i]);
            }
        }

        public void SummonRandomTarget(Enums.SummonType type, int count)
        {
            if (!AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.Dia, count * 10)) return;

            switch (type)
            {
                case Enums.SummonType.Squad:
                    QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.SummonSquad, count);
                    break;
                case Enums.SummonType.Weapon:
                    QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.SummonWeapon, count);
                    break;
                case Enums.SummonType.Gear:
                    QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.SummonGear, count);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            SummonedItemDictionary.Clear();
            currentSummonType = type;
            UIManager.Instance.summonPanelUI.summonResultPanelUI.gameObject.SetActive(true);

            foreach (var summonResultPanelItem in UIManager.Instance.summonPanelUI.summonResultPanelUI.summonResultPanelItems.TakeWhile(summonResultPanelItem => summonResultPanelItem.gameObject.activeInHierarchy))
            {
                summonResultPanelItem.gameObject.SetActive(false);
            }

            var dictionaryCount = 0;
            
            for (var i = 0; i < count; i++)
            {
                var randomTier = Random.Range(1, 6);

                var targetName = type switch
                {
                    Enums.SummonType.Squad =>
                        $"{squadSummoner?.GetRandomPick()}_{squadType[Random.Range(0, 3)]}",
                    Enums.SummonType.Weapon =>
                         $"{weaponSummoner?.GetRandomPick()}_{randomTier}_{weaponType[Random.Range(0, 3)]}",
                    Enums.SummonType.Gear =>
                        $"{gearSummoner?.GetRandomPick()}_{randomTier}_{gearType[Random.Range(0, 3)]}",
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };

                if (!CheckDictionaryKey(SummonedItemDictionary, targetName))
                {
                    SummonedItemDictionary.Add(targetName, 1);
                    dictionaryCount++;
                }
                else
                {
                    SummonedItemDictionary[targetName]++;
                }
            }

            foreach (var item in SummonedItemDictionary)
            {
                Debug.Log($"가챠! {item.Key} {item.Value}개");
            }
            
            var summonLists = SummonedItemDictionary.ToList();

            for (var i = 0; i < summonLists.Count; i++)
            {
                var target = summonLists[i];
                var splitString = target.Key.Split('_');
                
                if (type == Enums.SummonType.Squad)
                {
                    Character targetCharacter;
                    SquadConfigurePanelItemUI squadConfigurePanelScrollViewItem;
                    
                    var targetType = (Enums.CharacterType)Enum.Parse(typeof(Enums.CharacterType), splitString[1]);

                    switch (targetType)
                    {
                        case Enums.CharacterType.Warrior:
                            targetCharacter = SquadConfigureManager.Instance.WarriorDictionary[target.Key];
                            squadConfigurePanelScrollViewItem =
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI
                                    .squadConfigureScrollViewItemWarriors.Find(character =>
                                        character.GetComponent<SquadConfigurePanelItemUI>().characterName.text ==
                                        targetCharacter.characterName).GetComponent<SquadConfigurePanelItemUI>();
                            break;
                        case Enums.CharacterType.Archer:
                            targetCharacter = SquadConfigureManager.Instance.ArchersDictionary[target.Key];
                            squadConfigurePanelScrollViewItem =
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI
                                    .squadConfigureScrollViewItemArchers.Find(character =>
                                        character.GetComponent<SquadConfigurePanelItemUI>().characterName.text ==
                                        targetCharacter.characterName).GetComponent<SquadConfigurePanelItemUI>();
                            break;
                        case Enums.CharacterType.Wizard:
                            targetCharacter = SquadConfigureManager.Instance.WizardsDictionary[target.Key];
                            squadConfigurePanelScrollViewItem =
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI
                                    .squadConfigureScrollViewItemWizards.Find(character =>
                                        character.GetComponent<SquadConfigurePanelItemUI>().characterName.text ==
                                        targetCharacter.characterName).GetComponent<SquadConfigurePanelItemUI>();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    targetCharacter.characterQuantity++;
                    
                    if (targetCharacter.isPossessed == false)
                    {
                        targetCharacter.isPossessed = true;
                        targetCharacter.characterLevel = 1;
                        targetCharacter.SaveCharacterEquippedInfo(targetCharacter.characterId);
                    }

                    targetCharacter.SaveCharacterPossessedInfo(targetCharacter.characterId);
                    
                    squadConfigurePanelScrollViewItem.UpdateSquadConfigureAllItemUI(targetCharacter.characterLevel, targetCharacter.isEquipped, targetCharacter.isPossessed, targetCharacter.characterName, SpriteManager.Instance.GetCharacterSprite(targetCharacter.characterType, targetCharacter.characterIconIndex));
                    
                    UIManager.Instance.summonPanelUI.summonResultPanelUI.summonResultPanelItems[i]
                        .GetComponent<SummonResultPanelItemUI>().UpdateSummonResultPanelCharacterItemUI(
                            SpriteManager.Instance.GetCharacterSprite(targetCharacter.characterType,
                                targetCharacter.characterIconIndex), (int)targetCharacter.characterRarity, target.Value);
                }
                else
                {
                    Equipment targetEquipment;
                    InventoryPanelItemUI inventoryScrollViewItem;
                    
                    var targetRarityIndex = (int)Enum.Parse(typeof(Enums.EquipmentRarity), splitString[0]) * 5;
                    var targetType = (Enums.EquipmentType)Enum.Parse(typeof(Enums.EquipmentType), splitString[2]);
                    var targetTierIndex = 5 - Convert.ToInt32(splitString[1]);
                    var targetIndex = targetRarityIndex + targetTierIndex;
                        
                    switch (targetType)
                    {
                        case Enums.EquipmentType.Sword:
                            targetEquipment = InventoryManager.Instance.SwordsDictionary[target.Key];
                            inventoryScrollViewItem = UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemSwords[targetIndex].GetComponent<InventoryPanelItemUI>();
                            break;
                        case Enums.EquipmentType.Bow:
                            targetEquipment = InventoryManager.Instance.BowsDictionary[target.Key];
                            inventoryScrollViewItem = UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemBows[targetIndex].GetComponent<InventoryPanelItemUI>();
                            break;
                        case Enums.EquipmentType.Staff:
                            targetEquipment = InventoryManager.Instance.StaffsDictionary[target.Key];
                            inventoryScrollViewItem = UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemStaffs[targetIndex].GetComponent<InventoryPanelItemUI>();
                            break;
                        case Enums.EquipmentType.Helmet:
                            targetEquipment = InventoryManager.Instance.HelmetsDictionary[target.Key];
                            inventoryScrollViewItem = UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemHelmets[targetIndex].GetComponent<InventoryPanelItemUI>();
                            break;
                        case Enums.EquipmentType.Armor:
                            targetEquipment = InventoryManager.Instance.ArmorsDictionary[target.Key];
                            inventoryScrollViewItem = UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemArmors[targetIndex].GetComponent<InventoryPanelItemUI>();
                            break;
                        case Enums.EquipmentType.Gauntlet:
                            targetEquipment = InventoryManager.Instance.GauntletsDictionary[target.Key];
                            inventoryScrollViewItem = UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemGauntlets[targetIndex].GetComponent<InventoryPanelItemUI>();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                
                    targetEquipment.equipmentQuantity += target.Value;
                    
                    if (targetEquipment.isPossessed == false)
                    {
                        targetEquipment.isPossessed = true;
                        targetEquipment.SaveEquipmentEachInfo(targetEquipment.equipmentId, Enums.EquipmentProperty.IsPossessed);
                    }
                    
                    targetEquipment.SaveEquipmentEachInfo(targetEquipment.equipmentId, Enums.EquipmentProperty.Quantity);

                    inventoryScrollViewItem.UpdateInventoryPanelItemQuantityUI(targetEquipment.equipmentQuantity);
                    inventoryScrollViewItem.UpdateInventoryPanelItemPossessMark(targetEquipment.isPossessed);
                    
                    UIManager.Instance.summonPanelUI.summonResultPanelUI.summonResultPanelItems[i]
                        .GetComponent<SummonResultPanelItemUI>().UpdateSummonResultPanelEquipmentItemUI(
                            targetEquipment.equipmentTier,
                            SpriteManager.Instance.GetEquipmentSprite(targetEquipment.equipmentType,
                                targetEquipment.equipmentIconIndex), (int)targetEquipment.equipmentRarity, target.Value);
                }
            }

            IncreaseSummonExp(type, count);
            StartCoroutine(SummonEffect(dictionaryCount));
            UIManager.Instance.summonPanelUI.summonResultPanelUI.UpdateSummonResultPanelUI();
        }

        private IEnumerator SummonEffect(int index)
        {
            for (var i = 0 ; i < index ; i++)
            {
                UIManager.Instance.summonPanelUI.summonResultPanelUI.summonResultPanelItems[i].gameObject.SetActive(true);
                UIManager.Instance.summonPanelUI.summonResultPanelUI.summonResultPanelItems[i].StartSummonEffect();
                yield return summonWaitForSeconds;
            }
        }
        
        private void IncreaseSummonExp(Enums.SummonType type, int count)
        {
            switch (type)
            {
                case Enums.SummonType.Squad:
                    if (SquadSummonLevel.IncreaseExp(count))
                    {
                        squadSummoner = new WeightedRandomPicker<string>();
                        for (var i = 0 ; i < summonSo.SummonSquads[SquadSummonLevel.CurrentSummonLevel - 1].SummonProbabilities.Length; i++)
                        {
                            squadSummoner.Add($"{(Enums.CharacterRarity) i}", summonSo.SummonSquads[SquadSummonLevel.CurrentSummonLevel - 1].SummonProbabilities[i]);
                        }
                    }
                    UIManager.Instance.summonPanelUI.summonPanelScrollViewItems[0].GetComponent<SummonPanelItemUI>().UpdateSummonPanelItemUI(SquadSummonLevel.CurrentSummonLevel, SquadSummonLevel.CurrentSummonExp, SquadSummonLevel.TargetSummonExp);
                    break;
                case Enums.SummonType.Weapon:
                    if (WeaponSummonLevel.IncreaseExp(count))
                    {
                        weaponSummoner = new WeightedRandomPicker<string>();
                        for (var i = 0 ; i < summonSo.SummonWeapons[WeaponSummonLevel.CurrentSummonLevel - 1].SummonProbabilities.Length; i++)
                        {
                            weaponSummoner.Add($"{(Enums.EquipmentRarity) i}", summonSo.SummonWeapons[WeaponSummonLevel.CurrentSummonLevel - 1].SummonProbabilities[i]);
                        }
                    }
                    UIManager.Instance.summonPanelUI.summonPanelScrollViewItems[1].GetComponent<SummonPanelItemUI>().UpdateSummonPanelItemUI(WeaponSummonLevel.CurrentSummonLevel, WeaponSummonLevel.CurrentSummonExp, WeaponSummonLevel.TargetSummonExp);
                    break;
                case Enums.SummonType.Gear:
                    if (GearSummonLevel.IncreaseExp(count))
                    {
                        gearSummoner = new WeightedRandomPicker<string>();
                        for (var i = 0 ; i < summonSo.SummonGears[GearSummonLevel.CurrentSummonLevel - 1].SummonProbabilities.Length; i++)
                        {
                            gearSummoner.Add($"{(Enums.EquipmentRarity) i}", summonSo.SummonGears[GearSummonLevel.CurrentSummonLevel - 1].SummonProbabilities[i]);
                        }
                    }
                    UIManager.Instance.summonPanelUI.summonPanelScrollViewItems[2].GetComponent<SummonPanelItemUI>().UpdateSummonPanelItemUI(GearSummonLevel.CurrentSummonLevel, GearSummonLevel.CurrentSummonExp, GearSummonLevel.TargetSummonExp);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static bool CheckDictionaryKey<TK, TV>(Dictionary<TK, TV> dict, TK key)
        {
            return dict.Any(kvp => kvp.Key.Equals(key));
        }
    }
}