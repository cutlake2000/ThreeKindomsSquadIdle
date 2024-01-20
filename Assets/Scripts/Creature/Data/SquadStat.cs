using System;
using Function;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Creature.Data
{
    public class SquadStat : MonoBehaviour
    {
        public Action<Enum.SquadStatTypeBySquadPanel, float> UpgradeTotalSquadStatAction;

        [Header("아이콘")]
        public Sprite squadStatSprite;
        [Header("스탯 이름")]
        public string squadStatName;
        [Space(5)]
        [Header("초기값")]
        public int initStatValue = 0;
        [Header("레벨 업 비용")]
        public int levelUpCost = 1;
        [Space(5)]
        [Header("스탯 증가 타입")]
        public Enum.SquadStatTypeBySquadPanel squadStatTypeBySquadPanel;
        [Header("스탯 증가량 타입")]
        public Enum.IncreaseStatValueType increaseStatValueType;
        [Header("스탯 증가량")]
        public float increaseStatValue;
        [Space(5)]
        [Header("현재 스탯 레벨")]
        public int currentLevel; 
        [Header("현재 스탯 강화 비용")]
        public int currentLevelUpCost;
        [Header("현재 스탯 증가량")]
        public float currentIncreasedStat;

        [Header("UI")]
        public Image squadStatImage;
        public TMP_Text squadStatNameText;
        public TMP_Text squadStatLevelText;
        public TMP_Text squadCurrentIncreasedStatText;
        public Button resetButton;
        public Button upgradeButton;

        public SquadStat(
            Sprite squadStatImage,
            Enum.SquadStatTypeBySquadPanel squadStatTypeBySquadPanel,
            int currentLevel,
            int currentLevelUpCost,
            Enum.IncreaseStatValueType increaseStatValueType,
            int increaseStatValue,
            int currentIncreasedStat,
            Action<Enum.SquadStatTypeBySquadPanel, float> upgradeTotalSquadStatAction)
        {
            this.squadStatImage.sprite = squadStatImage;
            this.squadStatTypeBySquadPanel = squadStatTypeBySquadPanel;
            this.currentLevel = currentLevel;
            this.currentLevelUpCost = currentLevelUpCost;
            this.increaseStatValueType = increaseStatValueType;
            this.increaseStatValue = increaseStatValue;
            this.currentIncreasedStat = currentIncreasedStat;

            UpgradeTotalSquadStatAction = upgradeTotalSquadStatAction;
        }

        public void SetSquadStatUI()
        {
            squadStatImage.sprite = squadStatSprite;
            squadStatNameText.text = $"{squadStatName}";
        }

        // 스텟 UI 업데이트 하는 메서드
        public void UpdateIncreaseSquadStatUI()
        {
            squadStatLevelText.text = $"Lv. {currentLevel}";
            
            switch (increaseStatValueType)
            {
                case Enum.IncreaseStatValueType.BaseStat:
                    Debug.Log("깡옵");
                    squadCurrentIncreasedStatText.text = currentIncreasedStat == 0? "0" : $"{currentIncreasedStat}";
                    break;
                case Enum.IncreaseStatValueType.PercentStat:
                    Debug.Log("퍼옵");
                    squadCurrentIncreasedStatText.text = currentIncreasedStat == 0? "0%" : $"{currentIncreasedStat}%";
                    break;
            }
        }

        // 스텟 업데이트 하는 메서드
        public void UpdateSquadStat()
        {
            currentLevel++;
            currentIncreasedStat += increaseStatValue;
            
            ES3.Save($"{nameof(SquadEntireStat)}/{squadStatTypeBySquadPanel}/currentLevel : ", currentLevel);
            UpgradeTotalSquadStatAction?.Invoke(squadStatTypeBySquadPanel, increaseStatValue);
        }

        // 스텟 로드할 때 부르는 메서드
        public void LoadSquadStatLevel()
        {
            for (var i = 0; i < currentLevel; i++)
            {
                currentIncreasedStat += increaseStatValue;

                UpgradeTotalSquadStatAction?.Invoke(squadStatTypeBySquadPanel, increaseStatValue);
            }
        }
    }
}