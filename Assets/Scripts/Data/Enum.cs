namespace Data
{
    public static class Enum
    {
        public enum AchievementProperty
        {
            Name,
            Description,
            Level,
            CurrentCumulativeCount,
            MaxCumulativeCount,
            Type,
            RewardType,
            RewardValue
        }

        // 업적 보상 종류
        public enum AchieveRewardType
        {
            Gold,
            Dia,
            EnhanceStone,
            Atk
        }

        // 업적 종류
        public enum AchieveType
        {
            Stat,
            Summon,
            Enhance
        }

        public enum CreatureClassType
        {
            Squad,
            Monster
        }

        public enum CurrencyType
        {
            StatPoint,
            Gold,
            Dia,
            EnhanceStone
        }

        // 장비 능력치
        public enum EquipmentProperty
        {
            Name,
            Quantity,
            Level,
            Equipped,
            Type,
            Rarity,
            EnhancementLevel,
            BasicEquippedEffect,
            BasicOwnedEffect,
            EquippedEffect,
            OwnedEffect
        }

        /// <summary>
        /// 장비 레어도
        /// </summary>
        public enum EquipmentRarity
        {
            Common,
            Uncommon,
            Magic,
            Rare,
            Unique,
            Legend,
            Epic,
            Ancient,
            Legendary,
            Mythology,
            Null
        }

        /// <summary>
        /// 장비 종류
        /// </summary>
        public enum EquipmentType
        {
            Sword,
            Bow,
            Staff,
            Helmet,
            Armor,
            Gauntlet,
            Null
        }

        public enum LevelType
        {
            CurrentLv,
            CurrentExp,
            MaxLv,
            MaxExp
        }

        public enum MonsterClassType
        {
            Human,
            Indian
        }

        public enum PoolType
        {
            ProjectileBaseAttackWarrior,
            ProjectileBaseAttackArcher,
            ProjectileBaseAttackWizard,
            ProjectileSkillAttackWarrior,
            ProjectileSkillAttackArcher,
            ProjectileSkillAttackWizard,
            ProjectileBaseAttackEnemy,
            EffectDamage
        }

        public enum SquadClassType
        {
            Warrior = 0,
            Archer,
            Wizard
        }

        public enum SquadStatPanelStatType
        {
            Atk,                        // 공격력
            Hp,                         // 체력
            Penetration,                // 관통
            Accuracy,                    // 명중
            AcquisitionGold,            // 골드 획득량
            AcquisitionExp,             // 경험치 증가량
            CrtDmg,                     // 치명타 피해량
            AmplificationSkillEffects,  // 스킬 효과 증폭
            CurrentAtk,                 // 최종 공격력
        }

        public enum SquadStatType
        {
            WarriorAtk,                 // 전사 공격력
            WizardAtk,                  // 마법사 공격력
            ArcherAtk,                  // 궁수 공격력
            Health,                         // 체력 (Health Points)
            Attack,                        // 공격력 (Attack)
            Defence,                        // 방어력 (Defense)
            CriticalRate,                        // 치명타 확률 (Critical Hit Rate)
            CriticalDamage,                     // 치명타 피해량 (Critical Hit Damage)
            As,                         // 공격 속도 (Attack Speed)
            Accuracy,                   // 명중률 (Accuracy)
            Penetration,                // 관통 (Penetration)
            Evasion,                    // 회피율 (Evasion
            MagicPower,                 // 마법력 (Magic Power)
            Resistance,                 // 저항력 (Resistance)
            MoveSpeed,                  // 이동 속도 (Movement Speed)
            AcquisitionGold,            // 골드 획득량
            AcquisitionExp,             // 경험치 획득량
            AmplificationSkillEffects,  // 스킬 효과 증폭
            CurrentAtk,
            
            WarriorAttackRange,
            ArcherAttackRange,
            WizardAttackRange,
            FollowRange
        }

        public enum SummonEquipmentType
        {
            Weapon,
            Gear,
            A
        }

        public enum UISliderType
        {
            CurrentWaveSlider
        }

        public enum UITextType
        {
            CurrentStageName,
            CurrentWave,
            Timer
        }

        public static readonly EquipmentType[] equipmentTypes =
        {
            EquipmentType.Sword,
            EquipmentType.Bow,
            EquipmentType.Staff,
            EquipmentType.Helmet,
            EquipmentType.Armor,
            EquipmentType.Gauntlet
        };

        public static readonly EquipmentRarity[] equipmentRarities =
        {
            EquipmentRarity.Common,
            EquipmentRarity.Uncommon,
            EquipmentRarity.Magic,
            EquipmentRarity.Rare,
            EquipmentRarity.Unique
            // Rarity.Legend,
            // Rarity.Epic,
            // Rarity.Ancient,
            // Rarity.Legendary,
            // Rarity.Mythology,
        };
    }
}