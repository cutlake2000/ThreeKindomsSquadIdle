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
    public class UpgradeSquadStat
    {
        public Action<Enum.SquadStatType, int> UpgradeStatAction;
        
        [FormerlySerializedAs("allSquadStatType")] [Tooltip("스탯 타입")] public Enum.SquadStatType squadStatType;
        [Tooltip("현재 스탯 레벨")] public int currentUpgradeLevel;
        [Tooltip("현재 스탯 강화 비용")] public int currentUpgradeCost;
        [Tooltip("현재 스탯 증가량")] public int currentIncreasedStat;
        [Tooltip("스탯 증가량")] public int increasedStatValue;

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
            currentIncreasedStat += increasedStatValue;
            
            ES3.Save($"{nameof(SquadStat)}/{squadStatType}/{nameof(currentUpgradeLevel)} : ", currentUpgradeLevel);
            UpgradeStatAction?.Invoke(squadStatType, increasedStatValue);
        }

        // 스텟 로드할 때 부르는 메서드
        public void LoadSquadStatLevel()
        {
            for (var i = 0; i < currentUpgradeLevel; i++)
            {
                currentIncreasedStat += increasedStatValue;

                UpgradeStatAction?.Invoke(squadStatType, increasedStatValue);
            }
        }

        public UpgradeSquadStat(
            TMP_Text currentUpgradeLevelText,
            TMP_Text currentIncreasedStatText,
            Button upgradeButton,
            Enum.SquadStatType squadStatType,
            int currentUpgradeLevel,
            int currentUpgradeCost,
            int increasedStatValue,
            int currentIncreasedStat,
            Action<Enum.SquadStatType, int> upgradeStatAction = null)
        {
            this.currentUpgradeLevelText = currentUpgradeLevelText;
            this.currentIncreasedStatText = currentIncreasedStatText;
            this.upgradeButton = upgradeButton;
            
            this.squadStatType = squadStatType;
            this.currentUpgradeLevel = currentUpgradeLevel;
            this.currentUpgradeCost = currentUpgradeCost;
            this.increasedStatValue = increasedStatValue;
            this.currentIncreasedStat = currentIncreasedStat;

            UpgradeStatAction = upgradeStatAction;
            
            LoadSquadStatLevel();
        }
    }
}