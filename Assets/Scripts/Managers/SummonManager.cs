using System;
using System.Collections.Generic;
using System.Linq;
using Controller.UI;
using Controller.UI.BottomMenuUI;
using Creature.Data;
using Function;
using ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using Enum = Data.Enum;
using Random = UnityEngine.Random;

namespace Managers
{
    public class SummonManager : MonoBehaviour
    {
        public static Action<Enum.SummonEquipmentType, int> OnSummonEquipment;

        public static SummonManager Instance;

        private WeightedRandomPicker<string> weaponSummoner;
        private WeightedRandomPicker<string> armorSummoner;

        private Enum.EquipmentType[] weaponType;
        private Enum.EquipmentType[] gearType;

        public SummonSo summonProbability;

        public Dictionary<string, int> SummonedEquipmentDictionary = new();
        public List<Equipment> summonedEquipmentList = new();

        private void Awake()
        {
            Instance = this;
        }

        public void InitSummonManager()
        {
            SetupEventListeners();
            InitAllSummoner();
        }

        private void SetupEventListeners()
        {
            OnSummonEquipment += SummonRandomEquipment;
            OnSummonEquipment += IncreaseAchievementValue;
            SquadManager.Instance.SummonLevel.OnLevelUpgrade += SetupSummoner;
        }

        private void InitAllSummoner()
        {
            weaponSummoner = new WeightedRandomPicker<string>();
            armorSummoner = new WeightedRandomPicker<string>();

            weaponType = new[] { Enum.EquipmentType.Sword, Enum.EquipmentType.Bow, Enum.EquipmentType.Staff };
            gearType = new[] { Enum.EquipmentType.Helmet, Enum.EquipmentType.Armor, Enum.EquipmentType.Gauntlet };

            InitWeaponSummoner();
            InitGearSummoner();
        }

        private void InitWeaponSummoner()
        {
            var currentWeaponLevel = SquadManager.Instance.SummonLevel.CurrentWeaponLevel;
            
            if (currentWeaponLevel == 0) currentWeaponLevel = 1;
            
            var weaponProbability = summonProbability.GetProbability(currentWeaponLevel).SummonProbabilities;
            
            foreach (var probability in weaponProbability)
            {
                weaponSummoner.Add($"{probability.equipmentRarity}", probability.weight);
            }
        }

        private void InitGearSummoner()
        {
            var currentGearLevel = SquadManager.Instance.SummonLevel.CurrentGearLevel;
            
            if (currentGearLevel == 0) currentGearLevel = 1;
            
            var armorProbability = summonProbability.GetProbability(currentGearLevel).SummonProbabilities;
            
            foreach (var probability in armorProbability)
            {
                armorSummoner.Add($"{probability.equipmentRarity}", probability.weight);
            }
        }

        private void SetupSummoner(Enum.SummonEquipmentType type)
        {
            switch (type)
            {
                case Enum.SummonEquipmentType.Weapon:
                    SetWeaponSummoner();
                    break;
                case Enum.SummonEquipmentType.Gear:
                    SetGearSummoner();
                    break;
            }
        }

        private void SummonRandomEquipment(Enum.SummonEquipmentType type, int count)
        {
            for (var i = 0; i < count; i++)
            {
                string randomRarity = null;
                var randomTier = Random.Range(1, 5);
                var randomType = Enum.EquipmentType.Null;
            
                switch (type)
                {
                    case Enum.SummonEquipmentType.Weapon:
                        randomRarity = weaponSummoner?.GetRandomPick();
                        randomType = weaponType[Random.Range(0, 3)];
                        break;
                    case Enum.SummonEquipmentType.Gear:
                        randomRarity = armorSummoner?.GetRandomPick();
                        randomType = gearType[Random.Range(0, 3)];
                        break;
                }
                
                var targetName = $"{randomRarity}_{randomTier}_{randomType}";

                if (!CheckDictionaryKey(SummonedEquipmentDictionary, targetName))
                {
                    SummonedEquipmentDictionary.Add(targetName, 1);
                }
                else
                {
                    SummonedEquipmentDictionary[targetName]++;
                }
                
                Debug.Log($"가챠! {targetName}");
            }

            const int maxGradeEnumIndex = (int) Enum.EquipmentRarity.Null;
            var typeEnumIndex = 0;

            for (var i = 0; i < maxGradeEnumIndex; i++)
            {
                for (var j = 5; j >= 1; j--)
                {
                    switch (type)
                    {
                        case Enum.SummonEquipmentType.Weapon:
                            typeEnumIndex = (int) Enum.EquipmentType.Sword;
                            break;
                        case Enum.SummonEquipmentType.Gear:
                            typeEnumIndex = (int) Enum.EquipmentType.Helmet;
                            break;
                    }

                    for (var k = 0; k < 3; k++)
                    {
                        var targetId = $"{(Enum.EquipmentRarity) i}_{j}_{(Enum.EquipmentType) typeEnumIndex + k}";
                    
                        if (!SummonedEquipmentDictionary.TryGetValue(targetId, out var summonCount)) continue;
                    
                        Debug.Log($"가챠2 {targetId}");
                        var target = EquipmentManager.GetEquipment(targetId);
                        target.equipmentRarity = (Enum.EquipmentRarity)i;
                        target.summonCount = summonCount;
                        target.quantity += summonCount;
                        target.SaveEquipmentEachInfo(target.name, Enum.EquipmentProperty.Quantity);
                        summonedEquipmentList.Add(target);
                    }
                }
            }
            
            SummonUI.OnSummon?.Invoke(type, SummonedEquipmentDictionary.Count);
        }

        private void SetWeaponSummoner()
        {
            var currentWeaponLevel = SquadManager.Instance.SummonLevel.CurrentWeaponLevel;
            var weaponProbability = summonProbability.GetProbability(currentWeaponLevel).SummonProbabilities;
            
            foreach (var probability in weaponProbability)
            {
                weaponSummoner.ModifyWeight($"{probability.equipmentRarity}", probability.weight);
            }
        }

        private void SetGearSummoner()
        {
            var currentGearLevel = SquadManager.Instance.SummonLevel.CurrentWeaponLevel;
            var armorProbability = summonProbability.GetProbability(currentGearLevel).SummonProbabilities;
            
            foreach (var probability in armorProbability)
            {
                armorSummoner.ModifyWeight($"{probability.equipmentRarity}", probability.weight);
            }
        }

        //TODO : 업적
        private void IncreaseAchievementValue(Enum.SummonEquipmentType type, int count)
        {
            // AchievementManager.instance.IncreaseAchievementValue(Data.Enum.AchieveType.Summon, count);
        }
        
        private static bool CheckDictionaryKey<TK, TV>(Dictionary<TK, TV> dict, TK key)
        {
            foreach (var kvp in dict)
            {
                if (kvp.Key.Equals(key)) return true;
            }

            return false;
        }
    }
}