using System;
using Function; 
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Enum = Data.Enum;

namespace Creature.Data
{
    [Serializable]
    public class SquadEntireStat : BaseStat
    {
        [Tooltip("기본 공격력")]
        public int baseAttack;
        [Tooltip("기본 체력")]
        public int baseHealth;
        [Tooltip("기본 방어력")]
        public int baseDefense;
        [Tooltip("기본 관통력")]
        public int basePenetration;
        [Tooltip("기본 명중률")]
        public int baseAccuracy;
        [Tooltip("기본 치명타 확률")]
        public int baseCriticalRate;
        [Tooltip("기본 치명타 데미지")]
        public int baseCriticalDamage;
        [Tooltip("기본 골드 획득량")]
        public int baseAcquisitionGold;
        [Tooltip("기본 골드 획득량")]
        public int baseAcquisitionExp;
        
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
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseAttack, adjustValue, percentAttack));
                    break;
                case Enum.SquadStatType.Health:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseHealth, adjustValue,percentHealth));
                    break;
                case Enum.SquadStatType.Defence:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseDefense, adjustValue, percentDefence));
                    break;
                case Enum.SquadStatType.Penetration:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref basePenetration, adjustValue, percentPenetration));
                    break;
                case Enum.SquadStatType.Accuracy:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseAccuracy, adjustValue, percentAccuracy));
                    break;
                case Enum.SquadStatType.CriticalRate:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseCriticalRate, adjustValue, baseCriticalRate));
                    break;
                case Enum.SquadStatType.CriticalDamage:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseCriticalDamage, adjustValue, baseCriticalDamage));
                    break;
                case Enum.SquadStatType.AcquisitionGold:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseAcquisitionGold, adjustValue, baseAcquisitionGold));
                    break;
                case Enum.SquadStatType.AcquisitionExp:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseAcquisitionExp, adjustValue, baseAcquisitionExp));
                    break;
            }
        }
        
        public void UpdateTotalStatBySquadStatPanel(Enum.SquadStatTypeBySquadPanel type, int adjustValue)
        {
            switch (type)
            {
                case Enum.SquadStatTypeBySquadPanel.Atk:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Attack, AdjustTotalStat(ref baseAttack, adjustValue, percentAttack));
                    break;
                case Enum.SquadStatTypeBySquadPanel.Hp:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Health, AdjustTotalStat(ref baseHealth, adjustValue, percentHealth));
                    break;
                case Enum.SquadStatTypeBySquadPanel.Penetration:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Penetration, AdjustTotalStat(ref basePenetration, adjustValue, percentPenetration));
                    break;
                case Enum.SquadStatTypeBySquadPanel.Accuracy:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Evasion, AdjustTotalStat(ref baseAccuracy, adjustValue, percentAccuracy));
                    break;
                case Enum.SquadStatTypeBySquadPanel.AcquisitionGold:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.AcquisitionGold, AdjustTotalStat(ref baseAcquisitionGold, adjustValue, percentAcquisitionGold));
                    break;
                case Enum.SquadStatTypeBySquadPanel.AcquisitionExp:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.AcquisitionExp, AdjustTotalStat(ref baseAcquisitionExp, adjustValue, percentAcquisitionExp));
                    break;
                case Enum.SquadStatTypeBySquadPanel.CrtDmg:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.CriticalDamage, AdjustTotalStat(ref baseCriticalDamage, adjustValue, percentCriticalDamage));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
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
            var calculateTotalStat = (baseStat * percent) / 100;
            
            return calculateTotalStat;
        }
    }
}