using System;
using Function;
using JetBrains.Annotations;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Enum = Data.Enum;

namespace Creature.Data
{
    [Serializable]
    public class SquadStat : BaseStat
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
        [SerializeField] private int currentBaseCriticalRate;
        [SerializeField] private int currentBaseCriticalDamage;
        [SerializeField] private int currentBasePenetration;
        [SerializeField] private int currentBaseAccuracy;
        [SerializeField] private int currentBaseAcquisitionGold;
        [SerializeField] private int currentBaseAcquisitionExp;
        
        [SerializeField] private int currentPercentAttack;
        [SerializeField] private int currentPercentHealth;
        [SerializeField] private int currentPercentDefence;
        [SerializeField] private int currentPercentCriticalRate;
        [SerializeField] private int currentPercentCriticalDamage;
        [SerializeField] private int currentPercentPenetration;
        [SerializeField] private int currentPercentAccuracy;
        [SerializeField] private int currentPercentAcquisitionGold;
        [SerializeField] private int currentPercentAcquisitionExp;

        // 스텟 증가 메서드
        public void IncreaseBaseStat(Enum.SquadStatType squadStatType, int increaseValue)
        {
            switch (squadStatType)
            {
                case Enum.SquadStatType.Attack:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseAttack, increaseValue, ref currentBaseAttack, currentPercentAttack));
                    break;
                case Enum.SquadStatType.Health:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseHealth, increaseValue, ref currentBaseHealth, currentPercentHealth));
                    break;
                case Enum.SquadStatType.Defence:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseDefense, increaseValue, ref currentBaseDefence, currentPercentDefence));
                    break;
                case Enum.SquadStatType.CriticalRate:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseCriticalRate, increaseValue, ref currentBaseCriticalRate, currentPercentCriticalRate));
                    break;
                case Enum.SquadStatType.CriticalDamage:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseCriticalDamage, increaseValue, ref currentBaseCriticalDamage, currentPercentCriticalDamage));
                    break;
                case Enum.SquadStatType.Penetration:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BasePenetration, increaseValue, ref currentBasePenetration, currentPercentPenetration));
                    break;
                case Enum.SquadStatType.Accuracy:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseAccuracy, increaseValue, ref currentBaseAccuracy, currentPercentAccuracy));
                    break;
            }
        }
        
        public void IncreaseBaseStatBySquadStatPanel(Enum.SquadStatPanelStatType type, int increaseValue)
        {
            switch (type)
            {
                case Enum.SquadStatPanelStatType.Atk:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Attack, IncreaseBaseStat(ref BaseAttack, increaseValue, ref currentBaseAttack, currentPercentAttack));
                    break;
                case Enum.SquadStatPanelStatType.Hp:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Health, IncreaseBaseStat(ref BaseHealth, increaseValue, ref currentBaseHealth, currentPercentHealth));
                    break;
                case Enum.SquadStatPanelStatType.Penetration:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Penetration, IncreaseBaseStat(ref BasePenetration, increaseValue, ref currentBasePenetration, currentBasePenetration));
                    break;
                case Enum.SquadStatPanelStatType.Accuracy:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.Evasion, IncreaseBaseStat(ref BaseAccuracy, increaseValue, ref currentBaseAccuracy, currentPercentAccuracy));
                    break;
                case Enum.SquadStatPanelStatType.AcquisitionGold:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.AcquisitionGold, IncreaseBaseStat(ref BaseAcquisitionGold, increaseValue, ref currentBaseAcquisitionGold, currentPercentAcquisitionGold));
                    break;
                case Enum.SquadStatPanelStatType.AcquisitionExp:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.AcquisitionExp, IncreaseBaseStat(ref BaseAcquisitionExp, increaseValue, ref currentBaseAcquisitionExp, currentPercentAcquisitionExp));
                    break;
                case Enum.SquadStatPanelStatType.CrtDmg:
                    SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.CriticalDamage, IncreaseBaseStat(ref BaseCriticalDamage, increaseValue, ref currentBaseCriticalDamage, currentPercentCriticalDamage));
                    break;
                //TODO: 추후 스쿼드 패널 스탯 추가할 경우 수정
                // case Enum.SquadStatPanelStatType.AmplificationSkillEffects:
                //     SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.AmplificationSkillEffects, IncreaseBaseStat(ref baAm, increaseValue, ref currentBaseAttack, currentPercentAttack));
                //     break;
                // case Enum.SquadStatPanelStatType.CurrentAtk:
                //     SquadManager.Instance.SetTotalSquadStat(Enum.SquadStatType.CurrentAtk, IncreaseBaseStat(ref BaseAttack, increaseValue, ref currentBaseAttack, currentPercentAttack));
                //     break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        // 스텟 감소 메서드
        public void DecreaseBaseStat(Enum.SquadStatType squadStatType, int subtractValue)
        {
            switch (squadStatType)
            {
                case Enum.SquadStatType.Attack:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseAttack, subtractValue, ref currentBaseAttack, currentPercentAttack));
                    break;
                case Enum.SquadStatType.Health:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseHealth, subtractValue, ref currentBaseHealth, currentPercentHealth));
                    break;
                case Enum.SquadStatType.Defence:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseDefense, subtractValue, ref currentBaseDefence, currentPercentDefence));
                    break;
                case Enum.SquadStatType.CriticalRate:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseCriticalRate, subtractValue, ref currentBaseCriticalRate, currentPercentCriticalRate));
                    break;
                case Enum.SquadStatType.CriticalDamage:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseCriticalDamage, subtractValue, ref currentBaseCriticalDamage, currentPercentCriticalDamage));
                    break;
                case Enum.SquadStatType.Penetration:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BasePenetration, subtractValue, ref currentBasePenetration, currentPercentPenetration));
                    break;
                case Enum.SquadStatType.Accuracy:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreaseBaseStat(ref BaseHealth, subtractValue, ref currentBaseAccuracy, currentPercentAccuracy));
                    break;
            }
        }

        // 스텟 퍼센트 증가 메서드
        public void IncreasePercentStat(Enum.SquadStatType squadStatType, int addPercent)
        {
            switch (squadStatType)
            {
                case Enum.SquadStatType.Attack:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentAttack, addPercent, ref currentBaseAttack, BaseAttack));
                    break;
                case Enum.SquadStatType.Health:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentHealth, addPercent, ref currentBaseHealth, BaseHealth));
                    break;
                case Enum.SquadStatType.Defence:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentDefence, addPercent, ref currentBaseDefence, BaseDefense));
                    break;
                case Enum.SquadStatType.CriticalRate:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentCriticalRate, addPercent, ref currentBaseCriticalRate, BaseCriticalRate));
                    break;
                case Enum.SquadStatType.CriticalDamage:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentCriticalDamage, addPercent, ref currentBaseCriticalDamage, BaseCriticalDamage));
                    break;
                case Enum.SquadStatType.Penetration:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentPenetration, addPercent, ref currentBasePenetration, BasePenetration));
                    break;
                case Enum.SquadStatType.Accuracy:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentAccuracy, addPercent, ref currentBaseAccuracy, BaseAccuracy));
                    break;
            }
        }

        // 스텟 퍼센트 감소 메서드
        public void DecreasePercentStat(Enum.SquadStatType squadStatType, int subtractPercent)
        {
            switch (squadStatType)
            {
                case Enum.SquadStatType.Attack:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentAttack, subtractPercent, ref currentBaseAttack, BaseAttack));
                    break;
                case Enum.SquadStatType.Health:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentHealth, subtractPercent, ref currentBaseHealth, BaseHealth));
                    break;
                case Enum.SquadStatType.Defence:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentDefence, subtractPercent, ref currentBaseDefence, BaseDefense));
                    break;
                case Enum.SquadStatType.CriticalRate:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentCriticalRate, subtractPercent, ref currentBaseCriticalRate, BaseCriticalRate));
                    break;
                case Enum.SquadStatType.CriticalDamage:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentCriticalDamage, subtractPercent, ref currentBaseCriticalDamage, BaseCriticalDamage));
                    break;
                case Enum.SquadStatType.Penetration:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentPenetration, subtractPercent, ref currentBasePenetration, BasePenetration));
                    break;
                case Enum.SquadStatType.Accuracy:
                    SquadManager.Instance.SetTotalSquadStat(squadStatType, IncreasePercentStat(ref currentPercentAccuracy, subtractPercent, ref currentBaseAccuracy, BaseAccuracy));
                    break;
            }
        }

        // 스탯 깡옵 증가
        private BigInteger IncreaseBaseStat(ref int baseStat, int addValue, ref BigInteger currentValue, int percent)
        {
            baseStat += addValue;
            return CalculateSetTotalSquadStat(baseStat, percent, ref currentValue);
        }
        
        private int IncreaseBaseStat(ref int baseStat, int addValue, ref int currentValue, int percent)
        {
            baseStat += addValue;
            return CalculateSetTotalSquadStat(baseStat, percent, ref currentValue);
        }
        

        // 스탯 깡옵 감소
        private BigInteger DecreaseBaseStat(ref int baseStat, int subtractValue, ref BigInteger currentValue, int percent)
        {
            baseStat -= subtractValue;
            return CalculateSetTotalSquadStat(baseStat, percent, ref currentValue);
        }
        
        private int DecreaseBaseStat(ref int baseStat, int subtractValue, ref int currentValue, int percent)
        {
            baseStat -= subtractValue;
            return CalculateSetTotalSquadStat(baseStat, percent, ref currentValue);
        }

        // 스텟 퍼옵 증가
        private BigInteger IncreasePercentStat(ref int percent, int addPercentValue, ref BigInteger currentValue, int baseStat)
        {
            percent += addPercentValue;
            return CalculateSetTotalSquadStat(baseStat, percent, ref currentValue);
        }
        
        private int IncreasePercentStat(ref int percent, int addPercentValue, ref int currentValue, int baseStat)
        {
            percent += addPercentValue;
            return CalculateSetTotalSquadStat(baseStat, percent, ref currentValue);
        }

        // 스텟 퍼옵 감소
        private BigInteger DecreasePercentStat(ref int percent, int subtractPercentValue, ref BigInteger currentValue, int baseStat)
        {
            percent -= subtractPercentValue;
            return CalculateSetTotalSquadStat(baseStat, percent, ref currentValue);
        }
        
        private int DecreasePercentStat(ref int percent, int subtractPercentValue, ref int currentValue, int baseStat)
        {
            percent -= subtractPercentValue;
            return CalculateSetTotalSquadStat(baseStat, percent, ref currentValue);
        }

        // 총 공격력 합산 메서드
        private static BigInteger CalculateSetTotalSquadStat(BigInteger baseStat, int percent, ref BigInteger currentValue)
        {
            var calculateTotalStat = (baseStat * percent) / 100;
            
            return calculateTotalStat;
        }
        
        private static int CalculateSetTotalSquadStat(int baseStat, int percent, ref int currentValue)
        {
            var calculateTotalStat = (baseStat * percent) / 100;
            
            return calculateTotalStat;
        }
    }
}