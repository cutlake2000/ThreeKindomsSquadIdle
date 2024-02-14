using System;
using System.Collections.Generic;
using Data;
using Function;
using Managers.BattleManager;
using Managers.BottomMenuManager.SquadPanel;
using Managers.GameManager;
using ScriptableObjects.Scripts;
using UnityEngine;

namespace Creature.Data
{
    [Serializable]
    public class CharacterEffect
    {
        public Enums.SquadStatType statType;
        public Enums.IncreaseStatValueType increaseStatType;
        public int increaseValue;
    }

    [Serializable]
    public class Character
    {
        [Header("ES3 ID")] public string characterId;
        [Header("이름")] public string characterName;
        [Header("스프라이트 인덱스")] public int characterIconIndex;
        [Header("레벨")] public int characterLevel;
        [Header("요구 경험치")] public BigInteger characterRequiredCurrency;
        [Header("클래스 타입")] public Enums.CharacterType characterType;
        [Header("클래스 등급")] public Enums.CharacterRarity characterRarity;
        [Header("조각 개수")] public int characterQuantity;
        [Header("장착 여부")] public bool isEquipped;
        [Header("보유 여부")] public bool isPossessed;

        [field: Space(5)] [field: Header("스킬 효과")]
        public CharacterSkill[] characterSkills;

        [field: Space(5)] [field: Header("장착 효과")]
        public List<CharacterEffect> characterEquippedEffects;

        [field: Header("보유 효과")] public List<CharacterEffect> characterOwnedEffects;

        [Space(5)] [Header("프리팹 모델")] public int characterModelIndex;
        [Header("ES3 Loader")] public CharacterES3Loader characterES3Loader;

        public GameObject characterModel;

        /// <summary>
        ///     처음 생성할 때 사용되는 생성자
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="level"></param>
        /// <param name="isEquipped"></param>
        /// <param name="isPossessed"></param>
        /// <param name="type"></param>
        /// <param name="iconIndex"></param>
        /// <param name="icon"></param>
        /// <param name="rarity"></param>
        /// <param name="modelIndex"></param>
        /// <param name="model"></param>
        /// <param name="skills"></param>
        /// <param name="equippedEffect"></param>
        /// <param name="ownedEffect"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Character(string id, string name, int level, bool isEquipped, bool isPossessed, Enums.CharacterType type,
            int iconIndex, Sprite icon, Enums.CharacterRarity rarity, int modelIndex, GameObject model,
            CharacterSkill[] skills, SquadEffectSo equippedEffect, SquadEffectSo ownedEffect, int quantity)
        {
            characterId = id;
            characterName = name;
            characterLevel = level;
            this.isEquipped = isEquipped;
            this.isPossessed = isPossessed;
            characterType = type;
            characterIconIndex = iconIndex;
            characterRarity = rarity;
            characterQuantity = quantity;
            characterModelIndex = modelIndex;
            characterModel = model;

            characterSkills = skills;

            characterEquippedEffects = new List<CharacterEffect>();
            characterOwnedEffects = new List<CharacterEffect>();

            for (var i = 0; i < equippedEffect.squadEffects.Count; i++)
            {
                characterEquippedEffects.Add(new CharacterEffect());
                characterEquippedEffects[i].statType = equippedEffect.squadEffects[i].statType;
                characterEquippedEffects[i].increaseStatType = equippedEffect.squadEffects[i].increaseStatType;
                characterEquippedEffects[i].increaseValue = equippedEffect.squadEffects[i].increaseValue + equippedEffect.squadEffects[i].increaseValue * characterLevel / SquadConfigureManager.CharacterMaxLevel;
            }

            for (var i = 0; i < ownedEffect.squadEffects.Count; i++)
            {
                characterOwnedEffects.Add(new CharacterEffect());
                characterOwnedEffects[i].statType = ownedEffect.squadEffects[i].statType;
                characterOwnedEffects[i].increaseStatType = ownedEffect.squadEffects[i].increaseStatType;
                characterOwnedEffects[i].increaseValue = ownedEffect.squadEffects[i].increaseValue + ownedEffect.squadEffects[i].increaseValue * characterLevel / SquadConfigureManager.CharacterMaxLevel;
            }

            characterRequiredCurrency = rarity switch
            {
                Enums.CharacterRarity.Rare => 10,
                Enums.CharacterRarity.Magic => 15,
                Enums.CharacterRarity.Unique => 50,
                Enums.CharacterRarity.Legend => 65,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };

            SaveCharacterDataIntoES3Loader();
        }

