using System;
using Creature.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Controller.UI.BottomMenuUI.SquadMenu
{
    public class SquadStatUI : MonoBehaviour
    {
        public Action<Enum.SquadStatTypeBySquadPanel, int> UpgradeTotalSquadStatAction;

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
        public int increaseStatValue;
        [Space(5)]
        [Header("현재 스탯 레벨")]
        public int currentLevel; 
        [Header("최대 스탯 레벨")]
        public int maxLevel = 10000; 
        [Header("현재 스탯 강화 비용")]
        public int currentLevelUpCost;
        [Header("현재 스탯 증가량")]
        public int currentIncreasedStat;

        [Header("UI")]
        public Image squadStatImage;
        public TMP_Text squadStatNameText;
        public TMP_Text squadStatLevelText;
        public TMP_Text squadStatMaxLevelText;
        public TMP_Text squadCurrentIncreasedStatText;
        public Button resetButton;
        public Button upgradeButton;
        public Button upgradeBlockButton;

        public void SetSquadStatUI()
        {
            squadStatImage.sprite = squadStatSprite;
            squadStatMaxLevelText.text = $"최대 레벨 | {maxLevel}";
            squadStatNameText.text = $"{squadStatName}";
        }

        // 스텟 UI 업데이트 하는 메서드
        public void UpdateIncreaseSquadStatUI()
        {
            squadStatLevelText.text = $"Lv. {currentLevel}";
            
            switch (increaseStatValueType)
            {
                case Enum.IncreaseStatValueType.BaseStat:
                    squadCurrentIncreasedStatText.text = currentIncreasedStat == 0? "0" : $"{currentIncreasedStat}";
                    break;
                case Enum.IncreaseStatValueType.PercentStat:
                    squadCurrentIncreasedStatText.text = currentIncreasedStat == 0? "0%" : $"{(double) currentIncreasedStat / 100}%";
                    break;
            }
        }

        // 스텟 업데이트 하는 메서드
        public void UpdateSquadStat(int count)
        {
            currentLevel += count;
            currentIncreasedStat += increaseStatValue * count;
            
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