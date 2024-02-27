using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers.BattleManager
{
    public class SpriteManager : MonoBehaviour
    {
        public static SpriteManager Instance;

        [Header("장비 스프라이트")]
        [Tooltip("무기 - 워리어")] [SerializeField] private List<Sprite> swordWeaponSprite;
        [Tooltip("무기 - 아처")] [SerializeField] private List<Sprite> bowWeaponSprite;
        [Tooltip("무기 - 위자드")] [SerializeField] private List<Sprite> staffWeaponSprite;
        [Tooltip("방어구 - 헬멧")] [SerializeField] private List<Sprite> helmetGearSprite;
        [Tooltip("방어구 - 갑옷")] [SerializeField] private List<Sprite> armorGearSprite;
        [Tooltip("방어구 - 장갑")] [SerializeField] private List<Sprite> gauntletGearSprite;
        [FormerlySerializedAs("equipmentBackground")]
        [Space(5)]
        [Tooltip("장비 배경")] [SerializeField] private List<Sprite> equipmentBackgrounds;
        [Tooltip("장비 배경 효과")] [SerializeField] private List<Sprite> equipmentBackgroundEffects;

        [Space(5)]
        [Header("캐릭터 스프라이트")]
        [Tooltip("일러스트 - 워리어")] [SerializeField] private List<Sprite> warriorSprite;
        [Tooltip("일러스트 - 아처")] [SerializeField] private List<Sprite> archerSprite;
        [Tooltip("일러스트 - 위자드")] [SerializeField] private List<Sprite> wizardSprite;

        [Space(5)]
        [Header("스킬 스프라이트")]
        [Tooltip("스킬 - 워리어")] [SerializeField] private List<Sprite> warriorSkillSprite;
        [Tooltip("스킬 - 아처")] [SerializeField] private List<Sprite> archerSkillSprite;
        [Tooltip("스킬 - 위자드")] [SerializeField] private List<Sprite> wizardSkillSprite;

        [Space(5)]
        [Header("재화 스프라이트")] [SerializeField] private List<Sprite> currencySprite;

        private void Awake()
        {
            Instance = this;
        }

        public Sprite GetEquipmentBackground(int spriteIndex)
        {
            return equipmentBackgrounds[spriteIndex];
        }
        
        public Sprite GetEquipmentBackgroundEffect(int spriteIndex)
        {
            return equipmentBackgroundEffects[spriteIndex];
        }

        public Sprite GetEquipmentSprite(Enums.EquipmentType equipmentType, int spriteIndex)
        {
            return equipmentType switch
            {
                Enums.EquipmentType.Sword => swordWeaponSprite[spriteIndex],
                Enums.EquipmentType.Bow => bowWeaponSprite[spriteIndex],
                Enums.EquipmentType.Staff => staffWeaponSprite[spriteIndex],
                Enums.EquipmentType.Helmet => helmetGearSprite[spriteIndex],
                Enums.EquipmentType.Armor => armorGearSprite[spriteIndex],
                Enums.EquipmentType.Gauntlet => gauntletGearSprite[spriteIndex],
                _ => null
            };
        }

        public Sprite GetCharacterSprite(Enums.CharacterType characterType, int spriteIndex)
        {
            return characterType switch
            {
                Enums.CharacterType.Warrior => warriorSprite[spriteIndex],
                Enums.CharacterType.Archer => archerSprite[spriteIndex],
                Enums.CharacterType.Wizard => wizardSprite[spriteIndex],
                _ => null
            };
        }

        public Sprite GetSkillSprite(Enums.CharacterType characterType, int spriteIndex)
        {
            return characterType switch
            {
                Enums.CharacterType.Warrior => warriorSkillSprite[spriteIndex],
                Enums.CharacterType.Archer => archerSkillSprite[spriteIndex],
                Enums.CharacterType.Wizard => wizardSkillSprite[spriteIndex],
                _ => null
            };
        }

        public Sprite GetCurrencySprite(Enums.CurrencyType currencyType)
        {
            return currencyType switch
            {
                Enums.CurrencyType.Gold => currencySprite[0],
                Enums.CurrencyType.Dia => currencySprite[1],
                Enums.CurrencyType.Exp => currencySprite[2],
                Enums.CurrencyType.EquipmentEnhanceStone => currencySprite[3],
                Enums.CurrencyType.SquadEnhanceStone => currencySprite[4],
                Enums.CurrencyType.GoldDungeonTicket => currencySprite[5],
                Enums.CurrencyType.SquadEnhanceStoneDungeonTicket => currencySprite[6],
                Enums.CurrencyType.EquipmentEnhanceStoneDungeonTicket => currencySprite[7],
                _ => throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null)
            };
        }
    }
}