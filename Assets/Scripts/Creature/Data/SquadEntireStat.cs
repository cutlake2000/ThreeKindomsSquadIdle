using System;
using Controller.UI.BottomMenuUI.PopUpUI;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers;
using Managers.BattleManager;
using Managers.GameManager;
using UnityEngine;

namespace Creature.Data
{
    [Serializable]
    public class SquadEntireStat : BaseStat
    {
        [Header("Total SquadStats")]
        [Header("직전 전투력")] public BigInteger PreviousTotalCombatPower;
        [Header("현재 전투력")] public BigInteger CurrentTotalCombatPower;
        
        [Header("깡옵")]
        [Tooltip("기본 공격력")] public int baseAttack;
        [Tooltip("기본 워리어 공격력")] public int baseWarriorAttack;
        [Tooltip("기본 아처 공격력")] public int baseArcherAttack;
        [Tooltip("기본 위자드 공격력")] public int baseWizardAttack;
        [Tooltip("기본 체력")] public int baseHealth;
        [Tooltip("기본 방어력")] public int baseDefense;
        [Tooltip("기본 관통력")] public int basePenetration;
        [Tooltip("기본 명중률")] public int baseAccuracy;
        [Tooltip("기본 치명타 확률")] public int baseCriticalRate;
        [Tooltip("기본 치명타 데미지")] public int baseCriticalDamage;
        [Tooltip("기본 골드 획득량")] public int baseAcquisitionGold;
        [Tooltip("기본 경험치 획득량")] public int baseAcquisitionExp;

        [Space(5)]
        [Header("깡옵")]
        public int percentAttack = 10000;
        public int percentWarriorAttack = 10000;
        public int percentArcherAttack = 10000;
        public int percentWizardAttack = 10000;
        public int percentHealth = 10000;
        public int percentDefence = 10000;
        public int percentCriticalRate = 10000;
        public int percentCriticalDamage = 10000;
        public int percentPenetration = 10000;
        public int percentAccuracy = 10000;
        public int percentAcquisitionGold = 10000;
        public int percentAcquisitionExp = 10000;

        public void UpdateStat(Enums.SquadStatType statType, int adjustValue, bool baseStat)
        {
            PreviousTotalCombatPower = SquadBattleManager.Instance.GetTotalCombatPower();
            
            switch (baseStat)
            {
                case true:
                    UpdateBaseStat(statType, adjustValue);
                    break;
                case false:
                    UpdatePercentStat(statType, adjustValue);
                    break;
            }
            
            CurrentTotalCombatPower = SquadBattleManager.Instance.GetTotalCombatPower();

            var variationValue = CurrentTotalCombatPower - PreviousTotalCombatPower;
            var plusMark = variationValue >= 0 ? true : false;
            
            if (GameManager.ReadyToLaunch) UIManager.Instance.popUpMessagePanelUI.GetComponent<PopUpMessagePanelUI>().UpdateTotalCombatPowerPopUpMessagePanelUI(CurrentTotalCombatPower, variationValue, plusMark);
        }

