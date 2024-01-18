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
        [SerializeField] private TMP_Text currentUpgradeLevel;
        [SerializeField] private TMP_Text currentIncreasedStat;
        [SerializeField] private TMP_Text currentUpgradeCost;
        [SerializeField] private Button upgradeButton;
    }
    public class SquadStatManager : MonoBehaviour
    {
        public static event Action<Enum.SquadStatType, int> UpgradeTotalSquadAttackAction;
        public static event Action<Enum.SquadStatType, int> UpgradeTotalSquadHealthAction;
        public static event Action<Enum.SquadStatType, int> UpgradeTotalSquadDefenceAction;
        public static event Action<Enum.SquadStatType, int> UpgradeTotalSquadPenetrationAction;
        public static event Action<Enum.SquadStatType, int> UpgradeTotalSquadAccuracyAction;
        public static event Action<Enum.SquadStatType, int> UpgradeTotalSquadCriticalDamageAction;

        public static SquadStatManager Instance;

        [SerializeField] private UpgradeSquadStat upgradeSquadAttackStat;
        [SerializeField] private UpgradeSquadStat upgradeSquadHealthStat;
        [SerializeField] private UpgradeSquadStat upgradeSquadDefenceStat;
        [SerializeField] private UpgradeSquadStat upgradeSquadPenetrationStat;
        [SerializeField] private UpgradeSquadStat upgradeSquadAccuracyStat;
        [SerializeField] private UpgradeSquadStat upgradeSquadCriticalDamageStat;

        [Header("능력치 조정")]
        [Header("깡옵 세팅")]
        [SerializeField] private int upgradeBaseStatIncreaseValue;
        [SerializeField] private int upgradeBaseStatCurrentIncreaseValue;
        [SerializeField] private int upgradeBaseStatUpgradeCost;
        [Header("퍼옵 세팅")]
        [SerializeField] private int upgradePercentStatIncreaseValue;
        [SerializeField] private float upgradePercentStatCurrentIncreaseValue;
        [SerializeField] private int upgradePercentStatUpgradeCost;
        
        [Header("업그레이드 UI")]
        [Header("[공격력]")]
        [SerializeField] private TMP_Text currentAttackUpgradeLevelText;
        [SerializeField] private TMP_Text currentAttackIncreaseStatText;
        [SerializeField] private Button attackUpgradeButton;
        
        [Header("[체력]")]
        [SerializeField] private TMP_Text currentHealthUpgradeLevelText;
        [SerializeField] private TMP_Text currentHealthIncreaseStatText;
        [SerializeField] private Button healthUpgradeButton;

        [Header("[방어력]")]
        [SerializeField] private TMP_Text currentDefenceUpgradeLevelText;
        [SerializeField] private TMP_Text currentDefenceIncreaseStatText;
        [SerializeField] private Button defenceUpgradeButton;
        
        [Header("[치명타 데미지]")]
        [SerializeField] private TMP_Text currentCriticalDamageUpgradeLevelText;
        [SerializeField] private TMP_Text currentCriticalDamageIncreaseStatText;
        [SerializeField] private Button criticalDamageUpgradeButton;
        
        [Header("[관통]")]
        [SerializeField] private TMP_Text currentPenetrationUpgradeLevelText;
        [SerializeField] private TMP_Text currentPenetrationIncreaseStatText;
        [SerializeField] private Button penetrationUpgradeButton;
        
        [Header("[명중]")]
        [SerializeField] private TMP_Text currentAccuracyUpgradeLevelText;
        [SerializeField] private TMP_Text currentAccuracyIncreaseStatText;
        [SerializeField] private Button accuracyUpgradeButton;

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
            attackUpgradeButton.onClick.AddListener(UpgradeAttack);
            healthUpgradeButton.onClick.AddListener(UpgradeHealth);
            defenceUpgradeButton.onClick.AddListener(UpgradeDefense);
            criticalDamageUpgradeButton.onClick.AddListener(UpgradeCriticalDamage);
        
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
                Enum.SquadStatType.Atk,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatType.Atk}/{nameof(upgradeSquadAttackStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadAttackAction
                );
            upgradeSquadHealthStat = new UpgradeSquadStat(
                currentUpgradeLevelText: currentHealthUpgradeLevelText,
                currentIncreasedStatText: currentHealthIncreaseStatText,
                upgradeButton: healthUpgradeButton,
                Enum.SquadStatType.Hp,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatType.Hp}/{nameof(upgradeSquadHealthStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadHealthAction
                );
            upgradeSquadDefenceStat = new UpgradeSquadStat(
                currentUpgradeLevelText: currentDefenceUpgradeLevelText,
                currentIncreasedStatText: currentDefenceIncreaseStatText,
                upgradeButton: defenceUpgradeButton,
                Enum.SquadStatType.Def,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatType.Def}/{nameof(upgradeSquadDefenceStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadDefenceAction
                );
            upgradeSquadCriticalDamageStat = new UpgradeSquadStat(
                currentUpgradeLevelText: currentCriticalDamageUpgradeLevelText,
                currentIncreasedStatText: currentCriticalDamageIncreaseStatText,
                upgradeButton: criticalDamageUpgradeButton,
                Enum.SquadStatType.CrtDmg,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatType.CrtDmg}/{nameof(upgradeSquadCriticalDamageStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadCriticalDamageAction
            );
            upgradeSquadPenetrationStat = new UpgradeSquadStat(
                currentUpgradeLevelText: currentPenetrationUpgradeLevelText,
                currentIncreasedStatText: currentPenetrationIncreaseStatText,
                upgradeButton: penetrationUpgradeButton,
                Enum.SquadStatType.Penetration,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatType.Penetration}/{nameof(upgradeSquadPenetrationStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadPenetrationAction
            );
            upgradeSquadAccuracyStat = new UpgradeSquadStat(
                currentUpgradeLevelText: currentAccuracyUpgradeLevelText,
                currentIncreasedStatText: currentAccuracyIncreaseStatText,
                upgradeButton: accuracyUpgradeButton,
                Enum.SquadStatType.Accuracy,
                ES3.Load($"{nameof(SquadStat)}/{Enum.SquadStatType.Accuracy}/{nameof(upgradeSquadAccuracyStat.currentUpgradeLevel)} : ", 0),
                upgradeBaseStatUpgradeCost,
                upgradeBaseStatIncreaseValue,
                upgradeBaseStatCurrentIncreaseValue,
                upgradeStatAction: UpgradeTotalSquadAccuracyAction
            );
        }
        
        // 스텟 UI 업데이트
        private static void SetUpgradeUI(UpgradeSquadStat upgradeSquadStat) => upgradeSquadStat.UpdateSquadStatUI();

        private void SetUpgradeUI(Enum.SquadStatType type)
        {
            switch (type)
            {
                case Enum.SquadStatType.Atk:
                    upgradeSquadAttackStat.UpdateSquadStatUI();
                    break;
                case Enum.SquadStatType.Hp:
                    upgradeSquadHealthStat.UpdateSquadStatUI();
                    break;
                case Enum.SquadStatType.Def:
                    upgradeSquadDefenceStat.UpdateSquadStatUI();
                    break;
                case Enum.SquadStatType.CrtDmg:
                    upgradeSquadCriticalDamageStat.UpdateSquadStatUI();
                    break;
                case Enum.SquadStatType.Penetration:
                    upgradeSquadPenetrationStat.UpdateSquadStatUI();
                    break;
                case Enum.SquadStatType.Accuracy:
                    upgradeSquadAccuracyStat.UpdateSquadStatUI();
                    break;
            }
        }

        // 모든 스텟 UI 업데이트
        private void UpdateAllSquadStatUI()
        {
            upgradeSquadAttackStat.UpdateSquadStatUI();
            upgradeSquadHealthStat.UpdateSquadStatUI();
            upgradeSquadDefenceStat.UpdateSquadStatUI();
            upgradeSquadCriticalDamageStat.UpdateSquadStatUI();
            upgradeSquadPenetrationStat.UpdateSquadStatUI();
            upgradeSquadAccuracyStat.UpdateSquadStatUI();
        }

        // 버튼 눌렸을 때 동작하는 메서드
        public void UpgradeAttack()
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint, upgradeSquadAttackStat.currentUpgradeCost)) return;
            if (attackUpgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;

            upgradeSquadAttackStat.UpdateSquadStat();
            SetUpgradeUI(upgradeSquadAttackStat);
            
            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }

        public void UpgradeHealth()
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint, upgradeSquadHealthStat.currentUpgradeCost)) return;
            if (attackUpgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;
            
            upgradeSquadHealthStat.UpdateSquadStat();
            SetUpgradeUI(upgradeSquadHealthStat);
            
            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }

        public void UpgradeDefense()
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint, upgradeSquadDefenceStat.currentUpgradeCost)) return;
            if (attackUpgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;
            
            upgradeSquadDefenceStat.UpdateSquadStat();
            SetUpgradeUI(upgradeSquadDefenceStat);
            
            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }

        public void UpgradeCriticalDamage()
        {
            if (!AccountManager.Instance.SubtractCurrency(Enum.CurrencyType.StatPoint, upgradeSquadCriticalDamageStat.currentUpgradeCost)) return;
            if (attackUpgradeButton.GetComponent<HoldButton>().pauseUpgrade) return;
            
            upgradeSquadCriticalDamageStat.UpdateSquadStat();
            SetUpgradeUI(upgradeSquadCriticalDamageStat);
            
            // AchievementManager.Instance.IncreaseAchievementValue(Enum.AchieveType.Stat, 1);
        }
    }
}