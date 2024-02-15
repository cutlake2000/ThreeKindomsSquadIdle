using System;
using Creature.Data;
using Data;
using ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadStatPanel
{
    public class SquadStatPanelItemUI : MonoBehaviour
    {
        [Header("아이콘")] public Sprite squadStatSprite;

        [Header("스탯 이름")] public string squadStatName;

        [Space(5)] [Header("초기값")] public int initStatValue;

        [Header("레벨 업 비용")] public int levelUpCost = 1;

        [Space(5)] [Header("스탯 증가 타입")] public Enums.SquadStatType statTypeFromSquadStatPanel;

        [Header("스탯 증가량 타입")] public Enums.IncreaseStatValueType increaseStatValueType;

        [Header("스탯 증가량")] public int increaseStatValue;

        [Space(5)] [Header("현재 스탯 레벨")] public int currentLevel;

        [Header("최대 스탯 레벨")] public int maxLevel = 10000;

        [Header("현재 스탯 강화 비용")] public int currentLevelUpCost;

        [Header("현재 스탯 증가량")] public int currentIncreasedStat;

        [Header("UI")] public Image squadStatImage;

        public Transform effectTarget;
        public TMP_Text squadStatNameText;
        public TMP_Text squadStatLevelText;
        public TMP_Text squadStatMaxLevelText;
        public TMP_Text squadCurrentIncreasedStatText;
        public Button resetButton;
        public Button upgradeButton;
        public Button upgradeBlockButton;
        public Action<Enums.SquadStatType, int, bool> UpgradeTotalSquadStatBySquadStatItem;

        public void InitSquadStatUI()
        {
            squadStatImage.sprite = squadStatSprite;
            squadStatMaxLevelText.text = $"최대 레벨 | {maxLevel}";
            squadStatNameText.text = $"{squadStatName}";
        }

        // 스텟 UI 업데이트 하는 메서드
        public void UpdateSquadStatUI()
        {
            squadStatLevelText.text = $"Lv. {currentLevel}";

            switch (increaseStatValueType)
            {
                case Enums.IncreaseStatValueType.BaseStat:
                    squadCurrentIncreasedStatText.text = currentIncreasedStat == 0 ? "0" : $"{currentIncreasedStat}";
                    break;
                case Enums.IncreaseStatValueType.PercentStat:
                    squadCurrentIncreasedStatText.text =
                        currentIncreasedStat == 0 ? "0%" : $"{(double)currentIncreasedStat / 100}%";
                    break;
            }
        }

        // 스텟 업데이트 하는 메서드
        public void UpdateSquadStat(int count)
        {
            currentLevel += count;
            currentIncreasedStat += increaseStatValue * count;

            ES3.Save($"{nameof(SquadStatSo)}/{statTypeFromSquadStatPanel}/currentLevel : ", currentLevel);
            UpgradeTotalSquadStatBySquadStatItem?.Invoke(statTypeFromSquadStatPanel, increaseStatValue * count, increaseStatValueType == Enums.IncreaseStatValueType.BaseStat);
        }
    }
}