        public void UpdateBaseStat(Enums.SquadStatType statType, int adjustValue)
        {
            switch (statType)
            {
                case Enums.SquadStatType.WarriorAtk:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseWarriorAttack, adjustValue, ref percentWarriorAttack, 0));
                    break;
                case Enums.SquadStatType.ArcherAtk:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseArcherAttack, adjustValue, ref percentArcherAttack, 0));
                    break;
                case Enums.SquadStatType.WizardAtk:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseWizardAttack, adjustValue, ref percentWizardAttack, 0));
                    break;
                case Enums.SquadStatType.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseAttack, adjustValue, ref percentAttack, 0));
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseWarriorAttack, adjustValue, ref percentWarriorAttack, 0));
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseArcherAttack, adjustValue, ref percentArcherAttack, 0));
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseWizardAttack, adjustValue, ref percentWizardAttack, 0));
                    break;
                case Enums.SquadStatType.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseHealth, adjustValue, ref percentHealth, 0));
                    break;
                case Enums.SquadStatType.Defence:
                    SquadBattleManager.Instance.SetTotalSquadStat(Enums.SquadStatType.Defence, AdjustTotalStat(ref baseDefense, adjustValue, ref percentDefence, 0));
                    break;
                case Enums.SquadStatType.Penetration:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref basePenetration, adjustValue, ref percentPenetration, 0));
                    break;
                case Enums.SquadStatType.Accuracy:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseAccuracy, adjustValue, ref percentAccuracy, 0));
                    break;
                case Enums.SquadStatType.AcquisitionGold:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseAcquisitionGold, adjustValue, ref percentAcquisitionGold, 0));
                    break;
                case Enums.SquadStatType.AcquisitionExp:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseAcquisitionExp, adjustValue, ref percentAcquisitionExp, 0));
                    break;
                case Enums.SquadStatType.CriticalRate:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseCriticalRate, adjustValue, ref percentCriticalRate, 0));
                    break;
                case Enums.SquadStatType.CriticalDamage:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseCriticalDamage, adjustValue, ref percentCriticalDamage, 0));
                    break;
                case Enums.SquadStatType.WarriorHealth:
                case Enums.SquadStatType.WizardHealth:
                case Enums.SquadStatType.ArcherHealth:
                case Enums.SquadStatType.WarriorDefence:
                case Enums.SquadStatType.WizardDefence:
                case Enums.SquadStatType.ArcherDefence:
                case Enums.SquadStatType.Evasion:
                case Enums.SquadStatType.MoveSpeed:
                case Enums.SquadStatType.AmplificationSkillEffects:
                case Enums.SquadStatType.WarriorAttackRange:
                case Enums.SquadStatType.ArcherAttackRange:
                case Enums.SquadStatType.WizardAttackRange:
                case Enums.SquadStatType.FollowRange:
                default:
                    throw new ArgumentOutOfRangeException(nameof(statType), statType, null);
            }
        }

        public void UpdatePercentStat(Enums.SquadStatType statType, int adjustValue)
        {
            switch (statType)
            {
                case Enums.SquadStatType.WarriorAtk:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseWarriorAttack, 0, ref percentWarriorAttack, adjustValue));
                    break;
                case Enums.SquadStatType.ArcherAtk:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseArcherAttack, 0, ref percentArcherAttack, adjustValue));
                    break;
                case Enums.SquadStatType.WizardAtk:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseWizardAttack, 0, ref percentWizardAttack, adjustValue));
                    break;
                case Enums.SquadStatType.Attack:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseAttack, 0, ref percentAttack, adjustValue));
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseWarriorAttack, 0, ref percentWarriorAttack, adjustValue));
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseArcherAttack, 0, ref percentArcherAttack, adjustValue));
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseWizardAttack, 0, ref percentWizardAttack, adjustValue));
                    break;
                case Enums.SquadStatType.Health:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseHealth, 0, ref percentHealth, adjustValue));
                    break;
                case Enums.SquadStatType.Defence:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseDefense, 0, ref percentDefence, adjustValue));
                    break;
                case Enums.SquadStatType.Penetration:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref basePenetration, 0, ref percentPenetration, adjustValue));
                    break;
                case Enums.SquadStatType.Accuracy:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseAccuracy, 0, ref percentAccuracy, adjustValue));
                    break;
                case Enums.SquadStatType.AcquisitionGold:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseAcquisitionGold, 0, ref percentAcquisitionGold, adjustValue));
                    break;
                case Enums.SquadStatType.AcquisitionExp:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseAcquisitionExp, 0, ref percentAcquisitionExp, adjustValue));
                    break;
                case Enums.SquadStatType.CriticalRate:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseCriticalRate, 0, ref percentCriticalRate, adjustValue));
                    break;
                case Enums.SquadStatType.CriticalDamage:
                    SquadBattleManager.Instance.SetTotalSquadStat(statType, AdjustTotalStat(ref baseCriticalDamage, 0, ref percentCriticalDamage, adjustValue));
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
        
        // // 스텟 증가 메서드
        // public void UpdateTotalStat(Enums.SquadStatType squadStatType, int adjustValue)
        // {
        //     switch (squadStatType)
        //     {
        //         case Enums.SquadStatType.WarriorAtk:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseWarriorAttack, adjustValue, ref percentWarriorAttack, 0));
        //             break;
        //         case Enums.SquadStatType.ArcherAtk:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseArcherAttack, adjustValue, ref percentArcherAttack, 0));
        //             break;
        //         case Enums.SquadStatType.WizardAtk:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseWizardAttack, adjustValue, ref percentWizardAttack, 0));
        //             break;
        //         case Enums.SquadStatType.Attack:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseAttack, adjustValue, ref percentAttack, 0));
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseWarriorAttack, adjustValue, ref percentWarriorAttack, 0));
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseArcherAttack, adjustValue, ref percentArcherAttack, 0));
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseWizardAttack, adjustValue, ref percentWizardAttack, 0));
        //             break;
        //         case Enums.SquadStatType.Health:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseHealth, adjustValue, ref percentHealth, 0));
        //             break;
        //         case Enums.SquadStatType.Defence:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseDefense, adjustValue, ref percentDefence, 0));
        //             break;
        //         case Enums.SquadStatType.Penetration:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref basePenetration, adjustValue, ref percentPenetration, 0));
        //             break;
        //         case Enums.SquadStatType.Accuracy:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseAccuracy, adjustValue, ref percentAccuracy, 0));
        //             break;
        //         case Enums.SquadStatType.CriticalRate:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseCriticalRate, adjustValue, ref percentCriticalRate, 0));
        //             break;
        //         case Enums.SquadStatType.CriticalDamage:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseCriticalDamage, adjustValue, ref percentCriticalDamage, 0));
        //             break;
        //         case Enums.SquadStatType.AcquisitionGold:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseAcquisitionGold, adjustValue, ref percentAcquisitionGold, 0));
        //             break;
        //         case Enums.SquadStatType.AcquisitionExp:
        //             SquadBattleManager.Instance.SetTotalSquadStat(squadStatType, AdjustTotalStat(ref baseAcquisitionExp, adjustValue, ref percentAcquisitionExp, 0));
        //             break;
        //         case Enums.SquadStatType.WarriorHealth:
        //             break;
        //         case Enums.SquadStatType.WizardHealth:
        //             break;
        //         case Enums.SquadStatType.ArcherHealth:
        //             break;
        //         case Enums.SquadStatType.WarriorDefence:
        //             break;
        //         case Enums.SquadStatType.WizardDefence:
        //             break;
        //         case Enums.SquadStatType.ArcherDefence:
        //             break;
        //         case Enums.SquadStatType.Evasion:
        //             break;
        //         case Enums.SquadStatType.MoveSpeed:
        //             break;
        //         case Enums.SquadStatType.AmplificationSkillEffects:
        //             break;
        //         case Enums.SquadStatType.WarriorAttackRange:
        //             break;
        //         case Enums.SquadStatType.ArcherAttackRange:
        //             break;
        //         case Enums.SquadStatType.WizardAttackRange:
        //             break;
        //         case Enums.SquadStatType.FollowRange:
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(squadStatType), squadStatType, null);
        //     }
        // }
    }
}