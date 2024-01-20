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
        public event Action<Enum.SquadStatTypeBySquadPanel, int> UpgradeTotalSquadStatAction;

        [SerializeField] private SquadStatBySquadPanel[] squadStatPanelStat;
        [SerializeField] private SquadStatPanelStatUI[] squadStatPanelStatUI;
        
        [Header("능력치 조정")]
        [SerializeField] private int currentIncreaseStatValue;
        [SerializeField] private int upgradeCost;
        [Header("깡옵 세팅")]
        [SerializeField] private int increaseBaseStatValue;
        [Header("퍼옵 세팅")]
        [SerializeField] private int increasePercentStatValue;

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
                    .AddListener(() => UpgradeSquadStatPanelStat((Enum.SquadStatTypeBySquadPanel)index));
                squadStatPanelStatUI[i].upgradeButton.GetComponent<HoldButton>().onHold.AddListener(() =>
                    UpgradeSquadStatPanelStat((Enum.SquadStatTypeBySquadPanel)index));
            }
        }

        // UpdateData 초기화 메서드 - 여기서 스텟퍼센트 조정 가능
        private void SetUpgradeData()
        {
            for (var i = 0; i < squadStatPanelStatUI.Length; i++)
                squadStatPanelStat[i] = new SquadStatBySquadPanel(
                    squadStatPanelStatUI[i].currentUpgradeLevelText,
                    squadStatPanelStatUI[i].currentIncreasedStatText,
                    squadStatPanelStatUI[i].upgradeButton,
                    (Enum.SquadStatTypeBySquadPanel)i,
                    ES3.Load($"{nameof(SquadEntireStat)}/{(Enum.SquadStatTypeBySquadPanel)i}/currentUpgradeLevel : ", 0),
                    upgradeCost,
                    (Enum.IncreaseStatValueType)1,
                    increaseBaseStatValue,
                    currentIncreaseStatValue,
                    UpgradeTotalSquadStatAction
                );
        }

        // 스텟 UI 업데이트
        private static void SetUpgradeUI(SquadStatBySquadPanel squadStatBySquadPanel)
        {
            squadStatBySquadPanel.UpdateSquadStatUI();
        }

        private void SetUpgradeUI(Enum.SquadStatTypeBySquadPanel type)
        {
            squadStatPanelStat[(int)type].UpdateSquadStatUI();
        }

        // 모든 스텟 UI 업데이트
        private void UpdateAllSquadStatUI()
        {
            foreach (var squadStat in squadStatPanelStat) squadStat.UpdateSquadStatUI();
        }

        public void UpgradeSquadStatPanelStat(Enum.SquadStatTypeBySquadPanel type)
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