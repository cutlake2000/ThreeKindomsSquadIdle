using System;
using Data;
using Function;
using Managers;
using Managers.BattleManager;
using UnityEngine;

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
        public void UpdateTotalStat(Enums.SquadStatType squadStatType, int adjustValue)
        {
            switch (squadStatType)
            {
                case Enums.SquadStatType.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAttack, adjustValue, percentAttack));
                    break;
                case Enums.SquadStatType.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseHealth, adjustValue, percentHealth));
                    break;
                case Enums.SquadStatType.Defence:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseDefense, adjustValue, percentDefence));
                    break;
                case Enums.SquadStatType.Penetration:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref basePenetration, adjustValue, percentPenetration));
                    break;
                case Enums.SquadStatType.Accuracy:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAccuracy, adjustValue, percentAccuracy));
                    break;
                case Enums.SquadStatType.CriticalRate:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseCriticalRate, adjustValue, baseCriticalRate));
                    break;
                case Enums.SquadStatType.CriticalDamage:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseCriticalDamage, adjustValue, baseCriticalDamage));
                    break;
                case Enums.SquadStatType.AcquisitionGold:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAcquisitionGold, adjustValue, baseAcquisitionGold));
                    break;
                case Enums.SquadStatType.AcquisitionExp:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAcquisitionExp, adjustValue, baseAcquisitionExp));
                    break;
            }
        }

        public void UpdateBaseStatFromSquadStatPanel(
            Enums.StatTypeFromSquadStatPanel statType, int adjustValue)
        {
            switch (statType)
            {
                case Enums.StatTypeFromSquadStatPanel.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Attack,
                        AdjustTotalStat(ref baseAttack, adjustValue, percentAttack));
                    break;
                case Enums.StatTypeFromSquadStatPanel.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Health,
                        AdjustTotalStat(ref baseHealth, adjustValue, percentHealth));
                    break;
                case Enums.StatTypeFromSquadStatPanel.Penetration:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Penetration,
                        AdjustTotalStat(ref basePenetration, adjustValue, percentPenetration));
                    break;
                case Enums.StatTypeFromSquadStatPanel.Accuracy:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Evasion,
                        AdjustTotalStat(ref baseAccuracy, adjustValue, percentAccuracy));
                    break;
                case Enums.StatTypeFromSquadStatPanel.AcquisitionGold:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.AcquisitionGold,
                        AdjustTotalStat(ref baseAcquisitionGold, adjustValue, percentAcquisitionGold));
                    break;
                case Enums.StatTypeFromSquadStatPanel.AcquisitionExp:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.AcquisitionExp,
                        AdjustTotalStat(ref baseAcquisitionExp, adjustValue, percentAcquisitionExp));
                    break;
                case Enums.StatTypeFromSquadStatPanel.CriticalDamage:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.CriticalDamage,
                        AdjustTotalStat(ref baseCriticalDamage, adjustValue, percentCriticalDamage));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        public void UpdatePercentStatFromSquadStatPanel(Enums.StatTypeFromSquadStatPanel statType, int adjustValue)
        {
            switch (statType)
            {
                case Enums.StatTypeFromSquadStatPanel.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Attack,
                        AdjustTotalStat(ref baseAttack, 0, percentAttack + adjustValue));
                    break;
                case Enums.StatTypeFromSquadStatPanel.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Health,
                        AdjustTotalStat(ref baseHealth, 0, percentHealth + adjustValue));
                    break;
                case Enums.StatTypeFromSquadStatPanel.Penetration:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Penetration,
                        AdjustTotalStat(ref basePenetration, 0, percentPenetration + adjustValue));
                    break;
                case Enums.StatTypeFromSquadStatPanel.Accuracy:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Evasion,
                        AdjustTotalStat(ref baseAccuracy, 0, percentAccuracy + adjustValue));
                    break;
                case Enums.StatTypeFromSquadStatPanel.AcquisitionGold:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.AcquisitionGold,
                        AdjustTotalStat(ref baseAcquisitionGold, 0, percentAcquisitionGold + adjustValue));
                    break;
                case Enums.StatTypeFromSquadStatPanel.AcquisitionExp:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.AcquisitionExp,
                        AdjustTotalStat(ref baseAcquisitionExp, 0, percentAcquisitionExp + adjustValue));
                    break;
                case Enums.StatTypeFromSquadStatPanel.CriticalDamage:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.CriticalDamage,
                        AdjustTotalStat(ref baseCriticalDamage, 0, percentCriticalDamage + adjustValue));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        public void UpdateBaseStatFromSquadConfigurePanel(Enums.StatTypeFromSquadConfigurePanel type, int adjustValue)
        {
            switch (type)
            {
                case Enums.StatTypeFromSquadConfigurePanel.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Attack,
                        AdjustTotalStat(ref baseAttack, adjustValue, percentAttack));
                    break;
                case Enums.StatTypeFromSquadConfigurePanel.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Health,
                        AdjustTotalStat(ref baseHealth, adjustValue, percentHealth));
                    break;
            }
        }

        public void UpdatePercentStatFromSquadConfigurePanel(Enums.StatTypeFromSquadConfigurePanel type, int adjustValue)
        {
            switch (type)
            {
                case Enums.StatTypeFromSquadConfigurePanel.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Attack,
                        AdjustTotalStat(ref baseAttack, 0, percentAttack + adjustValue));
                    break;
                case Enums.StatTypeFromSquadConfigurePanel.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Health,
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