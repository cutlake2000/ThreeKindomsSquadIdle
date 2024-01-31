using System;
using System.Collections.Generic;
using Controller.UI.BottomMenuUI;
using Creature.Data;
using Function;
using Managers.BottomMenuManager.InventoryPanel;
using ScriptableObjects.Scripts;
using UnityEngine;
using Enum = Data.Enum;
using Random = UnityEngine.Random;

namespace Managers.BottomMenuManager.SummonPanel
{
    public class SummonManager : MonoBehaviour
    {
        public static Action<Enum.SummonEquipmentType, int> OnSummonEquipment;

        public static SummonManager Instance;

        public SummonSo summonProbability;
        public List<Equipment> summonedEquipmentList = new();

        public readonly Dictionary<string, int> SummonedItemDictionary = new();
        private WeightedRandomPicker<string> armorSummoner;
        private Enum.EquipmentType[] gearType;
        private WeightedRandomPicker<string> squadSummoner;

        private Enum.CharacterType[] squadType;

        private WeightedRandomPicker<string> weaponSummoner;
        private Enum.EquipmentType[] weaponType;

        private void Awake()
        {
            Instance = this;
        }

        public void InitSummonManager()
        {
            InitializeEventListeners();
            InitAllSummoner();
        }

        private void InitializeEventListeners()
        {
            OnSummonEquipment += RandomSummon;
            OnSummonEquipment += IncreaseAchievementValue;
            SquadBattleManager.Instance.SummonLevel.OnLevelUpgrade += SetupSummoner;
        }

        private void InitAllSummoner()
        {
            squadSummoner = new WeightedRandomPicker<string>();
            weaponSummoner = new WeightedRandomPicker<string>();
            armorSummoner = new WeightedRandomPicker<string>();

            squadType = new[] { Enum.CharacterType.Warrior, Enum.CharacterType.Archer, Enum.CharacterType.Wizard };
            weaponType = new[] { Enum.EquipmentType.Sword, Enum.EquipmentType.Bow, Enum.EquipmentType.Staff };
            gearType = new[] { Enum.EquipmentType.Helmet, Enum.EquipmentType.Armor, Enum.EquipmentType.Gauntlet };

            InitWeaponSummoner();
            InitGearSummoner();
        }

        private void InitSquadSummoner()
        {
            var currentSquadLevel = SquadBattleManager.Instance.SummonLevel.CurrentSquadLevel;

                Enum.CharacterType cr = (Enum.CharacterType)System.Enum.Parse(typeof(Enum.CharacterType) , "Warrior");
            
            if (currentSquadLevel == 0) currentSquadLevel = 1;

            var squadProbability = summonProbability.GetProbability(currentSquadLevel).SummonProbabilities;

            foreach (var probability in squadProbability)
                squadSummoner.Add($"{probability.equipmentRarity}", probability.weight);
        }

        private void InitWeaponSummoner()
        {
            var currentWeaponLevel = SquadBattleManager.Instance.SummonLevel.CurrentWeaponLevel;

            if (currentWeaponLevel == 0) currentWeaponLevel = 1;

            var weaponProbability = summonProbability.GetProbability(currentWeaponLevel).SummonProbabilities;

            foreach (var probability in weaponProbability)
                weaponSummoner.Add($"{probability.equipmentRarity}", probability.weight);
        }

        private void InitGearSummoner()
        {
            var currentGearLevel = SquadBattleManager.Instance.SummonLevel.CurrentGearLevel;

            if (currentGearLevel == 0) currentGearLevel = 1;

            var armorProbability = summonProbability.GetProbability(currentGearLevel).SummonProbabilities;

            foreach (var probability in armorProbability)
                armorSummoner.Add($"{probability.equipmentRarity}", probability.weight);
        }

        private void SetupSummoner(Enum.SummonEquipmentType type)
        {
            switch (type)
            {
                case Enum.SummonEquipmentType.Squad:
                    SetSquadSummoner();
                    break;
                case Enum.SummonEquipmentType.Weapon:
                    SetWeaponSummoner();
                    break;
                case Enum.SummonEquipmentType.Gear:
                    SetGearSummoner();
                    break;
            }
        }

