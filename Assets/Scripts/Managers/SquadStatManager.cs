using System;
using Creature.Data;
using Function;
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
        public event Action<Enum.SquadStatPanelStatType, int> UpgradeTotalSquadStatAction;

        [SerializeField] private SquadStatPanelStat[] squadStatPanelStat;
        [SerializeField] private SquadStatPanelStatUI[] squadStatPanelStatUI;

        [Header("능력치 조정")]
        [Header("깡옵 세팅")]
        [SerializeField] private int upgradeBaseStatIncreaseValue;
        [SerializeField] private int upgradeBaseStatCurrentIncreaseValue;
        [SerializeField] private int upgradeBaseStatUpgradeCost;
        [Header("퍼옵 세팅")]
        [SerializeField] private int upgradePercentStatIncreaseValue;
        [SerializeField] private float upgradePercentStatCurrentIncreaseValue;
        [SerializeField] private int upgradePercentStatUpgradeCost;

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
            for (var i = 0; i < squadStatPanelStatUI.Length; i++)
            {
                var index = i;
                squadStatPanelStatUI[i].upgradeButton.onClick
                    .AddListener(() => UpgradeSquadStatPanelStat((Enum.SquadStatPanelStatType)index));
                squadStatPanelStatUI[i].upgradeButton.GetComponent<HoldButton>().onHold.AddListener(() =>
                    UpgradeSquadStatPanelStat((Enum.SquadStatPanelStatType)index));
            }
        }

        // UpdateData 초기화 메서드 - 여기서 스텟퍼센트 조정 가능
        private void SetUpgradeData()
        {
            for (var i = 0; i < squadStatPanelStatUI.Length; i++)
                squadStatPanelStat[i] = new SquadStatPanelStat(
                    squadStatPanelStatUI[i].currentUpgradeLevelText,
                    squadStatPanelStatUI[i].currentIncreasedStatText,
                    squadStatPanelStatUI[i].upgradeButton,
                    (Enum.SquadStatPanelStatType)i,
                    ES3.Load($"{nameof(SquadStat)}/{(Enum.SquadStatPanelStatType)i}/currentUpgradeLevel : ", 0),
                    upgradeBaseStatUpgradeCost,
                    upgradeBaseStatIncreaseValue,
                    upgradeBaseStatCurrentIncreaseValue,
                    UpgradeTotalSquadStatAction
                );
        }

        // 스텟 UI 업데이트
        private static void SetUpgradeUI(SquadStatPanelStat squadStatPanelStat)
        {
            squadStatPanelStat.UpdateSquadStatUI();
        }

        private void SetUpgradeUI(Enum.SquadStatPanelStatType type)
        {
            squadStatPanelStat[(int)type].UpdateSquadStatUI();
        }

        // 모든 스텟 UI 업데이트
        private void UpdateAllSquadStatUI()
        {
            foreach (var squadStat in squadStatPanelStat) squadStat.UpdateSquadStatUI();
        }

        public void UpgradeSquadStatPanelStat(Enum.SquadStatPanelStatType type)
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint,
                    squadStatPanelStat[(int)type].currentUpgradeCost)) return;
            if (squadStatPanelStatUI[(int)type].upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;

            squadStatPanelStat[(int)type].UpdateSquadStat();
            SetUpgradeUI(squadStatPanelStat[(int)type]);

            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }
    }
}