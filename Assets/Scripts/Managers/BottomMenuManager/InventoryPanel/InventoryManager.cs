using System;
using System.Collections.Generic;
using System.Linq;
using Controller.UI.BottomMenuUI.BottomMenuPanel.InventoryPanel;
using Creature.Data;
using Data;
using Managers.BattleManager;
using Managers.GameManager;
using Module;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.BottomMenuManager.InventoryPanel
{
    public class InventoryManager : MonoBehaviour
    {
        public const int MaxTier = 1;
        public const int MaxQuantity = 5;
        public static InventoryManager Instance;
        public static Dictionary<string, Equipment> AllEquipments = new();

        [Header("임시 장비 이름")] public List<string> swordNames = new();
        public List<string> bowNames = new();
        public List<string> staffNames = new();
        public List<string> helmetNames = new();
        public List<string> armorNames = new();
        public List<string> gauntletNames = new();
        public const int EquipmentMaxLevel = 250;

        public readonly Dictionary<string, Equipment> SwordsDictionary = new();
        public readonly Dictionary<string, Equipment> BowsDictionary = new();
        public readonly Dictionary<string, Equipment> StaffsDictionary = new();
        public readonly Dictionary<string, Equipment> HelmetsDictionary = new();
        public readonly Dictionary<string, Equipment> ArmorsDictionary = new();
        public readonly Dictionary<string, Equipment> GauntletsDictionary = new();

        private void Awake()
        {
            Instance = this;
        }

        // 장비 매니저 초기화 메서드
        public void InitEquipmentManager()
        {
            SetAllEquipment();
        }

        // 장비들 업데이트 하는 메서드
        private void SetAllEquipment()
        {
            if (ES3.KeyExists("Init_Game"))
                LoadAllEquipment();
            else
                CreateAllEquipment();
        }

        // 로컬에 저장되어 있는 장비 데이터들 불러오는 메서드
        private void LoadAllEquipment()
        {
            foreach (var equipmentType in Enums.equipmentTypes)
            {
                var equipmentIndex = 0;

                foreach (var rarity in Enums.equipmentRarities)
                {
                    var rarityIntValue = Convert.ToInt32(rarity);

                    for (var equipmentTier = 5; equipmentTier >= MaxTier; equipmentTier--)
                    {
                        var equipmentId = $"{rarity}_{equipmentTier}_{equipmentType}";
                        var equipmentName = equipmentType switch
                        {
                            Enums.EquipmentType.Sword => $"{swordNames[rarityIntValue]} {6 - equipmentTier}",
                            Enums.EquipmentType.Bow => $"{bowNames[rarityIntValue]} {6 - equipmentTier}",
                            Enums.EquipmentType.Staff => $"{staffNames[rarityIntValue]} {6 - equipmentTier}",
                            Enums.EquipmentType.Helmet => $"{helmetNames[rarityIntValue]} {6 - equipmentTier}",
                            Enums.EquipmentType.Armor => $"{armorNames[rarityIntValue]} {6 - equipmentTier}",
                            Enums.EquipmentType.Gauntlet => $"{gauntletNames[rarityIntValue]} {6 - equipmentTier}",
                            _ => null
                        };
                        var equipmentIconIndex = equipmentIndex;
                        var equipmentIcon = SpriteManager.Instance.GetEquipmentSprite(equipmentType, equipmentIndex);
                        var equipmentRarity = rarity;

                        var equippedEffects = new List<EquipmentEffect>();
                        var ownedEffects =  new List<EquipmentEffect>();

                        for (var i = 0 ; i < 2;i++)
                        {
                            var equippedEffect = new EquipmentEffect
                            {
                                statType = i == 0? Enums.StatTypeFromInventoryPanel.Attack : Enums.StatTypeFromInventoryPanel.Health,
                                increaseStatType = Enums.IncreaseStatValueType.BaseStat,
                            };
                            equippedEffects.Add(equippedEffect);
                        }
                        
                        for (var i = 0 ; i < 2;i++)
                        {
                            var ownedEffect = new EquipmentEffect
                            {
                                statType = i == 0 ? Enums.StatTypeFromInventoryPanel.Attack : Enums.StatTypeFromInventoryPanel.Health,
                                increaseStatType = Enums.IncreaseStatValueType.PercentStat,
                            };
                            ownedEffects.Add(ownedEffect);
                        }
                        
                        var equipment = new Equipment(equipmentId, equipmentName, equipmentIconIndex, equipmentIcon,
                            equipmentType, equipmentRarity, equipmentTier, equippedEffects, ownedEffects);
                        AddEquipment(equipmentId, equipment);

                        var targetScrollViewItem = equipmentType switch
                        {
                            Enums.EquipmentType.Sword => UIManager.Instance.inventoryPanelUI
                                .inventoryScrollViewItemSwords,
                            Enums.EquipmentType.Bow => UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemBows,
                            Enums.EquipmentType.Staff => UIManager.Instance.inventoryPanelUI
                                .inventoryScrollViewItemStaffs,
                            Enums.EquipmentType.Helmet => UIManager.Instance.inventoryPanelUI
                                .inventoryScrollViewItemHelmets,
                            Enums.EquipmentType.Armor => UIManager.Instance.inventoryPanelUI
                                .inventoryScrollViewItemArmors,
                            Enums.EquipmentType.Gauntlet => UIManager.Instance.inventoryPanelUI
                                .inventoryScrollViewItemGauntlets,
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        targetScrollViewItem[equipmentIndex]
                            .GetComponent<InventoryPanelItemUI>().UpdateInventoryPanelItemUI(
                                1,
                                EquipmentMaxLevel,
                                equipment.equipmentQuantity,
                                MaxQuantity,
                                equipment.isEquipped,
                                equipment.isPossessed,
                                equipmentTier,
                                SpriteManager.Instance.GetEquipmentSprite(
                                    equipmentType,
                                    equipmentIconIndex),
                                SpriteManager.Instance.GetEquipmentBackgroundEffect((int)equipmentRarity),
                                SpriteManager.Instance.GetEquipmentBackground((int)equipmentRarity));
                        
                        targetScrollViewItem[equipmentIndex].GetComponent<Button>().onClick.AddListener(() => InventoryPanelUI.SelectEquipmentAction(equipment));

                        equipmentIndex++;

                        if (equipment.isEquipped)
                        {
                            UIManager.Instance.inventoryPanelUI.equipmentButton[(int)equipmentType]
                                .GetComponent<InventoryPanelSelectedItemUI>()
                                .UpdateInventoryPanelSelectedItem(equipmentTier, equipmentIcon,
                                    SpriteManager.Instance.GetEquipmentBackgroundEffect((int)equipmentRarity),
                                    SpriteManager.Instance.GetEquipmentBackground((int)equipmentRarity));

                            SquadBattleManager.EquipAction(equipment);
                        }
                    }
                }
            }
        }

        // 장비 데이터를 만드는 메서드
        private void CreateAllEquipment()
        {
            foreach (var equipmentType in Enums.equipmentTypes)
            {
                var equipmentIndex = 0;

                foreach (var rarity in Enums.equipmentRarities)
                {
                    var rarityIntValue = Convert.ToInt32(rarity);

                    for (var equipmentTier = 5; equipmentTier >= MaxTier; equipmentTier--)
                    {
                        var equipmentId = $"{rarity}_{equipmentTier}_{equipmentType}";
                        var equipmentName = equipmentType switch
                        {
                            Enums.EquipmentType.Sword => $"{swordNames[rarityIntValue]} {6 - equipmentTier}",
                            Enums.EquipmentType.Bow => $"{bowNames[rarityIntValue]} {6 - equipmentTier}",
                            Enums.EquipmentType.Staff => $"{staffNames[rarityIntValue]} {6 - equipmentTier}",
                            Enums.EquipmentType.Helmet => $"{helmetNames[rarityIntValue]} {6 - equipmentTier}",
                            Enums.EquipmentType.Armor => $"{armorNames[rarityIntValue]} {6 - equipmentTier}",
                            Enums.EquipmentType.Gauntlet => $"{gauntletNames[rarityIntValue]} {6 - equipmentTier}",
                            _ => null
                        };
                        var equipmentLevel = 1;
                        var isEquipped = rarityIntValue == 0 && equipmentTier == 5;
                        var isPossessed = rarityIntValue == 0 && equipmentTier == 5;
                        var equipmentIconIndex = equipmentIndex;
                        var equipmentIcon = SpriteManager.Instance.GetEquipmentSprite(equipmentType, equipmentIndex);
                        var equipmentRarity = rarity;
                        var equipmentQuantity = isPossessed ? 1 : 0;
                        var equipmentBackground = SpriteManager.Instance.GetEquipmentBackground(rarityIntValue);
                        var equipmentBackgroundEffect =
                            SpriteManager.Instance.GetEquipmentBackgroundEffect(rarityIntValue);

                        var equippedEffects = new List<EquipmentEffect>();
                        var ownedEffects =  new List<EquipmentEffect>();

                        for (var i = 0 ; i < 2;i++)
                        {
                            var equippedEffect = new EquipmentEffect
                            {
                                statType = i == 0? Enums.StatTypeFromInventoryPanel.Attack : Enums.StatTypeFromInventoryPanel.Health,
                                increaseStatType = Enums.IncreaseStatValueType.BaseStat,
                                increaseValue = (6 - equipmentTier) * (int)Mathf.Pow(10, rarityIntValue + 1)
                            };
                            equippedEffects.Add(equippedEffect);
                        }
                        
                        for (var i = 0 ; i < 2;i++)
                        {
                            var ownedEffect = new EquipmentEffect
                            {
                                statType = i == 0 ? Enums.StatTypeFromInventoryPanel.Attack : Enums.StatTypeFromInventoryPanel.Health,
                                increaseStatType = Enums.IncreaseStatValueType.PercentStat,
                                increaseValue = (6 - equipmentTier) * (int)Mathf.Pow(10, rarityIntValue + 1)
                            };
                            ownedEffects.Add(ownedEffect);
                        }

                        var equipment = new Equipment(equipmentId, equipmentName, equipmentIconIndex, equipmentLevel,
                            equipmentType, equipmentRarity, equipmentTier, equipmentQuantity, isEquipped, isPossessed,
                            equippedEffects, ownedEffects);
                        
                        AddEquipment(equipmentId, equipment);

                        var targetScrollViewItem = equipmentType switch
                        {
                            Enums.EquipmentType.Sword => UIManager.Instance.inventoryPanelUI
                                .inventoryScrollViewItemSwords,
                            Enums.EquipmentType.Bow => UIManager.Instance.inventoryPanelUI.inventoryScrollViewItemBows,
                            Enums.EquipmentType.Staff => UIManager.Instance.inventoryPanelUI
                                .inventoryScrollViewItemStaffs,
                            Enums.EquipmentType.Helmet => UIManager.Instance.inventoryPanelUI
                                .inventoryScrollViewItemHelmets,
                            Enums.EquipmentType.Armor => UIManager.Instance.inventoryPanelUI
                                .inventoryScrollViewItemArmors,
                            Enums.EquipmentType.Gauntlet => UIManager.Instance.inventoryPanelUI
                                .inventoryScrollViewItemGauntlets,
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        targetScrollViewItem[equipmentIndex]
                            .GetComponent<InventoryPanelItemUI>().UpdateInventoryPanelItemUI(
                                1,
                                EquipmentMaxLevel,
                                equipmentQuantity,
                                MaxQuantity,
                                isEquipped,
                                isEquipped,
                                equipmentTier,
                                SpriteManager.Instance.GetEquipmentSprite(
                                    equipmentType,
                                    equipmentIconIndex),
                                SpriteManager.Instance.GetEquipmentBackgroundEffect((int)equipmentRarity),
                                SpriteManager.Instance.GetEquipmentBackground((int)equipmentRarity));
                        
                        targetScrollViewItem[equipmentIndex].GetComponent<Button>().onClick.AddListener(() => InventoryPanelUI.SelectEquipmentAction(equipment));

                        equipmentIndex++;

                        if (equipment.isEquipped)
                        {
                            UIManager.Instance.inventoryPanelUI.equipmentButton[(int)equipmentType]
                                .GetComponent<InventoryPanelSelectedItemUI>()
                                .UpdateInventoryPanelSelectedItem(equipmentTier, equipmentIcon,
                                    equipmentBackgroundEffect, equipmentBackground);

                            SquadBattleManager.EquipAction(equipment);
                        }

                        InfiniteLoopDetector.Run();
                    }
                }
            }
        }

        // 매개변수로 받은 장비 합성하는 메서드
        private void CompositeAllEquipment(Equipment equipment)
        {
            if (equipment.equipmentQuantity < 5) return;

            var compositeCount = equipment.equipmentQuantity / 5;
            equipment.equipmentQuantity %= 5;

            // equipment.SetQuantityText();
            equipment.SaveEquipmentEachInfo(equipment.equipmentId, Enums.EquipmentProperty.Quantity);

            var nextEquipment = GetNextEquipment(equipment.equipmentId, equipment.equipmentType);

            if (nextEquipment == null) return;
            nextEquipment.equipmentQuantity += compositeCount;
        }

        // AllEquipment에 Equipment 더하는 메서드
        private void AddEquipment(string equipmentId, Equipment equipment)
        {
            if (!AllEquipments.TryAdd(equipmentId, equipment))
                Debug.LogWarning($"Weapon already exists in the dictionary: {equipmentId}");

            switch (equipment.equipmentType)
            {
                case Enums.EquipmentType.Sword:
                    if (!SwordsDictionary.TryAdd(equipmentId, equipment))
                        Debug.LogWarning($"Weapon already exists in the dictionary: {equipmentId}");
                    break;
                case Enums.EquipmentType.Bow:
                    if (!BowsDictionary.TryAdd(equipmentId, equipment))
                        Debug.LogWarning($"Weapon already exists in the dictionary: {equipmentId}");
                    break;
                case Enums.EquipmentType.Staff:
                    if (!StaffsDictionary.TryAdd(equipmentId, equipment))
                        Debug.LogWarning($"Weapon already exists in the dictionary: {equipmentId}");
                    break;
                case Enums.EquipmentType.Helmet:
                    if (!HelmetsDictionary.TryAdd(equipmentId, equipment))
                        Debug.LogWarning($"Weapon already exists in the dictionary: {equipmentId}");
                    break;
                case Enums.EquipmentType.Armor:
                    if (!ArmorsDictionary.TryAdd(equipmentId, equipment))
                        Debug.LogWarning($"Weapon already exists in the dictionary: {equipmentId}");
                    break;
                case Enums.EquipmentType.Gauntlet:
                    if (!GauntletsDictionary.TryAdd(equipmentId, equipment))
                        Debug.LogWarning($"Weapon already exists in the dictionary: {equipmentId}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // AllEquipment에서 매개변수로 받은 string을 key로 사용해 Equipment 찾는 매서드
        public static Equipment GetEquipment(string equipmentId)
        {
            if (AllEquipments.TryGetValue(equipmentId, out var equipment)) return equipment;

            Debug.LogError($"Equipment not found: {equipmentId}");
            return null;
        }

        // AllEquipment에서 매개변수로 받은 key을 사용하는 Equipment 업데이트 하는 메서드
        public static void SetEquipment(string equipmentId, Equipment equipment)
        {
            var targetEquipment = AllEquipments[equipmentId];

            if (targetEquipment == null) return;
            Debug.Log("이름 : " + targetEquipment.equipmentName);

            targetEquipment.equippedEffects = equipment.equippedEffects;
            targetEquipment.ownedEffects = equipment.ownedEffects;
            targetEquipment.equipmentQuantity = equipment.equipmentQuantity;
            targetEquipment.isEquipped = equipment.isEquipped;
            targetEquipment.equipmentTier = equipment.equipmentTier;

            // targetEquipment.SetQuantityText();
            targetEquipment.SaveEquipmentAllInfo(targetEquipment.equipmentId);
        }

        // 매개변수로 받은 key값을 사용하는 장비의 다음레벨 장비를 불러오는 메서드
        private static Equipment GetNextEquipment(string currentKey, Enums.EquipmentType type)
        {
            var currentRarityIndex = -1;
            var currentLevel = -1;

            // 현재 키에서 희귀도와 레벨 분리
            foreach (var rarity in Enums.equipmentRarities)
            {
                if (!currentKey.StartsWith(rarity.ToString())) continue;

                currentRarityIndex = Array.IndexOf(Enums.equipmentRarities, rarity);
                int.TryParse(currentKey.Replace(rarity + "_", "")[0].ToString(), out currentLevel);
                break;
            }

            if (currentRarityIndex == -1 || currentLevel == -1) return null;

            var nextKey = string.Empty;

            if (currentLevel < MaxTier)
                // 같은 희귀도 내에서 다음 레벨 찾기
                nextKey = Enums.equipmentRarities[currentRarityIndex] + "_" + (currentLevel + 1) + "_" + $"{type}";
            else if (currentRarityIndex < Enums.equipmentRarities.Length - 1)
                // 희귀도를 증가시키고 첫 번째 레벨의 장비 찾기
                nextKey = Enums.equipmentRarities[currentRarityIndex + 1] + "_1" + "_" + $"{type}";

            return AllEquipments.GetValueOrDefault(nextKey);
        }

        // // 매개변수로 받은 key값을 사용하는 장비의 이전레벨 장비를 불러오는 메서드
        // public Equipment GetPreviousEquipment(string currentKey)
        // {
        //     var currentRarityIndex = -1;
        //     var currentLevel = -1;
        //     
        //     var currentEquipmentType = Enum.EquipmentType.Weapon;
        //
        //     if (currentKey.EndsWith($"{Enum.EquipmentType.Weapon}"))
        //     {
        //         currentEquipmentType = Enum.EquipmentType.Weapon;
        //     }
        //     else if (currentKey.EndsWith($"{Enum.EquipmentType.Armor}"))
        //     {
        //         currentEquipmentType = Enum.EquipmentType.Armor;
        //     }
        //
        //     // 현재 키에서 희귀도와 레벨 분리
        //     foreach (var rarity in Enum.rarities)
        //     {
        //         if (currentKey.StartsWith(rarity.ToString()))
        //         {
        //             currentRarityIndex = Array.IndexOf(Enum.rarities, rarity);
        //             int.TryParse(currentKey.Replace(rarity + "_", ""), out currentLevel);
        //             break;
        //         }
        //     }
        //
        //     if (currentRarityIndex != -1 && currentLevel != -1)
        //     {
        //         var previousKey = string.Empty;
        //         Equipment prevEquipment;
        //         
        //         if (currentLevel > 1)
        //         {
        //             // 같은 희귀도 내에서 이전 레벨 찾기
        //             previousKey = Enum.rarities[currentRarityIndex] + "_" + (currentLevel - 1);
        //         }
        //         else if (currentRarityIndex > 0)
        //         {
        //             // 희귀도를 낮추고 최대 레벨의 장비 찾기
        //             previousKey = Enum.rarities[currentRarityIndex - 1] + "_4";
        //         }
        //         
        //         switch (currentEquipmentType)
        //         {
        //             case Enum.EquipmentType.Sword:
        //                 return AllWeapon.TryGetValue(previousKey, out prevEquipment) ? prevEquipment : null;
        //             case Enum.EquipmentType.Armor:
        //                 return AllArmor.TryGetValue(previousKey, out prevEquipment) ? prevEquipment : null;
        //         }
        //     }
        //
        //     // 이전 장비를 찾을 수 없는 경우
        //     return null;
        // }

        public void AutoEquip(Enums.EquipmentType currentEquipmentType)
        {
            var equipments = currentEquipmentType switch
            {
                Enums.EquipmentType.Sword => SwordsDictionary,
                Enums.EquipmentType.Bow => BowsDictionary,
                Enums.EquipmentType.Staff => StaffsDictionary,
                Enums.EquipmentType.Helmet => HelmetsDictionary,
                Enums.EquipmentType.Armor => ArmorsDictionary,
                Enums.EquipmentType.Gauntlet => GauntletsDictionary,
                _ => null
            };

            if (equipments == null) return;

            var highValueEquipment = equipments.OrderByDescending(equipment => equipment.Value.isPossessed).ThenByDescending(equipment => equipment.Value.equipmentRarity).ThenBy(equipment => equipment.Value.equipmentTier).ToList()[0].Value;
            highValueEquipment.isEquipped = true;
            highValueEquipment.SaveEquipmentEachInfo(highValueEquipment.equipmentId, Enums.EquipmentProperty.IsEquipped);

            SquadBattleManager.EquipAction?.Invoke(highValueEquipment);
            UIManager.Instance.inventoryPanelUI.SelectEquipment(highValueEquipment);
        }

        public void AllComposite(Enums.EquipmentType currentEquipmentType)
        {
            var equipments = currentEquipmentType switch
            {
                Enums.EquipmentType.Sword => SwordsDictionary,
                Enums.EquipmentType.Bow => BowsDictionary,
                Enums.EquipmentType.Staff => StaffsDictionary,
                Enums.EquipmentType.Helmet => HelmetsDictionary,
                Enums.EquipmentType.Armor => ArmorsDictionary,
                Enums.EquipmentType.Gauntlet => GauntletsDictionary,
                _ => null
            };

            if (equipments == null) return;

            var index = 0;
            
            foreach (var equipment in equipments)
                if (index == equipments.Count - 1)
                {
                    // equipment.SetQuantityText();
                    equipment.Value.SaveEquipmentEachInfo(equipment.Value.equipmentId, Enums.EquipmentProperty.Quantity);
                }
                else
                {
                    CompositeAllEquipment(equipment.Value);
                    index++;
                }
        }

        public static void IncreaseQuantity(Equipment equipment, int increaseValue)
        {
            equipment.equipmentQuantity += increaseValue;

            // equipment.SetQuantityText();
            equipment.SaveEquipmentEachInfo(equipment.equipmentId, Enums.EquipmentProperty.Quantity);
        }
    }
}