        private void RandomSummon(Enum.SummonEquipmentType type, int count)
        {
            for (var i = 0; i < count; i++)
            {
                string randomRarity = null;
                var randomTier = Random.Range(1, 5);
                var randomType = Enum.EquipmentType.Null;

                switch (type)
                {
                    case Enum.SummonEquipmentType.Squad:
                        randomRarity = squadSummoner?.GetRandomPick();
                        // randomType = squadType[Random.Range(0, 3)];
                        break;
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

                if (!CheckDictionaryKey(SummonedItemDictionary, targetName))
                    SummonedItemDictionary.Add(targetName, 1);
                else
                    SummonedItemDictionary[targetName]++;

                Debug.Log($"가챠! {targetName}");
            }

            const int maxGradeEnumIndex = (int)Enum.EquipmentRarity.Null;
            var typeEnumIndex = 0;

            for (var i = 0; i < maxGradeEnumIndex; i++)
            for (var j = 5; j >= 1; j--)
            {
                switch (type)
                {
                    case Enum.SummonEquipmentType.Weapon:
                        typeEnumIndex = (int)Enum.EquipmentType.Sword;
                        break;
                    case Enum.SummonEquipmentType.Gear:
                        typeEnumIndex = (int)Enum.EquipmentType.Helmet;
                        break;
                }

                for (var k = 0; k < 3; k++)
                {
                    var targetId = $"{(Enum.EquipmentRarity)i}_{j}_{(Enum.EquipmentType)typeEnumIndex + k}";

                    if (!SummonedItemDictionary.TryGetValue(targetId, out var summonCount)) continue;

                    Debug.Log($"가챠2 {targetId}");
                    var target = InventoryManager.GetEquipment(targetId);
                    target.equipmentRarity = (Enum.EquipmentRarity)i;
                    target.summonCount = summonCount;
                    target.quantity += summonCount;
                    target.SaveEquipmentEachInfo(target.name, Enum.EquipmentProperty.Quantity);
                    target.SetQuantityText();
                    summonedEquipmentList.Add(target);
                }
            }

            SummonPanelUI.OnSummon?.Invoke(type, SummonedItemDictionary.Count);
        }

        private void SetWeaponSummoner()
        {
            var currentWeaponLevel = SquadBattleManager.Instance.SummonLevel.CurrentWeaponLevel;
            var weaponProbability = summonProbability.GetProbability(currentWeaponLevel).SummonProbabilities;

            foreach (var probability in weaponProbability)
                weaponSummoner.ModifyWeight($"{probability.equipmentRarity}", probability.weight);
        }

        private void SetGearSummoner()
        {
            var currentGearLevel = SquadBattleManager.Instance.SummonLevel.CurrentWeaponLevel;
            var armorProbability = summonProbability.GetProbability(currentGearLevel).SummonProbabilities;

            foreach (var probability in armorProbability)
                armorSummoner.ModifyWeight($"{probability.equipmentRarity}", probability.weight);
        }

        private void SetSquadSummoner()
        {
            var currentSquadLevel = SquadBattleManager.Instance.SummonLevel.CurrentSquadLevel;
            var squadProbability = summonProbability.GetProbability(currentSquadLevel).SummonProbabilities;

            foreach (var probability in squadProbability)
                squadSummoner.ModifyWeight($"{probability.equipmentRarity}", probability.weight);
        }

        //TODO : 업적
        private void IncreaseAchievementValue(Enum.SummonEquipmentType type, int count)
        {
            // AchievementManager.instance.IncreaseAchievementValue(Data.Enum.AchieveType.Summon, count);
        }

        private static bool CheckDictionaryKey<TK, TV>(Dictionary<TK, TV> dict, TK key)
        {
            foreach (var kvp in dict)
                if (kvp.Key.Equals(key))
                    return true;

            return false;
        }
    }
}