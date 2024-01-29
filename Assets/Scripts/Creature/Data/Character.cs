using System;
using System.Collections.Generic;
using Function;
using Managers.BottomMenuManager.SquadPanel;
using ScriptableObjects.Scripts;
using UnityEngine;
using Enum = Data.Enum;

namespace Creature.Data
{
    [Serializable]
    public class CharacterEffect
    {
        public Enum.StatTypeBySquadConfigurePanel statType;
        public Enum.IncreaseStatValueType increaseStatType;
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

        [Header("클래스 타입")] public Enum.CharacterType characterType;

        [Header("클래스 등급")] public Enum.CharacterRarity characterRarity;

        [Header("장착 여부")] public bool isEquipped;

        [Header("보유 여부")] public bool isPossessed;

        [field: Space(5)] [field: Header("스킬 효과")]
        public CharacterSkill[] characterSkills;

        [field: Space(5)] [field: Header("장착 효과")]
        public List<CharacterEffect> characterEquippedEffects;

        [field: Header("보유 효과")] public List<CharacterEffect> characterOwnedEffects;

        [Space(5)] [Header("프리팹 모델")] public int characterModelIndex;

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
        public Character(string id, string name, int level, bool isEquipped, bool isPossessed, Enum.CharacterType type,
            int iconIndex, Sprite icon, Enum.CharacterRarity rarity, int modelIndex, GameObject model,
            CharacterSkill[] skills, SquadEffectSo equippedEffect, SquadEffectSo ownedEffect)
        {
            characterId = id;
            characterName = name;
            characterLevel = level;
            this.isEquipped = isEquipped;
            this.isPossessed = isPossessed;
            characterType = type;
            characterIconIndex = iconIndex;
            characterRarity = rarity;

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
                characterEquippedEffects[i].increaseValue = equippedEffect.squadEffects[i].increaseValue;
            }

            for (var i = 0; i < ownedEffect.squadEffects.Count; i++)
            {
                characterOwnedEffects.Add(new CharacterEffect());
                characterOwnedEffects[i].statType = ownedEffect.squadEffects[i].statType;
                characterOwnedEffects[i].increaseStatType = ownedEffect.squadEffects[i].increaseStatType;
                characterOwnedEffects[i].increaseValue = ownedEffect.squadEffects[i].increaseValue;
            }

            characterRequiredCurrency = rarity switch
            {
                Enum.CharacterRarity.Rare => 10,
                Enum.CharacterRarity.Magic => 15,
                Enum.CharacterRarity.Unique => 50,
                Enum.CharacterRarity.Legend => 65,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };

            SaveCharacterAllInfo();
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
        public Character(string id, string name, Enum.CharacterType type, int iconIndex, Sprite icon,
            Enum.CharacterRarity rarity, int modelIndex, GameObject model, CharacterSkill[] skills,
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

            for (var i = 0; i < equippedEffect.squadEffects.Count; i++)
            {
                characterEquippedEffects.Add(new CharacterEffect());
                characterEquippedEffects[i].statType = equippedEffect.squadEffects[i].statType;
                characterEquippedEffects[i].increaseStatType = equippedEffect.squadEffects[i].increaseStatType;
            }

            for (var i = 0; i < ownedEffect.squadEffects.Count; i++)
            {
                characterOwnedEffects.Add(new CharacterEffect());
                characterOwnedEffects[i].statType = ownedEffect.squadEffects[i].statType;
                characterOwnedEffects[i].increaseStatType = ownedEffect.squadEffects[i].increaseStatType;
            }

            characterRequiredCurrency = rarity switch
            {
                Enum.CharacterRarity.Rare => 10,
                Enum.CharacterRarity.Magic => 15,
                Enum.CharacterRarity.Unique => 50,
                Enum.CharacterRarity.Legend => 65,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };

            LoadCharacterAllInfo();
        }

        public void LoadCharacterAllInfo()
        {
            if (!ES3.KeyExists($"{nameof(characterId)}_" + characterId)) return;

            characterLevel = ES3.Load<int>($"{nameof(characterLevel)}_" + characterId);
            isEquipped = ES3.Load<bool>($"{nameof(isEquipped)}_" + characterId);
            isPossessed = ES3.Load<bool>($"{nameof(isPossessed)}_" + characterId);

            for (var i = 0; i < characterEquippedEffects.Count; i++)
                characterEquippedEffects[i].increaseValue =
                    ES3.Load<int>($"characterEquippedEffects[{i}].increaseValue_" + characterId);

            for (var i = 0; i < characterOwnedEffects.Count; i++)
                characterOwnedEffects[i].increaseValue =
                    ES3.Load<int>($"characterOwnedEffects[{i}].increaseValue_" + characterId);
        }

        public void SaveCharacterAllInfo()
        {
            ES3.Save($"{nameof(characterId)}_" + characterId, characterId);

            ES3.Save($"{nameof(characterLevel)}_" + characterId, characterLevel);
            ES3.Save($"{nameof(isEquipped)}_" + characterId, isEquipped);
            ES3.Save($"{nameof(isPossessed)}_" + characterId, isPossessed);

            for (var i = 0; i < characterEquippedEffects.Count; i++)
                ES3.Save($"characterEquippedEffects[{i}].increaseValue_" + characterId,
                    characterEquippedEffects[i].increaseValue);

            for (var i = 0; i < characterOwnedEffects.Count; i++)
                ES3.Save($"characterOwnedEffects[{i}].increaseValue_" + characterId,
                    characterOwnedEffects[i].increaseValue);
        }

        public void SaveCharacterAllInfo(string id)
        {
            ES3.Save($"{nameof(characterId)}_" + id, characterId);

            ES3.Save($"{nameof(characterLevel)}_" + id, characterLevel);
            ES3.Save($"{nameof(isEquipped)}_" + id, isEquipped);
            ES3.Save($"{nameof(isPossessed)}_" + id, isPossessed);

            for (var i = 0; i < characterEquippedEffects.Count; i++)
                ES3.Save($"characterEquippedEffects[{i}].increaseValue_" + id,
                    characterEquippedEffects[i].increaseValue);

            for (var i = 0; i < characterOwnedEffects.Count; i++)
                ES3.Save($"characterOwnedEffects[{i}].increaseValue_" + id, characterOwnedEffects[i].increaseValue);
        }

        // public void SaveCharacterEachInfo(string equipmentID, Enum.CharacterProperty property)
        // {
        //     switch (property)
        //     {
        //         case Enum.CharacterProperty.Level:
        //             ES3.Save($"{nameof(characterLevel)}_" + characterId, characterLevel);
        //             break;
        //         case Enum.CharacterProperty.Level:
        //             ES3.Save($"{nameof(characterLevel)}_" + characterId, characterLevel);
        //             break;
        //     }
        // }

        public BigInteger RequiredCurrencyForLevelUp()
        {
            var requiredCurrency = characterRequiredCurrency * (int)Mathf.Pow(1.8f, characterLevel);

            return requiredCurrency;
        }

        public void CharacterLevelUp()
        {
            characterLevel++;

            for (var i = 0; i < characterOwnedEffects.Count; i++)
                characterOwnedEffects[i].increaseValue += characterOwnedEffects[i].increaseValue /
                    SquadConfigureManager.CharacterMaxLevel * characterLevel;
        }
    }
}