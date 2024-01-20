using System;
using System.Collections.Generic;
using Creature.Data;
using Function;
using ScriptableObjects.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Managers
{
    [Serializable]
    public struct SquadStatPanelStatUI
    {
        public TMP_Text currentUpgradeLevelText;
        public TMP_Text currentIncreasedStatText;
        public Button upgradeButton;
    }

    public class SquadStatManager : MonoBehaviour
    {
        public event Action<Enum.SquadStatTypeBySquadPanel, int> UpgradeTotalSquadStatAction;
        
        //TODO: 임시 So 대체 클래스 -> 추후 csv, json으로 대체
        [SerializeField] private SquadStatSo[] squadStatSo;
        [SerializeField] private SquadStat[] squadStats;
        public static SquadStatManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        // 이벤트 설정하는 메서드
        public void InitSquadStatManager()
        {
            SetButtonListeners();
            SetUpgradeData();
            UpdateAllSquadStatUI();
        }

        // 버튼 초기화 메서드
        private void SetButtonListeners()
        {
            for (var i = 0; i < squadStats.Length; i++)
            {
                var index = i;
                squadStats[i].GetComponent<SquadStat>().upgradeButton.onClick
                    .AddListener(() => UpgradeSquadStatPanelStat((Enum.SquadStatTypeBySquadPanel)index));
                squadStats[i].GetComponent<SquadStat>().upgradeButton.GetComponent<HoldButton>().onHold.AddListener(() =>
                    UpgradeSquadStatPanelStat((Enum.SquadStatTypeBySquadPanel)index));
            }
        }

        // UpdateData 초기화 메서드 - 여기서 스텟퍼센트 조정 가능
        private void SetUpgradeData()
        {
            for (var i = 0; i < squadStats.Length; i++)
            {
                squadStats[i].squadStatName = squadStatSo[i].squadStatName;
                squadStats[i].squadStatTypeBySquadPanel = squadStatSo[i].squadStatTypeBySquadPanel;
                squadStats[i].increaseStatValueType = squadStatSo[i].increaseStatValueType;
                squadStats[i].increaseStatValue = squadStatSo[i].increaseStatValue;
                squadStats[i].currentLevel =
                    ES3.Load($"{nameof(SquadEntireStat)}/{(Enum.SquadStatTypeBySquadPanel)i}/currentLevel : ",
                        0);
                squadStats[i].currentLevelUpCost = squadStatSo[i].levelUpCost;
                squadStats[i].currentIncreasedStat = squadStats[i].currentLevel * squadStats[i].increaseStatValue;
                squadStats[i].squadStatSprite = squadStatSo[i].squadStatImage;
                squadStats[i].UpgradeTotalSquadStatAction = UpgradeTotalSquadStatAction;
                
                squadStats[i].SetSquadStatUI();
            }
        }

        // 스텟 UI 업데이트
        private static void SetUpgradeUI(SquadStat squadStat)
        {
            squadStat.UpdateIncreaseSquadStatUI();
        }

        // 모든 스텟 UI 업데이트
        private void UpdateAllSquadStatUI()
        {
            foreach (var squadStat in squadStats) squadStat.UpdateIncreaseSquadStatUI();
        }

        public void UpgradeSquadStatPanelStat(Enum.SquadStatTypeBySquadPanel type)
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint,
                    squadStats[(int)type].levelUpCost)) return;
            if (squadStats[(int)type].upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;

            squadStats[(int)type].UpdateSquadStat();
            SetUpgradeUI(squadStats[(int)type]);

            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }
    }
}