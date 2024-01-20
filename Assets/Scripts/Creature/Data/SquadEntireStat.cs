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
        [Tooltip("기본 관통력")]
        public int BasePenetration;
        [Tooltip("기본 명중률")]
        public int BaseAccuracy;
        [Tooltip("기본 치명타 확률")]
        public int BaseCriticalRate;
        [Tooltip("기본 치명타 데미지")]
        public int BaseCriticalDamage;
        [Tooltip("기본 골드 획득량")]
        public int BaseAcquisitionGold;
        [Tooltip("기본 골드 획득량")]
        public int BaseAcquisitionExp;
        
        [SerializeField] private BigInteger currentBaseAttack = 0;
        [SerializeField] private BigInteger currentBaseHealth = 0;
        [SerializeField] private BigInteger currentBaseDefence = 0;
        [SerializeField] private BigInteger currentBaseCriticalRate;
        [SerializeField] private BigInteger currentBaseCriticalDamage;
        [SerializeField] private BigInteger currentBasePenetration;
        [SerializeField] private BigInteger currentBaseAccuracy;
        [SerializeField] private BigInteger currentBaseAcquisitionGold;
        [SerializeField] private BigInteger currentBaseAcquisitionExp;
        
        [SerializeField] private BigInteger currentPercentAttack;
        [SerializeField] private BigInteger currentPercentHealth;
        [SerializeField] private BigInteger currentPercentDefence;
        [SerializeField] private BigInteger currentPercentCriticalRate;
        [SerializeField] private BigInteger currentPercentCriticalDamage;
        [SerializeField] private BigInteger currentPercentPenetration;
        [SerializeField] private BigInteger currentPercentAccuracy;
        [SerializeField] private BigInteger currentPercentAcquisitionGold;
        [SerializeField] private BigInteger currentPercentAcquisitionExp;

         // 스텟 증가 메서드
        public void UpdateTotalStat(Enum.SquadStatType squadStatType, int increaseValue)
        {
            switch (squadStatType)
            {
                case Enum.SquadStatType.Attack:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref BaseAttack, increaseValue, ref currentBaseAttack, currentPercentAttack));
                    break;
                case Enum.SquadStatType.Health:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref BaseHealth, increaseValue, ref currentBaseHealth, currentPercentHealth));
                    break;
                case Enum.SquadStatType.Defence:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref BaseDefense, increaseValue, ref currentBaseDefence, currentPercentDefence));
                    break;
                case Enum.SquadStatType.Penetration:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref BasePenetration, increaseValue, ref currentBasePenetration, currentPercentPenetration));
                    break;
                case Enum.SquadStatType.Accuracy:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref BaseAccuracy, increaseValue, ref currentBaseAccuracy, currentPercentAccuracy));
                    break;
                case Enum.SquadStatType.CriticalRate:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref BaseCriticalRate, increaseValue, ref currentBaseCriticalRate, BaseCriticalRate));
                    break;
                case Enum.SquadStatType.CriticalDamage:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref BaseCriticalDamage, increaseValue, ref currentBaseCriticalDamage, BaseCriticalDamage));
                    break;
                case Enum.SquadStatType.AcquisitionGold:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref BaseAcquisitionGold, increaseValue, ref currentBaseAcquisitionGold, BaseAcquisitionGold));
                    break;
                case Enum.SquadStatType.AcquisitionExp:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref BaseAcquisitionExp, increaseValue, ref currentBaseAcquisitionExp, BaseAcquisitionExp));
                    break;
            }
        }
        
        public void UpdateTotalStatBySquadStatPanel(Enum.SquadStatTypeBySquadPanel type, int increaseValue)
        {
            switch (type)
            {
                case Enum.SquadStatTypeBySquadPanel.Atk:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Attack, AdjustTotalStat(ref BaseAttack, increaseValue, ref currentBaseAttack, currentPercentAttack));
                    break;
                case Enum.SquadStatTypeBySquadPanel.Hp:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Health, AdjustTotalStat(ref BaseHealth, increaseValue, ref currentBaseHealth, currentPercentHealth));
                    break;
                case Enum.SquadStatTypeBySquadPanel.Penetration:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Penetration, AdjustTotalStat(ref BasePenetration, increaseValue, ref currentBasePenetration, currentBasePenetration));
                    break;
                case Enum.SquadStatTypeBySquadPanel.Accuracy:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Evasion, AdjustTotalStat(ref BaseAccuracy, increaseValue, ref currentBaseAccuracy, currentPercentAccuracy));
                    break;
                case Enum.SquadStatTypeBySquadPanel.AcquisitionGold:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.AcquisitionGold, AdjustTotalStat(ref BaseAcquisitionGold, increaseValue, ref currentBaseAcquisitionGold, currentPercentAcquisitionGold));
                    break;
                case Enum.SquadStatTypeBySquadPanel.AcquisitionExp:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.AcquisitionExp, AdjustTotalStat(ref BaseAcquisitionExp, increaseValue, ref currentBaseAcquisitionExp, currentPercentAcquisitionExp));
                    break;
                case Enum.SquadStatTypeBySquadPanel.CrtDmg:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.CriticalDamage, AdjustTotalStat(ref BaseCriticalDamage, increaseValue, ref currentBaseCriticalDamage, currentPercentCriticalDamage));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        // 스탯 깡옵 증가
        private BigInteger AdjustTotalStat(ref int baseStat, int addValue, ref BigInteger currentValue, BigInteger percent)
        {
            baseStat += addValue;
            return SetTotalSquadStat(baseStat, percent, ref currentValue);
        }

        // 총 공격력 합산 메서드
        private BigInteger SetTotalSquadStat(BigInteger baseStat, BigInteger percent, ref BigInteger currentValue)
        {
            var calculateTotalStat = (baseStat * percent) / 100;
            
            return calculateTotalStat;
        }
    }
}