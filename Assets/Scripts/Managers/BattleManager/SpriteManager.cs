using System;
using System.Collections.Generic;
using UnityEngine;
using Enum = Data.Enum;

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

        public Sprite GetEquipmentSprite(Enum.EquipmentType equipmentType, int spriteIndex)
        {
            return equipmentType switch
            {
                Enum.EquipmentType.Sword => swordWeaponSprite[spriteIndex],
                Enum.EquipmentType.Bow => bowWeaponSprite[spriteIndex],
                Enum.EquipmentType.Staff => staffWeaponSprite[spriteIndex],
                Enum.EquipmentType.Helmet => helmetGearSprite[spriteIndex],
                Enum.EquipmentType.Armor => armorGearSprite[spriteIndex],
                Enum.EquipmentType.Gauntlet => gauntletGearSprite[spriteIndex],
                _ => null
            };
        }

        public Sprite GetCharacterSprite(Enum.CharacterType characterType, int spriteIndex)
        {
            return characterType switch
            {
                Enum.CharacterType.Warrior => warriorSprite[spriteIndex],
                Enum.CharacterType.Archer => archerSprite[spriteIndex],
                Enum.CharacterType.Wizard => wizardSprite[spriteIndex],
                _ => null
            };
        }

        public Sprite GetSkillSprite(Enum.CharacterType characterType, int spriteIndex)
        {
            return characterType switch
            {
                Enum.CharacterType.Warrior => warriorSkillSprite[spriteIndex],
                Enum.CharacterType.Archer => archerSkillSprite[spriteIndex],
                Enum.CharacterType.Wizard => wizardSkillSprite[spriteIndex],
                _ => null
            };
        }

        public Sprite GetCurrencySprite(Enum.CurrencyType currencyType)
        {
            return currencyType switch
            {
                Enum.CurrencyType.Gold => currencySprite[0],
                Enum.CurrencyType.Dia => currencySprite[1],
                Enum.CurrencyType.WeaponEnhanceStone => currencySprite[2],
                Enum.CurrencyType.GearEnhanceStone => currencySprite[3],
                Enum.CurrencyType.SquadEnhanceStone => currencySprite[4],
                Enum.CurrencyType.GoldDungeonTicket => currencySprite[5],
                Enum.CurrencyType.EnhanceDungeonTicket => currencySprite[6],
                _ => throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null)
            };
        }
    }
}