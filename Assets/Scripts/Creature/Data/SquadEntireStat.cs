using System;
using Data;
using Function;
using Managers;
using Managers.BattleManager;
using Managers.GameManager;
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

        [SerializeField] private int percentAttack = 10000;
        [SerializeField] private int percentHealth = 10000;
        [SerializeField] private int percentDefence = 10000;
        [SerializeField] private int percentCriticalRate = 10000;
        [SerializeField] private int percentCriticalDamage = 10000;
        [SerializeField] private int percentPenetration = 10000;
        [SerializeField] private int percentAccuracy = 10000;
        [SerializeField] private int percentAcquisitionGold = 10000;
        [SerializeField] private int percentAcquisitionExp = 10000;

        // 스텟 증가 메서드
        public void UpdateTotalStat(Enums.SquadStatType squadStatType, int adjustValue)
        {
            switch (squadStatType)
            {
                case Enums.SquadStatType.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAttack, adjustValue, ref percentAttack, 0));
                    break;
                case Enums.SquadStatType.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseHealth, adjustValue, ref percentHealth, 0));
                    break;
                case Enums.SquadStatType.Defence:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseDefense, adjustValue, ref percentDefence, 0));
                    break;
                case Enums.SquadStatType.Penetration:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref basePenetration, adjustValue, ref percentPenetration, 0));
                    break;
                case Enums.SquadStatType.Accuracy:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAccuracy, adjustValue, ref percentAccuracy, 0));
                    break;
                case Enums.SquadStatType.CriticalRate:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseCriticalRate, adjustValue, ref percentCriticalRate, 0));
                    break;
                case Enums.SquadStatType.CriticalDamage:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseCriticalDamage, adjustValue, ref percentCriticalDamage, 0));
                    break;
                case Enums.SquadStatType.AcquisitionGold:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAcquisitionGold, adjustValue, ref percentAcquisitionGold, 0));
                    break;
                case Enums.SquadStatType.AcquisitionExp:
                    SquadBattleManager.Instance.SetTotalSquadStat(squadStatType,
                        AdjustTotalStat(ref baseAcquisitionExp, adjustValue, ref percentAcquisitionExp, 0));
                    break;
            }
        }

        public void UpdateStat(Enums.SquadStatType statType, int adjustValue, bool baseStat)
        {
            switch (baseStat)
            {
                case true:
                    UpdateBaseStat(statType, adjustValue);
                    break;
                case false:
                    UpdatePercentStat(statType, adjustValue);
                    break;
            }
        }

        public void UpdateBaseStat(
            Enums.SquadStatType statType, int adjustValue)
        {
            switch (statType)
            {
                case Enums.SquadStatType.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Attack,
                        AdjustTotalStat(ref baseAttack, adjustValue, ref percentAttack, 0));
                    break;
                case Enums.SquadStatType.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Health,
                        AdjustTotalStat(ref baseHealth, adjustValue, ref percentHealth, 0));
                    break;
                case Enums.SquadStatType.Defence:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Defence,
                        AdjustTotalStat(ref baseHealth, adjustValue, ref percentHealth, 0));
                    break;
                case Enums.SquadStatType.Penetration:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Penetration,
                        AdjustTotalStat(ref basePenetration, adjustValue, ref percentPenetration, 0));
                    break;
                case Enums.SquadStatType.Accuracy:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Evasion,
                        AdjustTotalStat(ref baseAccuracy, adjustValue, ref percentAccuracy, 0));
                    break;
                case Enums.SquadStatType.AcquisitionGold:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.AcquisitionGold,
                        AdjustTotalStat(ref baseAcquisitionGold, adjustValue, ref percentAcquisitionGold, 0));
                    break;
                case Enums.SquadStatType.AcquisitionExp:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.AcquisitionExp,
                        AdjustTotalStat(ref baseAcquisitionExp, adjustValue, ref percentAcquisitionExp, 0));
                    break;
                case Enums.SquadStatType.CriticalDamage:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.CriticalDamage,
                        AdjustTotalStat(ref baseCriticalDamage, adjustValue, ref percentCriticalDamage, 0));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        public void UpdatePercentStat(Enums.SquadStatType statType, int adjustValue)
        {
            switch (statType)
            {
                case Enums.SquadStatType.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Attack,
                        AdjustTotalStat(ref baseAttack, 0, ref percentAttack, adjustValue));
                    break;
                case Enums.SquadStatType.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Health,
                        AdjustTotalStat(ref baseHealth, 0, ref percentHealth, adjustValue));
                    break;
                case Enums.SquadStatType.Penetration:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Penetration,
                        AdjustTotalStat(ref basePenetration, 0, ref percentPenetration, adjustValue));
                    break;
                case Enums.SquadStatType.Accuracy:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Evasion,
                        AdjustTotalStat(ref baseAccuracy, 0, ref percentAccuracy, adjustValue));
                    break;
                case Enums.SquadStatType.AcquisitionGold:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.AcquisitionGold,
                        AdjustTotalStat(ref baseAcquisitionGold, 0, ref percentAcquisitionGold, adjustValue));
                    break;
                case Enums.SquadStatType.AcquisitionExp:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.AcquisitionExp,
                        AdjustTotalStat(ref baseAcquisitionExp, 0, ref percentAcquisitionExp, adjustValue));
                    break;
                case Enums.SquadStatType.CriticalRate:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.CriticalRate,
                        AdjustTotalStat(ref baseHealth, 0, ref percentHealth, adjustValue));
                    break;
                case Enums.SquadStatType.CriticalDamage:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.CriticalDamage,
                        AdjustTotalStat(ref baseCriticalDamage, 0, ref percentCriticalDamage, adjustValue));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        // 스탯 깡옵 증가
        private BigInteger AdjustTotalStat(ref int baseStat, int addBaseValue, ref int percent, int addPercentValue)
        {
            baseStat += addBaseValue;
            percent += addPercentValue;
            return SetTotalSquadStat(baseStat, percent);
        }

        // 총 공격력 합산 메서드
        private BigInteger SetTotalSquadStat(BigInteger baseStat, int percent)
        {
            var calculateTotalStat = baseStat * percent / 10000;

            return calculateTotalStat;
        }
    }
}