        /// <summary>
        ///     ES3 Load 할 때 사용되는 생성자
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="iconIndex"></param>
        /// <param name="icon"></param>
        /// <param name="rarity"></param>
        /// <param name="modelIndex"></param>
        /// <param name="model"></param>
        /// <param name="skills"></param>
        /// <param name="equippedEffect"></param>
        /// <param name="ownedEffect"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Character(string id, string name, Enums.CharacterType type, int iconIndex, Sprite icon,
            Enums.CharacterRarity rarity, int modelIndex, GameObject model, CharacterSkill[] skills,
            SquadEffectSo equippedEffect, SquadEffectSo ownedEffect)
        {
            characterId = id;
            characterName = name;
            characterType = type;
            characterIconIndex = iconIndex;
            characterRarity = rarity;
            characterModelIndex = modelIndex;
            characterModel = model;
            characterSkills = skills;

            characterEquippedEffects = new List<CharacterEffect>();
            characterOwnedEffects = new List<CharacterEffect>();
            
            LoadCharacterDataFromES3Loader();

            for (var i = 0; i < equippedEffect.squadEffects.Count; i++)
            {
                characterEquippedEffects.Add(new CharacterEffect());
                characterEquippedEffects[i].statType = equippedEffect.squadEffects[i].statType;
                characterEquippedEffects[i].increaseStatType = equippedEffect.squadEffects[i].increaseStatType;
                characterEquippedEffects[i].increaseValue = equippedEffect.squadEffects[i].increaseValue + equippedEffect.squadEffects[i].increaseValue * characterLevel / SquadConfigureManager.CharacterMaxLevel;
            }

            for (var i = 0; i < ownedEffect.squadEffects.Count; i++)
            {
                characterOwnedEffects.Add(new CharacterEffect());
                characterOwnedEffects[i].statType = ownedEffect.squadEffects[i].statType;
                characterOwnedEffects[i].increaseStatType = ownedEffect.squadEffects[i].increaseStatType;
                characterOwnedEffects[i].increaseValue = ownedEffect.squadEffects[i].increaseValue + ownedEffect.squadEffects[i].increaseValue * characterLevel / SquadConfigureManager.CharacterMaxLevel;
            }

            characterRequiredCurrency = rarity switch
            {
                Enums.CharacterRarity.Rare => 10,
                Enums.CharacterRarity.Magic => 15,
                Enums.CharacterRarity.Unique => 50,
                Enums.CharacterRarity.Legend => 65,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }
        
        public void SaveCharacterDataIntoES3Loader()
        {
            var targetIndex = SquadConfigureManager.FindCharacterIndex(characterId);
            SquadConfigureManager.Instance.characterES3Loaders[targetIndex].SaveData(characterId, characterLevel, characterQuantity, isEquipped, isPossessed);
        }

        public void LoadCharacterDataFromES3Loader()
        {
            var targetIndex = SquadConfigureManager.FindCharacterIndex(characterId);

            characterId = SquadConfigureManager.Instance.characterES3Loaders[targetIndex].id;
            characterLevel = SquadConfigureManager.Instance.characterES3Loaders[targetIndex].level;
            characterQuantity = SquadConfigureManager.Instance.characterES3Loaders[targetIndex].quantity;
            isEquipped = SquadConfigureManager.Instance.characterES3Loaders[targetIndex].isEquipped;
            isPossessed = SquadConfigureManager.Instance.characterES3Loaders[targetIndex].isPossessed;
        }

        public BigInteger RequiredCurrencyForLevelUp()
        {
            var requiredCurrency = characterRequiredCurrency * (int)Mathf.Pow(1.8f, characterLevel);

            return requiredCurrency;
        }

        public void CharacterLevelUp()
        {
            characterLevel = Mathf.Min(characterLevel + 1, SquadConfigureManager.CharacterMaxLevel);

            for (var i = 0; i < characterOwnedEffects.Count; i++)
            {
                characterOwnedEffects[i].increaseValue += characterOwnedEffects[i].increaseValue / SquadConfigureManager.CharacterMaxLevel * characterLevel;
                var isBaseStat = characterOwnedEffects[i].increaseStatType == Enums.IncreaseStatValueType.BaseStat;
                SquadBattleManager.Instance.squadEntireStat.UpdateStat((Enums.SquadStatType) Enum.Parse(typeof(Enums.SquadStatType), characterOwnedEffects[i].statType.ToString()), characterOwnedEffects[i].increaseValue, isBaseStat);
            }
        }
    }
}