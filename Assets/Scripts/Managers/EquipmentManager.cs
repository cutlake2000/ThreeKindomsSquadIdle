using System;
using System.Collections.Generic;
using System.Linq;
using Controller.UI;
using Controller.UI.BottomMenuUI;
using Creature.Data;
using Module;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Managers
{
    public class EquipmentManager : MonoBehaviour
    {
        public static EquipmentManager Instance;

        [Header("장비 리스트")]
        public List<Equipment> swords = new();
        public List<Equipment> bows = new();
        public List<Equipment> staffs = new();
        public List<Equipment> helmets = new();
        public List<Equipment> armors = new();
        public List<Equipment> gauntlets = new();
        
        [Header("장비 스프라이트")]
        public List<Sprite> swordSprites = new();
        public List<Sprite> bowSprites = new();
        public List<Sprite> staffSprites = new();
        public List<Sprite> helmetSprites = new();
        public List<Sprite> armorSprites = new();
        public List<Sprite> gauntletSprites = new();
        public Sprite[] backgroundEffects;
        
        public static Dictionary<string, Equipment> AllEquipments = new();

        // private static readonly Dictionary<string, Equipment> AllSwords = new();
        // private static readonly Dictionary<string, Equipment> AllBows = new();
        // private static readonly Dictionary<string, Equipment> AllStaffs = new();
        // private static readonly Dictionary<string, Equipment> AllHelmets = new();
        // private static readonly Dictionary<string, Equipment> AllArmors = new();
        // private static readonly Dictionary<string, Equipment> AllGauntlets = new();
        
        private const int MaxLevel = 1;

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
            {
                LoadAllEquipment();
            }
            else
            {
                CreateAllEquipment();
            }
        }

        // 로컬에 저장되어 있는 장비 데이터들 불러오는 메서드
        private void LoadAllEquipment()
        {
            foreach (var equipmentType in Enum.equipmentTypes)
            {
                var equipmentCount = 0;
                
                foreach (var rarity in Enum.equipmentRarities)
                {
                    var rarityIntValue = Convert.ToInt32(rarity);
                    
                    for (var tier = 5; tier >= MaxLevel; tier--)
                    {
                        var equipmentId = $"{rarity}_{tier}_{equipmentType}";

                        var equipment = equipmentType switch
                        {
                            Enum.EquipmentType.Sword => swords[equipmentCount],
                            Enum.EquipmentType.Bow => bows[equipmentCount],
                            Enum.EquipmentType.Staff => staffs[equipmentCount],
                            Enum.EquipmentType.Helmet => helmets[equipmentCount],
                            Enum.EquipmentType.Armor => armors[equipmentCount],
                            Enum.EquipmentType.Gauntlet => gauntlets[equipmentCount],
                            _ => null
                        };

                        if (equipment == null) continue;
                        
                        equipment.equipmentImage = equipmentType switch
                        {
                            Enum.EquipmentType.Sword => swordSprites[equipmentCount],
                            Enum.EquipmentType.Bow => bowSprites[equipmentCount],
                            Enum.EquipmentType.Staff => staffSprites[equipmentCount],
                            Enum.EquipmentType.Helmet => helmetSprites[equipmentCount],
                            Enum.EquipmentType.Armor => armorSprites[equipmentCount],
                            Enum.EquipmentType.Gauntlet => gauntletSprites[equipmentCount],
                            _ => null
                        };

                        equipment.equipmentBackground = backgroundEffects[(int)rarity];
                        equipment.LoadEquipment(equipmentId);
                        equipment.GetComponent<Button>().onClick
                            .AddListener(() => InventoryUI.SelectEquipmentAction(equipment));

                        AddEquipment(equipmentId, equipment);

                        if (equipment.isEquipped)
                        {
                            SquadManager.EquipAction(equipment);

                            InventoryUI.Instance.equipmentButton[(int)equipmentType].GetComponent<Equipment>().SetEquipmentInfo(equipment);
                            InventoryUI.Instance.equipmentButton[(int)equipmentType].GetComponent<Equipment>().SetUI();
                        }

                        equipmentCount++;

                        equipment.equipmentBackground = backgroundEffects[rarityIntValue];
                        equipment.SetUI();
                        
                        InfiniteLoopDetector.Run();
                    }
                }
            }
        }

        // 장비 데이터를 만드는 메서드
        private void CreateAllEquipment()
        {
            foreach (var equipmentType in Enum.equipmentTypes)
            {
                var equipmentCount = 0;
                
                foreach (var rarity in Enum.equipmentRarities)
                {
                    var rarityIntValue = Convert.ToInt32(rarity);

                    for (var tier = 5; tier >= MaxLevel; tier--)
                    {
                        int initQuantity;
                        bool isEquipped;
                        
                        var equipment = equipmentType switch
                        {
                            Enum.EquipmentType.Sword => swords[equipmentCount],
                            Enum.EquipmentType.Bow => bows[equipmentCount],
                            Enum.EquipmentType.Staff => staffs[equipmentCount],
                            Enum.EquipmentType.Helmet => helmets[equipmentCount],
                            Enum.EquipmentType.Armor => armors[equipmentCount],
                            Enum.EquipmentType.Gauntlet => gauntlets[equipmentCount],
                            _ => null
                        };
                        
                        var equipmentSprite = equipmentType switch
                        {
                            Enum.EquipmentType.Sword => swordSprites[equipmentCount],
                            Enum.EquipmentType.Bow => bowSprites[equipmentCount],
                            Enum.EquipmentType.Staff => staffSprites[equipmentCount],
                            Enum.EquipmentType.Helmet => helmetSprites[equipmentCount],
                            Enum.EquipmentType.Armor => armorSprites[equipmentCount],
                            Enum.EquipmentType.Gauntlet => gauntletSprites[equipmentCount],
                            _ => null
                        };
                        
                        if (tier == 5 && rarity == Enum.EquipmentRarity.Common)
                        {
                            initQuantity = 1;
                            isEquipped = true;
                        }
                        else
                        {
                            initQuantity = 0;
                            isEquipped = false;
                        }

                        if (equipment == null) continue;
                        
                        var equipmentId = $"{rarity}_{tier}_{equipmentType}";
                        var equippedEffect = tier * ((int)Mathf.Pow(10, rarityIntValue + 1));
                        var ownedEffect = (int)(equippedEffect * 0.5f);

                        equipment.SetEquipmentInfo(equipmentId, initQuantity, tier, isEquipped, equipmentType, rarity, 1, equippedEffect, ownedEffect, backgroundEffects[rarityIntValue], equipmentSprite);
                        equipment.GetComponent<Button>().onClick.AddListener(() => InventoryUI.SelectEquipmentAction(equipment));

                        AddEquipment(equipmentId, equipment);
                        equipment.SaveEquipmentAllInfo(equipmentId);
                        
                        if (isEquipped)
                        {
                            SquadManager.EquipAction(equipment);
                            InventoryUI.Instance.equipmentButton[(int)equipmentType].GetComponent<Equipment>().SetEquipmentInfo(equipment);
                            InventoryUI.Instance.equipmentButton[(int)equipmentType].GetComponent<Equipment>().SetUI();
                        }

                        equipmentCount++;
                        
                        InfiniteLoopDetector.Run();
                    }
                }   
            }
        }
        
        // 매개변수로 받은 장비 합성하는 메서드
        private void CompositeAllEquipment(Equipment equipment)
        {
            if (equipment.quantity < 5) return;

            var compositeCount = equipment.quantity / 5;
            equipment.quantity %= 5;

            equipment.SetQuantityText();
            equipment.SaveEquipmentEachInfo(equipment.id, Enum.EquipmentProperty.Quantity);
            
            var nextEquipment = GetNextEquipment(equipment.id, equipment.type);

            if (nextEquipment == null) return;
            nextEquipment.quantity += compositeCount;
        }

        // AllEquipment에 Equipment 더하는 메서드
        private static void AddEquipment(string equipmentId, Equipment equipment)
        {
            if (!AllEquipments.TryAdd(equipmentId, equipment))
            {
                Debug.LogWarning($"Weapon already exists in the dictionary: {equipmentId}");
            }
        }

        // AllEquipment에서 매개변수로 받은 string을 key로 사용해 Equipment 찾는 매서드
        public static Equipment GetEquipment(string equipmentId)
        {
            if (AllEquipments.TryGetValue(equipmentId, out var equipment))
            {
                return equipment;
            }

            Debug.LogError($"Equipment not found: {equipmentId}");
            return null;
        }

        // AllEquipment에서 매개변수로 받은 key을 사용하는 Equipment 업데이트 하는 메서드
        public static void SetEquipment(string equipmentId, Equipment equipment)
        {
            var targetEquipment = AllEquipments[equipmentId];

            if (targetEquipment == null) return;
            Debug.Log("이름 : " + targetEquipment.gameObject.name);

            targetEquipment.equippedEffect = equipment.equippedEffect;
            targetEquipment.ownedEffect = equipment.ownedEffect;
            targetEquipment.quantity = equipment.quantity;
            targetEquipment.isEquipped = equipment.isEquipped;
            targetEquipment.tier = equipment.tier;

            targetEquipment.SetQuantityText();
            targetEquipment.SaveEquipmentAllInfo(targetEquipment.id);
        }

        // 매개변수로 받은 key값을 사용하는 장비의 다음레벨 장비를 불러오는 메서드
        private static Equipment GetNextEquipment(string currentKey, Enum.EquipmentType type)
        {
            var currentRarityIndex = -1;
            var currentLevel = -1;

            // 현재 키에서 희귀도와 레벨 분리
            foreach (var rarity in Enum.equipmentRarities)
            {
                if (!currentKey.StartsWith(rarity.ToString())) continue;
                
                currentRarityIndex = Array.IndexOf(Enum.equipmentRarities, rarity);
                int.TryParse(currentKey.Replace(rarity + "_", "")[0].ToString(), out currentLevel);
                break;
            }

            if (currentRarityIndex == -1 || currentLevel == -1) return null;
            
            var nextKey = string.Empty;

            if (currentLevel < MaxLevel)
            {
                // 같은 희귀도 내에서 다음 레벨 찾기
                nextKey = Enum.equipmentRarities[currentRarityIndex] + "_" + (currentLevel + 1) + "_" + $"{type}";
            }
            else if (currentRarityIndex < Enum.equipmentRarities.Length - 1)
            {
                // 희귀도를 증가시키고 첫 번째 레벨의 장비 찾기
                nextKey = Enum.equipmentRarities[currentRarityIndex + 1] + "_1" + "_" + $"{type}";
            }
                
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

        public void AutoEquip(Enum.EquipmentType currentEquipmentType)
        {
            Equipment highValueEquipment = null;

            var i = 0;

            var equipments = currentEquipmentType switch
            {
                Enum.EquipmentType.Sword => swords,
                Enum.EquipmentType.Bow => bows,
                Enum.EquipmentType.Staff => staffs,
                Enum.EquipmentType.Helmet => helmets,
                Enum.EquipmentType.Armor => armors,
                Enum.EquipmentType.Gauntlet => gauntlets,
                _ => null
            };

            if (equipments != null)
            {
                foreach (var equipment in equipments.Where(equipment => equipment.quantity != 0))
                {
                    if (i == 0) highValueEquipment = equipment;
                    else if (highValueEquipment != null && highValueEquipment.equippedEffect < equipment.equippedEffect)
                    {
                        highValueEquipment = equipment;
                    }

                    i++;
                }
            }

            SquadManager.EquipAction?.Invoke(highValueEquipment);
            InventoryUI.Instance.SelectEquipment(highValueEquipment);
        }

        public void AllComposite(Enum.EquipmentType currentEquipmentType)
        {
            var equipments = currentEquipmentType switch
            {
                Enum.EquipmentType.Sword => swords,
                Enum.EquipmentType.Bow => bows,
                Enum.EquipmentType.Staff => staffs,
                Enum.EquipmentType.Helmet => helmets,
                Enum.EquipmentType.Armor => armors,
                Enum.EquipmentType.Gauntlet => gauntlets,
                _ => null
            };

            if (equipments == null) return;

            var index = 0;
            foreach (var equipment in equipments)
            {
                if (index == equipments.Count - 1)
                {
                    equipment.SetQuantityText();
                    equipment.SaveEquipmentEachInfo(equipment.id, Enum.EquipmentProperty.Quantity);
                }
                else{
                    CompositeAllEquipment(equipment);
                    index++;
                }
            }
        }

        public static void IncreaseQuantity(Equipment equipment, int increaseValue)
        {
            equipment.quantity += increaseValue;
            
            equipment.SetQuantityText();
            equipment.SaveEquipmentEachInfo(equipment.id, Enum.EquipmentProperty.Quantity);
        }
    }
}