using System;
using Controller.UI.BottomMenuUI;
using Controller.UI.BottomMenuUI.BottomMenuPanel.SummonPanel;
using Data;
using Managers.BottomMenuManager.SummonPanel;
using Unity.VisualScripting;
using UnityEngine;

namespace Creature.Data
{
    public class SummonLevel
    {
        [Header("소환 타입")] public readonly Enums.SummonType summonType;
        
        [Header("초기 레벨")] public const int LEVEL_FIRST_VALUE = 1;
        [Header("최대 레벨")] public const int LEVEL_MAX_VALUE = 5;
        [Header("초기 경험치")] public const int EXP_FIRST_VALUE = 0;

        public int CurrentSummonLevel;
        public float CurrentSummonExp;
        public float TargetSummonExp;

        public SummonLevel(Enums.SummonType summonType)
        {
            this.summonType = summonType;
        }

        public void InitializeData()
        {
            CurrentSummonLevel = ES3.Load($"{nameof(Enums.LevelType.CurrentLv)}/{summonType}/{nameof(CurrentSummonLevel)}", LEVEL_FIRST_VALUE);
            CurrentSummonExp = ES3.Load<float>($"{nameof(Enums.LevelType.CurrentExp)}/{summonType}/{nameof(CurrentSummonExp)}", EXP_FIRST_VALUE);

            UpdateTargetExp();
        }
        
        public bool IncreaseExp(int currentExpIncreaseValue)
        {
            var isLevelUp = false;
            
            CurrentSummonExp += currentExpIncreaseValue;
            
            if (CurrentSummonExp >= TargetSummonExp && CurrentSummonLevel < LEVEL_MAX_VALUE)
            {
                CurrentSummonLevel++;
                CurrentSummonExp -= TargetSummonExp;
                UpdateTargetExp();
                
                ES3.Save($"{nameof(Enums.LevelType.CurrentLv)}/{summonType}/{nameof(CurrentSummonLevel)}", CurrentSummonLevel);

                isLevelUp = true;
            }
            
            ES3.Save($"{nameof(Enums.LevelType.CurrentExp)}/{summonType}/{nameof(CurrentSummonExp)}", CurrentSummonExp);

            return isLevelUp;
        }

        private void UpdateTargetExp()
        {
            TargetSummonExp = SummonManager.Instance.summonSo.GetRequiredExp(summonType, CurrentSummonLevel);
        }
    }
}