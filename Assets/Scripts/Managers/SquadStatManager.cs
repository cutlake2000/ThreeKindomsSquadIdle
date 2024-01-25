using System;
using Controller.UI.BottomMenuUI.SquadMenu;
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
        public static SquadStatManager Instance;
        public event Action<Enum.SquadStatTypeBySquadPanel, int> UpgradeTotalSquadStatAction;

        //TODO: 임시 So 대체 클래스 -> 추후 csv, json으로 대체
        [SerializeField] private SquadStatSo[] squadStatSo;
        public SquadStatItemUI[] squadStatItem;

        private void Awake()
        {
            Instance = this;
        }

        // 이벤트 설정하는 메서드
        public void InitSquadStatManager()
        {
            InitializeEventListeners();
            SetSquadStatData();
            UpdateAllSquadStatUI();
        }

        // 버튼 초기화 메서드
        private void InitializeEventListeners()
        {
            for (var i = 0; i < squadStatItem.Length; i++)
            {
                var index = i;
                squadStatItem[i].GetComponent<SquadStatItemUI>().
                    upgradeButton.onClick.AddListener(() => UpgradeSquadStatPanelStat((Enum.SquadStatTypeBySquadPanel)index));
                squadStatItem[i].GetComponent<SquadStatItemUI>().
                    upgradeButton.GetComponent<HoldButton>().onHold.AddListener(() => UpgradeSquadStatPanelStat((Enum.SquadStatTypeBySquadPanel)index));
            }
        }

        // UpdateData 초기화 메서드 - 여기서 스텟퍼센트 조정 가능
        private void SetSquadStatData()
        {
            for (var i = 0; i < squadStatItem.Length; i++)
            {
                squadStatItem[i].squadStatName = squadStatSo[i].squadStatName;
                squadStatItem[i].squadStatTypeBySquadPanel = squadStatSo[i].squadStatTypeBySquadPanel;
                squadStatItem[i].increaseStatValueType = squadStatSo[i].increaseStatValueType;
                squadStatItem[i].increaseStatValue = squadStatSo[i].increaseStatValue;
                squadStatItem[i].currentLevel =
                    ES3.Load($"{nameof(SquadEntireStat)}/{(Enum.SquadStatTypeBySquadPanel)i}/currentLevel : ",
                        0);
                squadStatItem[i].currentLevelUpCost = squadStatSo[i].levelUpCost;
                squadStatItem[i].currentIncreasedStat = squadStatItem[i].currentLevel * squadStatItem[i].increaseStatValue;
                squadStatItem[i].squadStatSprite = squadStatSo[i].squadStatImage;
                squadStatItem[i].UpgradeTotalSquadStatAction = UpgradeTotalSquadStatAction;

                squadStatItem[i].InitSquadStatUI();
            }
        }
        
        // 모든 스텟 UI 업데이트
        private void UpdateAllSquadStatUI()
        {
            foreach (var squadStat in squadStatItem) squadStat.UpdateSquadStatUI();
        }

        public void UpgradeSquadStatPanelStat(Enum.SquadStatTypeBySquadPanel type)
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint, squadStatItem[(int)type].levelUpCost * UIManager.Instance.squadPanelUI.levelUpMagnification)) return;
            if (squadStatItem[(int)type].upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;

            squadStatItem[(int)type].UpdateSquadStat(UIManager.Instance.squadPanelUI.levelUpMagnification);
            SetUpgradeUI(squadStatItem[(int)type]);
            UIManager.Instance.squadPanelUI.CheckRequiredStatPointOfMagnificationButton((int) type);

            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }
        
        // 스텟 UI 업데이트
        private static void SetUpgradeUI(SquadStatItemUI squadStatItemUI)
        {
            squadStatItemUI.UpdateSquadStatUI();
        }

    }
}