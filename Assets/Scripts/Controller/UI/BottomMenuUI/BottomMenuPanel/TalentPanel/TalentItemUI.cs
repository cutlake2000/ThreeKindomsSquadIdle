using System;
using Creature.Data;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Managers.BottomMenuManager.TalentPanel;
using Managers.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.TalentPanel
{
    public class TalentItemUI : MonoBehaviour
    {
        [Header("이펙트 타겟")] public Transform effectTarget;
        [Header("아이콘")] public Sprite squadTalentSprite;
        [Header("스탯 이름")] public string squadTalentName;
        [Header("초기값")] public int initTalentValue;
        [Header("스탯 증가 타입")] public Enums.SquadStatType statTypeFromSquadTalentPanel;
        [Header("스탯 증가량 타입")] public Enums.IncreaseStatValueType increaseTalentValueType;
        [Header("스탯 증가량")] public int increaseTalentValue;
        [Header("현재 스탯 레벨")] public int currentLevel;
        [Header("최대 스탯 레벨")] public int maxLevel = 10000;
        [Header("초기 강화 비용 레벨")] public int initialLevelUpCost;
        [Header("추가 강화 비용 레벨")] public int extraLevelUpCost;
        [Header("x1 / x10 / x100 스탯 강화 비용")] public BigInteger[] CurrentLevelUpCost;
        [Header("현재 스탯 증가량")] public int currentIncreasedStat;
        [Header("UI")] public Image squadTalentImage;

        public TMP_Text squadTalentNameText;
        public TMP_Text squadTalentLevelText;
        public TMP_Text squadTalentMaxLevelText;
        public TMP_Text squadCurrentIncreasedStatText;
        public TMP_Text squadTalentRequiredCurrencyText;
        public Button upgradeButton;
        public Button upgradeBlockButton;
        public Action<Enums.SquadStatType, int, bool> UpgradeTotalSquadStatBySquadTalentItem;

        public void InitSquadTalentUI()
        {
            squadTalentImage.sprite = squadTalentSprite;
            squadTalentMaxLevelText.text = $"Max. {maxLevel}";
            squadTalentNameText.text = $"{squadTalentName}";
        }

        // 스텟 UI 업데이트 하는 메서드
        public void UpdateSquadTalentUI(int levelMagnification)
        {
            squadTalentLevelText.text = $"Lv. {currentLevel}";

            if (statTypeFromSquadTalentPanel is Enums.SquadStatType.CriticalRate or Enums.SquadStatType.CriticalDamage or Enums.SquadStatType.AcquisitionGold or Enums.SquadStatType.AcquisitionExp)
            {
                squadCurrentIncreasedStatText.text = currentIncreasedStat == 0 ? "0.00%" : $"{UIManager.FormatCurrency(currentIncreasedStat)}%";
            }
            else
            {
                squadCurrentIncreasedStatText.text = currentIncreasedStat == 0 ? "0" : $"{currentIncreasedStat}";
            }
            
            squadTalentRequiredCurrencyText.text = $"<sprite={(int)Enums.IconType.CurrencyType_Gold}> {CurrentLevelUpCost[(int)Mathf.Log10(levelMagnification)].ChangeMoney()}";
        }
    }
}