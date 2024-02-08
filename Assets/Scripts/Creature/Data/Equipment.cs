using System;
using System.Collections.Generic;
using Data;
using Function;
using Managers.BottomMenuManager.InventoryPanel;
using Managers.GameManager;
using UnityEngine;

namespace Creature.Data
{
    [Serializable]
    public class EquipmentEffect
    {
        public Enums.SquadStatType statType;
        public Enums.IncreaseStatValueType increaseStatType;
        public int increaseValue;
    }
    
    [Serializable]
    public class Equipment
    {
        [Header("ES3 ID")] public string equipmentId;
        [Header("이름")] public string equipmentName;
        [Header("스프라이트 인덱스")] public int equipmentIconIndex;
        [Header("레벨")] public int equipmentLevel;
        [Header("요구 강화석")] public BigInteger equipmentRequiredCurrency;
        [Header("장비 타입")] public Enums.EquipmentType equipmentType;
        [Header("장비 등급")] public Enums.EquipmentRarity equipmentRarity;
        [Header("장비 티어")] public int equipmentTier;
        [Header("보유 수량")] public int equipmentQuantity; // 장비의 개수
        [Header("장착 여부")] public bool isEquipped;
        [Header("보유 여부")] public bool isPossessed;
        [Space(5)]
        [Header("장착 효과 타입")] public List<EquipmentEffect> equippedEffects;
        [Header("보유 효과 타입")] public List<EquipmentEffect> ownedEffects;

        [Header("ES3 Loader")] public EquipmentES3Loader equipmentES3Loader;
        
