using System;
using Controller.UI;
using UnityEngine;
using Enum = Data.Enum;

namespace Creature.Data
{
    public class SquadLevel
    {
            public event Action OnLevelUpgrade;
        public event Action<float> OnExpUpgrade;
    
        public int IncreaseAttack => 2;
        public int IncreaseHealth => 50;
        public int IncreaseDefence => 2;

        [Header("[Level]")] [Header("[초기 레벨, 초기 경험치, 레벨 당 경험치 증가율]")] [SerializeField]
        private const int LevelFirstValue = 1;

        private const int MaxLevelFirstValue = 1000;
        private const float ExpFirstValue = 0f;
        private const float MaxExpFirstValue = 100f;
        private const float MaxExpIncreaseValue = 1.2f;

        public int CurrentLevel { get; private set; }
        public float CurrentExp { get; private set; }
        public float MaxExp { get; private set; }
    
        public void IncreaseExp(float currentExpIncreaseValue)
        {
            OnExpUpgrade?.Invoke(currentExpIncreaseValue);
        }

        public void InitializeData()
        {
            CurrentLevel = ES3.Load($"{Enum.LevelType.CurrentLv}CurrentLevel", LevelFirstValue);
            CurrentExp = ES3.Load($"{Enum.LevelType.CurrentExp}CurrentExp", ExpFirstValue);
        
            SetMaxExp();
            SetBaseStats();

            LevelUI.Instance.SetUI();
        }
    
        public void SetupEventListeners()
        {
            OnExpUpgrade += UpdateCurrentExp;
        
            OnLevelUpgrade += UpdateLevel;
            OnLevelUpgrade += UpdateBaseStats;
        }

        private void UpdateCurrentExp(float currentExpIncreaseValue)
        {
            CurrentExp += currentExpIncreaseValue;
        
            if (CurrentExp >= MaxExp && CurrentLevel < MaxLevelFirstValue)
            {
                OnLevelUpgrade?.Invoke();
            }
        
            ES3.Save($"{Enum.LevelType.CurrentExp}CurrentExp", CurrentExp);
        
            LevelUI.Instance.SetUI();
        }
    
        private void UpdateLevel()
        {
            CurrentLevel++;
            CurrentExp -= MaxExp;
            
            ES3.Save($"{Enum.LevelType.CurrentLv}CurrentLevel", CurrentLevel);
            
            UpdateMaxExp();
        
            LevelUI.Instance.SetUI();
        }

        private void SetMaxExp()
        {
            MaxExp = MaxExpFirstValue * (float)(Math.Pow(MaxExpIncreaseValue, CurrentLevel - 1));
        }
    
        private void SetBaseStats()
        {
            Managers.SquadManager.Instance.squadEntireStat.IncreaseBaseStat(Enum.SquadStatType.Attack, IncreaseAttack * (CurrentLevel - 1));
            Managers.SquadManager.Instance.squadEntireStat.IncreaseBaseStat(Enum.SquadStatType.Health, IncreaseHealth * (CurrentLevel - 1));
            Managers.SquadManager.Instance.squadEntireStat.IncreaseBaseStat(Enum.SquadStatType.Defence, IncreaseDefence * (CurrentLevel - 1));
        }
    
        private void UpdateMaxExp()
        {
            MaxExp *= MaxExpIncreaseValue;
        }
        
        private void UpdateBaseStats()
        {
            Managers.SquadManager.Instance.squadEntireStat.IncreaseBaseStat(Enum.SquadStatType.Attack, IncreaseAttack);
            Managers.SquadManager.Instance.squadEntireStat.IncreaseBaseStat(Enum.SquadStatType.Health, IncreaseHealth);
            Managers.SquadManager.Instance.squadEntireStat.IncreaseBaseStat(Enum.SquadStatType.Defence, IncreaseDefence);
        }
    }
}