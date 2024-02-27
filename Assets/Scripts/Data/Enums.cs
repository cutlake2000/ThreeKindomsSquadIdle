namespace Data
{
    public static class Enums
    {
        public enum RewardType
        {
            Dia,
            Gold,
            Exp,
            Rare_5_Sword,
            OfflineReward
        }

        public enum LockButtonType
        {
            TalentPanel,
            InventoryPanel,
            SquadConfigurePanel,
            SummonPanel,
            SummonCharacterPanel,
            SummonWeaponPanel,
            SummonGearPanel,
            DungeonPanel,
            GoldDungeonPanel,
            SquadEnhanceStoneDungeonPanel,
            WillBeUpdate,
            NotEnoughSquadEnhanceStone,
            NotEnoughExp,
            NotEnoughGold,
            NotEnoughStatPoint,
            AlreadyAutoEquip,
            AlreadyAllComposite,
            NotEnoughDungeonTicket,
            NotEnoughEquipmentEnhanceStone,
            EquipmentEnhanceStoneDungeonPanel
        }
        
        public static readonly LockButtonType[] lockButtonTypes =
        {
            LockButtonType.TalentPanel,
            LockButtonType.InventoryPanel,
            LockButtonType.SquadConfigurePanel,
            LockButtonType.SummonPanel,
            LockButtonType.SummonCharacterPanel,
            LockButtonType.SummonWeaponPanel,
            LockButtonType.SummonGearPanel,
            LockButtonType.DungeonPanel,
            LockButtonType.GoldDungeonPanel,
            LockButtonType.SquadEnhanceStoneDungeonPanel,
            LockButtonType.WillBeUpdate,
            LockButtonType.NotEnoughSquadEnhanceStone,
            LockButtonType.NotEnoughExp,
            LockButtonType.NotEnoughGold,
            LockButtonType.NotEnoughStatPoint,
            LockButtonType.AlreadyAutoEquip,
            LockButtonType.AlreadyAllComposite,
            LockButtonType.NotEnoughDungeonTicket
        };

        public enum OpenContent
        {
            None,
            TalentPanel,
            InventoryPanel,
            SquadConfigurePanel,
            SummonWeaponPanel,
            SummonGearPanel,
            SummonCharacterPanel,
            GoldDungeonPanel,
            SquadEnhanceStoneDungeonPanel,
            EquipmentEnhanceStoneDungeonPanel
        }
        
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

        public enum IconType
        {
            SquadType_Archer,
            EquipmentType_Helmet,
            SquadType_Wizard,
            DungeonKeyType_EnhanceStoneGear,
            Temp1,
            SquadType_Warrior,
            EquipmentType_Staff,
            EquipmentType_Bow,
            DungeonKeyType_Gold,
            CurrencyType_EnhanceStoneSquad,
            DungeonKeyType_EnhanceStoneSquad,
            EquipmentType_Armor,
            EquipmentType_Sword,
            EquipmentType_Gauntlet,
            Temp2,
            CurrencyType_Gold,
            CurrencyType_Dia,
            CurrencyType_Exp,
            CurrencyType_EnhanceStoneEquipment
        }

        public enum CharacterRarity
        {
            Magic,
            Rare,
            Unique,
            Legend
        }

        public enum CharacterType
        {
            Warrior = 0,
            Archer,
            Wizard
        }

        public enum CreatureClassType
        {
            Squad,
            Monster
        }

        public enum CurrencyType
        {
            Gold,
            Dia,
            StatPoint,
            EquipmentEnhanceStone,
            SquadEnhanceStone,
            GoldDungeonTicket,
            SquadEnhanceStoneDungeonTicket,
            Exp,
            EquipmentEnhanceStoneDungeonTicket
        }

        public enum QuestType
        {
            AttackTalentLevel,
            HealthTalentLevel,
            AutoEquipSword,
            AutoEquipBow,
            AutoEquipStaff,
            AutoEquipHelmet,
            AutoEquipArmor,
            AutoEquipGauntlet,
            SummonWeapon10,
            SummonGear10,
            SummonSquad10,
            SummonWeapon100,
            SummonGear100,
            StageClear,
            EquipSquad,
            PlayGoldDungeon,
            PlaySquadEnhanceStoneDungeon,
            CompositeSword,
            CompositeBow,
            CompositeStaff,
            CompositeHelmet,
            CompositeArmor,
            CompositeGauntlet,
            LevelUpCharacter,
            LevelUpSquad,
            InitialQuest,
            TouchChallengeButton,
            TouchLoopButton,
            ArcherCamera,
            WarriorCamera,
            TouchAutoSkillButton,
            LevelUpEquipment,
            PlayEquipmentEnhanceStoneDungeon
        }
        
        public enum QuestRewardType
        {
            Gold,
            Dia,
            SquadEnhanceStone,
            GoldDungeonTicket,
            SquadEnhanceStoneDungeonTicket,
            EquipmentEnhanceStone,
            EquipmentEnhanceStoneDungeonTicket
        }

        public enum DungeonClearType
        {
            KillCount,
            BossKill
        }

        public enum DungeonType
        {
            GoldDungeon,
            SquadEnhanceStoneDungeon,
            EquipmentEnhanceStoneDungeon
        }

        /// <summary>
        ///     장비 레어도
        /// </summary>
        public enum EquipmentRarity
        {
            Common,
            Uncommon,
            Magic,
            Rare,
            Unique
            // Legend,
            // Epic,
            // Ancient,
            // Legendary,
            // Mythology,
            // Null
        }

        /// <summary>
        ///     장비 종류
        /// </summary>
        public enum EquipmentType
        {
            Sword,
            Bow,
            Staff,
            Helmet,
            Armor,
            Gauntlet
        }

        public enum IncreaseStatValueType
        {
            BaseStat,
            PercentStat
        }

        public enum LevelType
        {
            CurrentLv,
            CurrentExp
        }

        public enum MonsterClassType
        {
            Human,
            Indian,
            General
        }

        public enum PoolType
        {
            ProjectileBaseAttackWarrior,
            ProjectileBaseAttackArcher,
            ProjectileBaseAttackWizard,
            ProjectileBaseAttackMonster,
            EffectEnhance,
            EffectDamageNormal,
            EffectDamageCritical,
            PopUpMessage
        }

        public enum SkillType
        {
            MoveTo,
            Spawn,
            Follow
        }

        public enum SquadStatType
        {
            Attack, // 공격력 (Attack)
            WarriorAtk, // 전사 공격력
            WizardAtk, // 마법사 공격력
            ArcherAtk, // 궁수 공격력
            
            Health, // 체력 (Health Points)
            WarriorHealth, // 전사 공격력
            WizardHealth, // 마법사 공격력
            ArcherHealth, // 궁수 공격력
            
            Defence, // 방어력 (Defense)
            WarriorDefence, // 전사 공격력
            WizardDefence, // 마법사 공격력
            ArcherDefence, // 궁수 공격력
            
            CriticalRate, // 치명타 확률 (Critical Hit Rate)
            CriticalDamage, // 치명타 피해량 (Critical Hit Damage)
            Accuracy, // 명중률 (Accuracy)
            Penetration, // 관통 (Penetration)
            Evasion, // 회피율 (Evasion
            MoveSpeed, // 이동 속도 (Movement Speed)
            AcquisitionGold, // 골드 획득량
            AcquisitionExp, // 경험치 획득량
            AmplificationSkillEffects, // 스킬 효과 증폭
            WarriorAttackRange,
            ArcherAttackRange,
            WizardAttackRange,
            FollowRange
        }

        public enum SummonType
        {
            Squad,
            Weapon,
            Gear
        }
        
        public static readonly SummonType[] summonTypes =
        {
            SummonType.Squad,
            SummonType.Weapon,
            SummonType.Gear
        };

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
            // Rarity.Epic,
            // Rarity.Ancient,
            // Rarity.Legendary,
            // Rarity.Mythology,
        };

        public static readonly CharacterType[] characterTypes =
        {
            CharacterType.Warrior,
            CharacterType.Archer,
            CharacterType.Wizard
        };

        public static readonly CharacterRarity[] characterRarities =
        {
            CharacterRarity.Rare,
            CharacterRarity.Magic,
            CharacterRarity.Legend
        };
    }
}