        /// <summary>
        /// 처음 생성할 때 사용되는 생성자
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="iconIndex"></param>
        /// <param name="level"></param>
        /// <param name="type"></param>
        /// <param name="rarity"></param>
        /// <param name="tier"></param>
        /// <param name="quantity"></param>
        /// <param name="isEquipped"></param>
        /// <param name="isPossessed"></param>
        /// <param name="equippedEffects"></param>
        /// <param name="ownedEffects"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Equipment(string id, string name, int iconIndex, int level, Enums.EquipmentType type,
            Enums.EquipmentRarity rarity, int tier, int quantity, bool isEquipped, bool isPossessed,
            List<EquipmentEffect> equippedEffects, List<EquipmentEffect> ownedEffects)
        {
            equipmentId = id;
            equipmentName = name;
            equipmentIconIndex = iconIndex;
            equipmentLevel = level;
            equipmentType = type;
            equipmentRarity = rarity;
            equipmentTier = tier;
            equipmentQuantity = quantity;
            this.isEquipped = isEquipped;
            this.isPossessed = isPossessed;

            this.equippedEffects = equippedEffects;
            this.ownedEffects = ownedEffects;

            equipmentRequiredCurrency = rarity switch
            {
                Enums.EquipmentRarity.Common => 10,
                Enums.EquipmentRarity.Uncommon => 20,
                Enums.EquipmentRarity.Magic => 80,
                Enums.EquipmentRarity.Rare => 160,
                Enums.EquipmentRarity.Unique => 320,
                // Enums.EquipmentRarity.Legend => 640,
                // Enums.EquipmentRarity.Epic => 1280,
                // Enums.EquipmentRarity.Ancient => 2560,
                // Enums.EquipmentRarity.Legendary => 5120,
                // Enums.EquipmentRarity.Mythology => 10240,
                // Enums.EquipmentRarity.Null => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
            
            SaveEquipmentDataIntoES3Loader();
        }

        /// <summary>
        /// 로드할 때 사용되는 생성자
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="iconIndex"></param>
        /// <param name="icon"></param>
        /// <param name="rarity"></param>
        public Equipment(string id, string name, int iconIndex, Sprite icon, Enums.EquipmentType type, Enums.EquipmentRarity rarity, int tier,
            List<EquipmentEffect> equippedEffects, List<EquipmentEffect> ownedEffects)
        {
            equipmentId = id;
            equipmentName = name;
            equipmentIconIndex = iconIndex;
            equipmentType = type;
            equipmentRarity = rarity;
            equipmentTier = tier;
            this.equippedEffects = equippedEffects;
            this.ownedEffects = ownedEffects;

            LoadEquipmentDataFromES3Loader();
        }
        
        // 강화 메서드
        public void Enhance()
        {
            // 강화 로직...
            foreach (var t in equippedEffects)
            {
                t.increaseValue += t.increaseValue;
            }
            
            foreach (var t in ownedEffects)
            {
                t.increaseValue += t.increaseValue;
            }

            equipmentLevel = Mathf.Min(equipmentLevel++, InventoryManager.EquipmentMaxLevel);
        }

        // 강화할 때 필요한 강화석 return 시키는 메서드
        public BigInteger GetEnhanceStone()
        {
            // var requiredEnhanceStone = equippedEffect - basicOwnedEffect;

            return 100;
        }

        // 장비 데이터를 ES3 파일에 저장
        public void SaveEquipmentDataIntoES3Loader()
        {
            var targetIndex = InventoryManager.FindEquipmentIndex(equipmentId);
            InventoryManager.Instance.equipmentES3Loaders[targetIndex].SaveData(equipmentId, equipmentLevel, equipmentQuantity, isEquipped, isPossessed);
        }

        public void LoadEquipmentDataFromES3Loader()
        {
            var targetIndex = InventoryManager.FindEquipmentIndex(equipmentId);

            equipmentId = InventoryManager.Instance.equipmentES3Loaders[targetIndex].id;
            equipmentLevel = InventoryManager.Instance.equipmentES3Loaders[targetIndex].level;
            equipmentQuantity = InventoryManager.Instance.equipmentES3Loaders[targetIndex].quantity;
            isEquipped = InventoryManager.Instance.equipmentES3Loaders[targetIndex].isEquipped;
            isPossessed = InventoryManager.Instance.equipmentES3Loaders[targetIndex].isPossessed;
        }
        
        
        // public void SaveEquipmentAllInfo(string id)
        // {
        //     ES3.Save($"{nameof(equipmentId)}_" + id, equipmentId);
        //     ES3.Save($"{nameof(equipmentLevel)}_" + id, equipmentLevel);
        //     ES3.Save($"{nameof(equipmentQuantity)}_" + id, equipmentQuantity);
        //     ES3.Save($"{nameof(isEquipped)}_" + id, isEquipped);
        //     ES3.Save($"{nameof(isPossessed)}_" + id, isPossessed);
        //
        //     for (var i = 0; i < equippedEffects.Count; i++)
        //     {
        //         ES3.Save($"equipmentEquippedEffects[{i}]).increaseValue_" + equipmentId, equippedEffects[i].increaseValue);   
        //     }
        //     
        //     for (var i = 0; i < ownedEffects.Count; i++)
        //     {
        //         ES3.Save($"equipmentOwnedEffects[{i}]).increaseValue_" + equipmentId, ownedEffects[i].increaseValue);   
        //     }
        // }
        
        // public void SaveEquipmentEachInfo(string equipmentID, Enums.EquipmentProperty property)
        // {
        //     switch (property)
        //     {
        //         case Enums.EquipmentProperty.Quantity:
        //             ES3.Save($"{nameof(equipmentQuantity)}_" + equipmentID, equipmentQuantity);
        //             break;
        //         case Enums.EquipmentProperty.Level:
        //             ES3.Save($"{nameof(equipmentTier)}_" + equipmentID, equipmentTier);
        //             break;
        //         case Enums.EquipmentProperty.IsEquipped:
        //             ES3.Save($"{nameof(isEquipped)}_" + equipmentID, isEquipped);
        //             break;
        //         case Enums.EquipmentProperty.IsPossessed:
        //             ES3.Save($"{nameof(isPossessed)}_" + equipmentID, isPossessed);
        //             break;
        //     }
        // }
        //
        // // 장비 데이터를 ES3 파일에서 불러오기
        // public void LoadEquipmentAllInfo()
        // {
        //     if (!ES3.KeyExists($"{nameof(equipmentId)}_" + equipmentId)) return;
        //     
        //     equipmentLevel = ES3.Load<int>($"{nameof(equipmentLevel)}_" +equipmentId);
        //     equipmentQuantity = ES3.Load<int>($"{nameof(equipmentQuantity)}_" +equipmentId);
        //     isEquipped = ES3.Load<bool>($"{nameof(isEquipped)}_" + equipmentId);
        //     isPossessed = ES3.Load<bool>($"{nameof(isPossessed)}_" + equipmentId);
        //
        //     for (var i = 0; i < equippedEffects.Count; i++)
        //     {
        //         equippedEffects[i].increaseValue = ES3.Load<int>($"equipmentEquippedEffects[{i}]).increaseValue_" + equipmentId);   
        //     }
        //     
        //     for (var i = 0; i < ownedEffects.Count; i++)
        //     {
        //         ownedEffects[i].increaseValue = ES3.Load<int>($"equipmentOwnedEffects[{i}]).increaseValue_" + equipmentId);
        //     }
        // }
        

        // 매개변수로 받은 WeaponInfo 의 정보 복사
        public void SetEquipmentInfo(Equipment equipment)
        {
            equipmentId = equipment.equipmentId;
            equipmentQuantity = equipment.equipmentQuantity;
            equipmentTier = equipment.equipmentTier;
            isEquipped = equipment.isEquipped;
            equipmentType = equipment.equipmentType;
            equipmentRarity = equipment.equipmentRarity;
        }
    }
}