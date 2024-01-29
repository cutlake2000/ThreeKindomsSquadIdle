using System;
using Function;
using Managers;
using UnityEngine;
using Enum = Data.Enum;

namespace Creature.Data
{
    [Serializable]
    public class SquadEntireStat : BaseStat
    {
        [Tooltip("기본 공격력")] public int baseAttack;

        [Tooltip("기본 체력")] public int baseHealth;

        [Tooltip("기본 방어력")] public int baseDefense;

        [Tooltip("기본 관통력")] public int basePenetration;

        [Tooltip("기본 명중률")] public int baseAccuracy;

        [Tooltip("기본 치명타 확률")] public int baseCriticalRate;

        [Tooltip("기본 치명타 데미지")] public int baseCriticalDamage;

        [Tooltip("기본 골드 획득량")] public int baseAcquisitionGold;

        [Tooltip("기본 골드 획득량")] public int baseAcquisitionExp;

        [SerializeField] private int percentAttack = 100;
        [SerializeField] private int percentHealth = 100;
        [SerializeField] private int percentDefence = 100;
        [SerializeField] private int percentCriticalRate = 100;
        [SerializeField] private int percentCriticalDamage = 100;
        [SerializeField] private int percentPenetration = 100;
        [SerializeField] private int percentAccuracy = 100;
        [SerializeField] private int percentAcquisitionGold = 100;
        [SerializeField] private int percentAcquisitionExp = 100;

        // 스텟 증가 메서드
        public void UpdateTotalStat(Enum.SquadStatType squadStatType, int adjustValue)
        {
            switch (squadStatType)
            {
                case Enum.SquadStatType.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAttack, adjustValue, percentAttack));
                    break;
                case Enum.SquadStatType.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseHealth, adjustValue, percentHealth));
                    break;
                case Enum.SquadStatType.Defence:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseDefense, adjustValue, percentDefence));
                    break;
                case Enum.SquadStatType.Penetration:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref basePenetration, adjustValue, percentPenetration));
                    break;
                case Enum.SquadStatType.Accuracy:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAccuracy, adjustValue, percentAccuracy));
                    break;
                case Enum.SquadStatType.CriticalRate:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseCriticalRate, adjustValue, baseCriticalRate));
                    break;
                case Enum.SquadStatType.CriticalDamage:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseCriticalDamage, adjustValue, baseCriticalDamage));
                    break;
                case Enum.SquadStatType.AcquisitionGold:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAcquisitionGold, adjustValue, baseAcquisitionGold));
                    break;
                case Enum.SquadStatType.AcquisitionExp:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAcquisitionExp, adjustValue, baseAcquisitionExp));
                    break;
            }
        }

        public void UpdateBaseStatFromSquadStatPanelBySquadStatFromSquadStatPanelPanel(
            Enum.StatTypeBySquadStatPanel statType, int adjustValue)
        {
            switch (statType)
            {
                case Enum.StatTypeBySquadStatPanel.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Attack,
                        AdjustTotalStat(ref baseAttack, adjustValue, percentAttack));
                    break;
                case Enum.StatTypeBySquadStatPanel.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Health,
                        AdjustTotalStat(ref baseHealth, adjustValue, percentHealth));
                    break;
                case Enum.StatTypeBySquadStatPanel.Penetration:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Penetration,
                        AdjustTotalStat(ref basePenetration, adjustValue, percentPenetration));
                    break;
                case Enum.StatTypeBySquadStatPanel.Accuracy:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Evasion,
                        AdjustTotalStat(ref baseAccuracy, adjustValue, percentAccuracy));
                    break;
                case Enum.StatTypeBySquadStatPanel.AcquisitionGold:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.AcquisitionGold,
                        AdjustTotalStat(ref baseAcquisitionGold, adjustValue, percentAcquisitionGold));
                    break;
                case Enum.StatTypeBySquadStatPanel.AcquisitionExp:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.AcquisitionExp,
                        AdjustTotalStat(ref baseAcquisitionExp, adjustValue, percentAcquisitionExp));
                    break;
                case Enum.StatTypeBySquadStatPanel.CriticalDamage:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.CriticalDamage,
                        AdjustTotalStat(ref baseCriticalDamage, adjustValue, percentCriticalDamage));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        public void UpdatePercentStatBySquadStatPanel(Enum.StatTypeBySquadStatPanel statType, int adjustValue)
        {
            switch (statType)
            {
                case Enum.StatTypeBySquadStatPanel.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Attack,
                        AdjustTotalStat(ref baseAttack, 0, percentAttack + adjustValue));
                    break;
                case Enum.StatTypeBySquadStatPanel.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Health,
                        AdjustTotalStat(ref baseHealth, 0, percentHealth + adjustValue));
                    break;
                case Enum.StatTypeBySquadStatPanel.Penetration:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Penetration,
                        AdjustTotalStat(ref basePenetration, 0, percentPenetration + adjustValue));
                    break;
                case Enum.StatTypeBySquadStatPanel.Accuracy:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Evasion,
                        AdjustTotalStat(ref baseAccuracy, 0, percentAccuracy + adjustValue));
                    break;
                case Enum.StatTypeBySquadStatPanel.AcquisitionGold:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.AcquisitionGold,
                        AdjustTotalStat(ref baseAcquisitionGold, 0, percentAcquisitionGold + adjustValue));
                    break;
                case Enum.StatTypeBySquadStatPanel.AcquisitionExp:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.AcquisitionExp,
                        AdjustTotalStat(ref baseAcquisitionExp, 0, percentAcquisitionExp + adjustValue));
                    break;
                case Enum.StatTypeBySquadStatPanel.CriticalDamage:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.CriticalDamage,
                        AdjustTotalStat(ref baseCriticalDamage, 0, percentCriticalDamage + adjustValue));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        public void UpdateBaseStatBySquadConfigurePanel(Enum.StatTypeBySquadConfigurePanel type, int adjustValue)
        {
            switch (type)
            {
                case Enum.StatTypeBySquadConfigurePanel.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Attack,
                        AdjustTotalStat(ref baseAttack, adjustValue, percentAttack));
                    break;
                case Enum.StatTypeBySquadConfigurePanel.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Health,
                        AdjustTotalStat(ref baseHealth, adjustValue, percentHealth));
                    break;
            }
        }

        public void UpdatePercentStatBySquadConfigurePanel(Enum.StatTypeBySquadConfigurePanel type, int adjustValue)
        {
            switch (type)
            {
                case Enum.StatTypeBySquadConfigurePanel.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Attack,
                        AdjustTotalStat(ref baseAttack, 0, percentAttack + adjustValue));
                    break;
                case Enum.StatTypeBySquadConfigurePanel.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Health,
                        AdjustTotalStat(ref baseHealth, 0, percentHealth + adjustValue));
                    break;
            }
        }

        // 스탯 깡옵 증가
        private BigInteger AdjustTotalStat(ref int baseStat, int addValue, BigInteger percent)
        {
            baseStat += addValue;
            return SetTotalSquadStat(baseStat, percent);
        }

        // 총 공격력 합산 메서드
        private BigInteger SetTotalSquadStat(BigInteger baseStat, BigInteger percent)
        {
            var calculateTotalStat = baseStat * percent / 100;

            return calculateTotalStat;
        }
    }
}