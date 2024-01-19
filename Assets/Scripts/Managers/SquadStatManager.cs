using System;
using System.Collections.Generic;
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
    public struct SquadStatUI
    {
        public TMP_Text currentUpgradeLevel;
        public TMP_Text currentIncreasedStat;
        public Button upgradeButton;
    }
    public class SquadStatManager : MonoBehaviour
    {
        public static event Action<Enum.SquadStatPanelStatType, int>[] UpgradeTotalSquadStatAction;
        public static event Action<Enum.SquadStatPanelStatType, int> UpgradeTotalSquadHealthAction;
        public static event Action<Enum.SquadStatPanelStatType, int> UpgradeTotalSquadPenetrationAction;
        public static event Action<Enum.SquadStatPanelStatType, int> UpgradeTotalSquadAccuracyAction;
        public static event Action<Enum.SquadStatPanelStatType, int> UpgradeTotalSquadAcquisitionGoldAction;
        public static event Action<Enum.SquadStatPanelStatType, int> UpgradeTotalSquadAcquisitionExpAAction;
        public static event Action<Enum.SquadStatPanelStatType, int> UpgradeTotalSquadCritialDamageAAction;

        public static SquadStatManager Instance;

        [SerializeField] private UpgradeSquadStat upgradeSquadAttackStat;
        [SerializeField] private UpgradeSquadStat upgradeSquadHealthStat;
        [SerializeField] private UpgradeSquadStat upgradeSquadPenetrationStat;
        [SerializeField] private UpgradeSquadStat upgradeSquadAccuracyStat;
        [SerializeField] private UpgradeSquadStat upgradeSquadAcquisitionGoldStat;
        [SerializeField] private UpgradeSquadStat upgradeSquadAcquisitionExpStat;
        [SerializeField] private UpgradeSquadStat upgradeSquadCriticalDamageStat;
        
        [SerializeField] private SquadStatUI squadAttackStatUI;
        [SerializeField] private SquadStatUI squadHealthStatUI;
        [SerializeField] private SquadStatUI squadPenetrationStatUI;
        [SerializeField] private SquadStatUI squadAccuracyStatUI;
        [SerializeField] private SquadStatUI squadAcquisitionGoldStatUI;
        [SerializeField] private SquadStatUI squadAcquisitionExpStatUI;
        [SerializeField] private SquadStatUI squadCriticalDamageStatUI;

        [Header("능력치 조정")]
        [Header("깡옵 세팅")]
        [SerializeField] private int upgradeBaseStatIncreaseValue;
        [SerializeField] private int upgradeBaseStatCurrentIncreaseValue;
        [SerializeField] private int upgradeBaseStatUpgradeCost;
        [Header("퍼옵 세팅")]
        [SerializeField] private int upgradePercentStatIncreaseValue;
        [SerializeField] private float upgradePercentStatCurrentIncreaseValue;
        [SerializeField] private int upgradePercentStatUpgradeCost;

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
            squadAttackStatUI.upgradeButton.onClick.AddListener(UpgradeAttack);
            squadHealthStatUI.upgradeButton.onClick.AddListener(UpgradeHealth);
            squadPenetrationStatUI.upgradeButton.onClick.AddListener(upgrpene);
            squadAccuracyStatUI.upgradeButton.onClick.AddListener(UpgradeHealth);
            squadAcquisitionGoldStatUI.upgradeButton.onClick.AddListener(UpgradeHealth);
            squadAcquisitionExpStatUI.upgradeButton.onClick.AddListener(UpgradeHealth);
            squadCriticalDamageStatUI.upgradeButton.onClick.AddListener(UpgradeCriticalDamage);
        
            attackUpgradeButton.GetComponent<HoldButton>().onHold.AddListener(UpgradeAttack);
            healthUpgradeButton.GetComponent<HoldButton>().onHold.AddListener(UpgradeHealth);
            defenceUpgradeButton.GetComponent<HoldButton>().onHold.AddListener(UpgradeDefense);
            criticalDamageUpgradeButton.GetComponent<HoldButton>().onHold.AddListener(UpgradeCriticalDamage);
        }

        // UpdateData 초기화 메서드 - 여기서 스텟퍼센트 조정 가능
        private void SetUpgradeData()
        {
            upgradeSquadAttackStat = new UpgradeSquadStat(
                currentUpgradeLevelText: currentAttackUpgradeLevelText,
                currentIncreasedStatText: currentAttackIncreaseStatText,
                upgradeButton: attackUpgradeButton,
                Enum.SquadStatPanelStatType.Atk,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatPanelStatType.Atk}/{nameof(upgradeSquadAttackStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadAttackAction
                );
            upgradeSquadHealthStat = new UpgradeSquadStat(
                currentUpgradeLevelText: currentHealthUpgradeLevelText,
                currentIncreasedStatText: currentHealthIncreaseStatText,
                upgradeButton: healthUpgradeButton,
                Enum.SquadStatPanelStatType.Hp,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatPanelStatType.Hp}/{nameof(upgradeSquadHealthStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadHealthAction
                );
            upgradeSquadPenetrationStat = new UpgradeSquadStat(
                currentUpgradeLevelText: currentPenetrationUpgradeLevelText,
                currentIncreasedStatText: currentPenetrationIncreaseStatText,
                upgradeButton: defenceUpgradeButton,
                Enum.SquadStatPanelStatType.Def,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatPanelStatType.Def}/{nameof(upgradeSquadPenetrationStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadPenetrationAction
                );
            upgradeSquadAccuracyStat = new UpgradeSquadStat(
                currentUpgradeLevelText: currentAccuracyUpgradeLevelText,
                currentIncreasedStatText: currentAccuracyIncreaseStatText,
                upgradeButton: accuracyUpgradeButton,
                Enum.SquadStatPanelStatType.Accuracy,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatPanelStatType.Accuracy}/{nameof(upgradeSquadAccuracyStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadAccuracyAction
            );
            upgradeSquadAcquisitionGoldStat = new UpgradeSquadStat(
                currentUpgradeLevelText: currentCriticalDamageUpgradeLevelText,
                currentIncreasedStatText: currentCriticalDamageIncreaseStatText,
                upgradeButton: criticalDamageUpgradeButton,
                Enum.SquadStatPanelStatType.CrtDmg,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatPanelStatType.CrtDmg}/{nameof(upgradeSquadCriticalDamageStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadCriticalDamageAction
            );
            upgradeSquadPenetrationStat = new UpgradeSquadStat(
                currentUpgradeLevelText: currentPenetrationUpgradeLevelText,
                currentIncreasedStatText: currentPenetrationIncreaseStatText,
                upgradeButton: penetrationUpgradeButton,
                Enum.SquadStatPanelStatType.Penetration,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatPanelStatType.Penetration}/{nameof(upgradeSquadPenetrationStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadPenetrationAction
            );
  
        }
        
        // 스텟 UI 업데이트
        private static void SetUpgradeUI(UpgradeSquadStat upgradeSquadStat) => upgradeSquadStat.UpdateSquadStatUI();

        private void SetUpgradeUI(Enum.SquadStatPanelStatType type)
        {
            switch (type)
            {
                case Enum.SquadStatPanelStatType.Atk:
                    upgradeSquadAttackStat.UpdateSquadStatUI();
                    break;
                case Enum.SquadStatPanelStatType.Hp:
                    upgradeSquadHealthStat.UpdateSquadStatUI();
                    break;
                    break;
                case Enum.SquadStatPanelStatType.CrtDmg:
                    upgradeSquadCriticalDamageStat.UpdateSquadStatUI();
                    break;
                case Enum.SquadStatPanelStatType.Penetration:
                    upgradeSquadPenetrationStat.UpdateSquadStatUI();
                    break;
                case Enum.SquadStatPanelStatType.Accuracy:
                    upgradeSquadAccuracyStat.UpdateSquadStatUI();
                    break;
            }
        }

        // 모든 스텟 UI 업데이트
        private void UpdateAllSquadStatUI()
        {
            upgradeSquadAttackStat.UpdateSquadStatUI();
            upgradeSquadHealthStat.UpdateSquadStatUI();
            upgradeSquadCriticalDamageStat.UpdateSquadStatUI();
            upgradeSquadPenetrationStat.UpdateSquadStatUI();
            upgradeSquadAccuracyStat.UpdateSquadStatUI();
        }

        // 버튼 눌렸을 때 동작하는 메서드
        public void UpgradeAttack()
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint, upgradeSquadAttackStat.currentUpgradeCost)) return;
            if (squadAttackStatUI.upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;

            upgradeSquadAttackStat.UpdateSquadStat();
            SetUpgradeUI(upgradeSquadAttackStat);
            
            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }

        public void UpgradeHealth()
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint, upgradeSquadHealthStat.currentUpgradeCost)) return;
            if (squadHealthStatUI.upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;
            
            upgradeSquadHealthStat.UpdateSquadStat();
            SetUpgradeUI(upgradeSquadHealthStat);
            
            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }

        public void UpgradePenetration()
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint, upgradeSquadPenetrationStat.currentUpgradeCost)) return;
            if (squadPenetrationStatUI.upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;
            
            upgradeSquadPenetrationStat.UpdateSquadStat();
            SetUpgradeUI(upgradeSquadPenetrationStat);
            
            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }

        public void UpgradeCriticalDamage()
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint, upgradeSquadCriticalDamageStat.currentUpgradeCost)) return;
            if (squadCriticalDamageStatUI.upgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;
            
            upgradeSquadCriticalDamageStat.UpdateSquadStat();
            SetUpgradeUI(upgradeSquadCriticalDamageStat);
            
            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }
    }
}