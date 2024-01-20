using System;
using Function;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Creature.Data
{
    [Serializable]
    public class SquadStatBySquadPanel
    {
        public Action<Enum.SquadStatTypeBySquadPanel, int> UpgradeTotalSquadStatAction;
        
        [Tooltip("스탯 타입")] public Enum.SquadStatTypeBySquadPanel squadStatTypeBySquadPanel;
        [Tooltip("현재 스탯 레벨")] public int currentUpgradeLevel;
        [Tooltip("현재 스탯 강화 비용")] public int currentUpgradeCost;
        [Tooltip("현재 스탯 증가량")] public int currentIncreasedStat;
        [Tooltip("스탯 증가량 타입")] public Enum.IncreaseStatValueType increaseStatValueType;
        [Tooltip("스탯 증가량")] public int increaseStatValue;

        public TMP_Text currentUpgradeLevelText;
        public TMP_Text currentIncreasedStatText;
        public Button upgradeButton;

        // 스텟 UI 업데이트 하는 메서드
        public void UpdateSquadStatUI()
        {
            currentUpgradeLevelText.text = $"Lv. {currentUpgradeLevel}";
            
            if (currentIncreasedStat == 0)
            {
                currentIncreasedStatText.text = "0";
                return;
            }
            
            currentIncreasedStatText.text = currentIncreasedStat == 0? "0" : $"{currentIncreasedStat}";
        }

        // 스텟 업데이트 하는 메서드
        public void UpdateSquadStat()
        {
            currentUpgradeLevel++;
            currentIncreasedStat += increaseStatValue;
            
            ES3.Save($"{nameof(SquadEntireStat)}/{squadStatTypeBySquadPanel}/{nameof(currentUpgradeLevel)} : ", currentUpgradeLevel);
            UpgradeTotalSquadStatAction?.Invoke(squadStatTypeBySquadPanel, increaseStatValue);
        }

        // 스텟 로드할 때 부르는 메서드
        public void LoadSquadStatLevel()
        {
            for (var i = 0; i < currentUpgradeLevel; i++)
            {
                currentIncreasedStat += increaseStatValue;

                UpgradeTotalSquadStatAction?.Invoke(squadStatTypeBySquadPanel, increaseStatValue);
            }
        }

        public SquadStatBySquadPanel(
            TMP_Text currentUpgradeLevelText,
            TMP_Text currentIncreasedStatText,
            Button upgradeButton,
            Enum.SquadStatTypeBySquadPanel squadStatTypeBySquadPanel,
            int currentUpgradeLevel,
            int currentUpgradeCost,
            Enum.IncreaseStatValueType increaseStatValueType,
            int increaseStatValue,
            int currentIncreasedStat,
            Action<Enum.SquadStatTypeBySquadPanel, int> upgradeTotalSquadStatAction)
        {
            this.currentUpgradeLevelText = currentUpgradeLevelText;
            this.currentIncreasedStatText = currentIncreasedStatText;
            this.upgradeButton = upgradeButton;
            
            this.squadStatTypeBySquadPanel = squadStatTypeBySquadPanel;
            this.currentUpgradeLevel = currentUpgradeLevel;
            this.currentUpgradeCost = currentUpgradeCost;
            this.increaseStatValueType = increaseStatValueType;
            this.increaseStatValue = increaseStatValue;
            this.currentIncreasedStat = currentIncreasedStat;

            UpgradeTotalSquadStatAction = upgradeTotalSquadStatAction;
            
            LoadSquadStatLevel();
        }
    }
}