using System;
using Controller.UI;
using Controller.UI.BottomMenuUI;
using UnityEngine;
using Enum = Data.Enum;

namespace Creature.Data
{
    public class SummonLevel
    {
        public Action<Enum.SummonEquipmentType> OnLevelUpgrade;
        public Action<Enum.SummonEquipmentType, int> OnExpUpgrade;

        [Header("[Summon Level]")]
        [Header("[초기 레벨, 초기 경험치, 레벨 당 경험치 증가율]")]
        private const int LevelFirstValue = 1;
        private const int MaxLevelFirstValue = 30;
        private const int ExpFirstValue = 0;
        private const int MaxExpFirstValue = 1000;
        private const int MaxExpIncreaseValue = 200;
        
        public int CurrentSquadLevel { get; private set; }
        public float CurrentSquadExp { get; private set; }
        public int MaxSquadLevel { get; private set; }
        public float MaxSquadExp { get; private set; }

        public int CurrentWeaponLevel { get; private set; }
        public float CurrentWeaponExp { get; private set; }
        public int MaxWeaponLevel { get; private set; }
        public float MaxWeaponExp { get; private set; }
    
        public int CurrentGearLevel { get; private set; }
        public float CurrentGearExp { get; private set; }
        public int MaxGearLevel { get; private set; }
        public float MaxGearExp { get; private set; }

        public void InitializeData()
        {
            CurrentSquadLevel = ES3.Load($"{Enum.LevelType.CurrentLv}{Enum.SummonEquipmentType.Squad}CurrentLevel", LevelFirstValue);
            CurrentSquadExp = ES3.Load<float>($"{Enum.LevelType.CurrentExp}{Enum.SummonEquipmentType.Squad}CurrentExp", ExpFirstValue);
            
            CurrentWeaponLevel = ES3.Load($"{Enum.LevelType.CurrentLv}{Enum.SummonEquipmentType.Weapon}CurrentLevel", LevelFirstValue);
            CurrentWeaponExp = ES3.Load<float>($"{Enum.LevelType.CurrentExp}{Enum.SummonEquipmentType.Weapon}CurrentExp", ExpFirstValue);
            
            CurrentGearLevel = ES3.Load($"{Enum.LevelType.CurrentLv}{Enum.SummonEquipmentType.Gear}CurrentLevel", LevelFirstValue);
            CurrentGearExp = ES3.Load<float>($"{Enum.LevelType.CurrentExp}{Enum.SummonEquipmentType.Gear}CurrentExp", ExpFirstValue);

            MaxSquadExp = MaxExpFirstValue;
            MaxWeaponExp = MaxExpFirstValue;
            MaxGearExp = MaxExpFirstValue;
        
            SetMaxExp(Enum.SummonEquipmentType.Squad);
            SetMaxExp(Enum.SummonEquipmentType.Weapon);
            SetMaxExp(Enum.SummonEquipmentType.Gear);
        
            SummonPanelUI.Instance.SetSummonUI();
        }
    
        public void SetupEventListeners()
        {
            OnExpUpgrade += UpdateCurrentExp;
            OnLevelUpgrade += UpdateLevel;
        }
    
        public void IncreaseExp(Enum.SummonEquipmentType type, int currentExpIncreaseValue)
        {
            OnExpUpgrade?.Invoke(type, currentExpIncreaseValue);
        }

        private void UpdateCurrentExp(Enum.SummonEquipmentType type, int currentExpIncreaseValue)
        {
            switch (type)
            {
                case Enum.SummonEquipmentType.Squad:
                    CurrentSquadExp += currentExpIncreaseValue;
                    if (CurrentSquadExp >= MaxSquadExp && CurrentSquadLevel < MaxLevelFirstValue) OnLevelUpgrade?.Invoke(type);
                    ES3.Save($"{Enum.LevelType.CurrentExp}{Enum.SummonEquipmentType.Squad}CurrentExp", CurrentWeaponExp);
                    break;
                case Enum.SummonEquipmentType.Weapon:
                    CurrentWeaponExp += currentExpIncreaseValue;
                    if (CurrentWeaponExp >= MaxWeaponExp && CurrentWeaponLevel < MaxLevelFirstValue) OnLevelUpgrade?.Invoke(type);
                    ES3.Save($"{Enum.LevelType.CurrentExp}{Enum.SummonEquipmentType.Weapon}CurrentExp", CurrentWeaponExp);
                    break;
                case Enum.SummonEquipmentType.Gear:
                    CurrentGearExp += currentExpIncreaseValue;
                    if (CurrentGearExp >= MaxGearExp && CurrentGearLevel < MaxLevelFirstValue) OnLevelUpgrade?.Invoke(type);
                    ES3.Save($"{Enum.LevelType.CurrentExp}{Enum.SummonEquipmentType.Gear}CurrentExp", CurrentGearExp);
                    break;
            }
        
            SummonPanelUI.Instance.SetSummonUI();
        }
    
        private void UpdateLevel(Enum.SummonEquipmentType type)
        {
            switch (type)
            {
                case Enum.SummonEquipmentType.Squad:
                    CurrentSquadLevel++;
                    CurrentSquadExp -= MaxSquadExp;
            
                    ES3.Save($"{Enum.LevelType.CurrentLv}{Enum.SummonEquipmentType.Squad}CurrentLevel", CurrentSquadLevel);
                    break;
                case Enum.SummonEquipmentType.Weapon:
                    CurrentWeaponLevel++;
                    CurrentWeaponExp -= MaxWeaponExp;
            
                    ES3.Save($"{Enum.LevelType.CurrentLv}{Enum.SummonEquipmentType.Weapon}CurrentLevel", CurrentWeaponLevel);
                    break;
                case Enum.SummonEquipmentType.Gear:
                    CurrentGearLevel++;
                    CurrentGearExp -= MaxGearExp;
            
                    ES3.Save($"{Enum.LevelType.CurrentLv}{Enum.SummonEquipmentType.Gear}CurrentLevel", CurrentGearLevel);
                    break;
            }
        
            SetMaxExp(type);
        
            SummonPanelUI.Instance.SetSummonUI();
        }

        private void SetMaxExp(Enum.SummonEquipmentType type)
        {
            switch (type)
            {
                case Enum.SummonEquipmentType.Squad:
                    MaxSquadExp = MaxExpFirstValue + CurrentSquadLevel * MaxExpIncreaseValue;
                    break;
                case Enum.SummonEquipmentType.Weapon:
                    MaxWeaponExp = MaxExpFirstValue + CurrentWeaponLevel * MaxExpIncreaseValue;
                    break;
                case Enum.SummonEquipmentType.Gear:
                    MaxGearExp = MaxExpFirstValue + CurrentGearLevel * MaxExpIncreaseValue;
                    break;
            }
        }
    }
}