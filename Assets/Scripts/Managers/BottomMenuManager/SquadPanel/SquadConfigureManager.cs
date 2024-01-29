using System;
using System.Collections.Generic;
using Creature.CreatureClass.SquadClass;
using Creature.Data;
using Managers.BattleManager;
using Module;
using ScriptableObjects.Scripts;
using UnityEngine;
using Enum = Data.Enum;

namespace Managers.BottomMenuManager.SquadPanel
{
    public class SquadConfigureManager : MonoBehaviour
    {
        public const int CharacterMaxLevel = 100;
        public static SquadConfigureManager Instance;
        private static readonly Dictionary<string, Character> AllCharactersDictionary = new();

        //TODO: 임시 So 대체 클래스 -> 추후 csv, json으로 대체
        [SerializeField] private SquadConfigureSo[] squadConfigureSo;
        [SerializeField] private SquadEffectSo[] squadOwnedEffectValueSoByRarity;
        [SerializeField] private SquadEffectSo[] squadEquippedEffectValueSoByRarity;

        [Header("캐릭터 정보 컨테이너")] public List<Character> warriors = new();

        public List<Character> archers = new();
        public List<Character> wizards = new();

        [Header("캐릭터 모델 컨테이너")] public List<GameObject> warriorModels = new();
        public List<GameObject> archerModels = new();
        public List<GameObject> wizardModels = new();

        [Header("캐릭터 모델 스폰 좌표")] public GameObject[] modelSpawnPoints;
        public GameObject[] skillSpawnPoints;
        public readonly Dictionary<string, Character> ArchersDictionary = new();
        public readonly Dictionary<string, Character> WarriorDictionary = new();
        public readonly Dictionary<string, Character> WizardsDictionary = new();

        private void Awake()
        {
            Instance = this;
        }

        public void InitSquadConfigureManager()
        {
            SetAllCharacters();
        }

        private void SetAllCharacters()
        {
            if (ES3.KeyExists("Init_Game"))
                LoadAllCharacters();
            else
                CreateAllCharacters();
        }

        private void LoadAllCharacters()
        {
            foreach (var characterType in Enum.characterTypes)
            {
                foreach (var characterSo in squadConfigureSo)
                {
                    if (characterType != characterSo.characterType) continue;

                    var characterId = $"{characterSo.characterName}";
                    var characterName = characterSo.characterName;
                    var characterIconIndex = characterSo.characterIconIndex;
                    var characterIcon =
                        SpriteManager.Instance.GetCharacterSprite(characterType, characterSo.characterIconIndex);
                    var characterRarity = characterSo.characterRarity;
                    var characterModelIndex = characterSo.characterModelIndex;
                    var characterModel = characterType switch
                    {
                        Enum.CharacterType.Warrior => warriorModels[characterModelIndex],
                        Enum.CharacterType.Archer => archerModels[characterModelIndex],
                        Enum.CharacterType.Wizard => wizardModels[characterModelIndex],
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    var characterSkills = characterSo.characterSkills;
                    var equippedEffect = squadEquippedEffectValueSoByRarity[(int)characterSo.characterRarity];
                    var ownedEffect = squadOwnedEffectValueSoByRarity[(int)characterSo.characterRarity];

                    var character = new Character(characterId, characterName, characterType, characterIconIndex,
                        characterIcon, characterRarity, characterModelIndex, characterModel, characterSkills,
                        equippedEffect, ownedEffect);
                    AddCharacter(characterId, character);

                    if (character.isEquipped)
                    {
                        InstantiateModelUnderParent(characterType, characterModel, characterIcon,
                            modelSpawnPoints[(int)characterType].transform);
                        InstantiateSkillUnderParent(characterType, characterSkills,
                            skillSpawnPoints[(int)characterType].transform);

                        foreach (var effect in character.characterEquippedEffects)
                            if (effect.increaseStatType == Enum.IncreaseStatValueType.BaseStat)
                                SquadBattleManager.Instance.squadEntireStat.UpdateBaseStatBySquadConfigurePanel(
                                    effect.statType, effect.increaseValue);
                            else
                                SquadBattleManager.Instance.squadEntireStat.UpdatePercentStatBySquadConfigurePanel(
                                    effect.statType, effect.increaseValue);
                    }

                    if (character.isPossessed)
                        foreach (var effect in character.characterOwnedEffects)
                            if (effect.increaseStatType == Enum.IncreaseStatValueType.BaseStat)
                                SquadBattleManager.Instance.squadEntireStat.UpdateBaseStatBySquadConfigurePanel(
                                    effect.statType, effect.increaseValue);
                            else
                                SquadBattleManager.Instance.squadEntireStat.UpdatePercentStatBySquadConfigurePanel(
                                    effect.statType, effect.increaseValue);

                    InfiniteLoopDetector.Run();
                }

                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.UpdateSquadConfigureScrollViewItemUI(
                    characterType);
            }
        }

        private void CreateAllCharacters()
        {
            foreach (var characterType in Enum.characterTypes)
            {
                var characterIndex = 0;

                foreach (var characterSo in squadConfigureSo)
                {
                    if (characterType != characterSo.characterType) continue;

                    var characterId = $"{characterSo.characterName}";
                    var characterName = characterSo.characterName;
                    var isEquipped = characterIndex == 1;
                    var isPossessed = characterIndex == 1;
                    var characterLevel = isPossessed ? 1 : 0;
                    var characterIconIndex = characterSo.characterIconIndex;
                    var characterIcon =
                        SpriteManager.Instance.GetCharacterSprite(characterType, characterSo.characterIconIndex);
                    var characterRarity = characterSo.characterRarity;
                    var characterModelIndex = characterSo.characterModelIndex;
                    var characterModel = characterType switch
                    {
                        Enum.CharacterType.Warrior => warriorModels[characterModelIndex],
                        Enum.CharacterType.Archer => archerModels[characterModelIndex],
                        Enum.CharacterType.Wizard => wizardModels[characterModelIndex],
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    var characterSkills = characterSo.characterSkills;
                    var equippedEffect = squadEquippedEffectValueSoByRarity[(int)characterSo.characterRarity];
                    var ownedEffect = squadOwnedEffectValueSoByRarity[(int)characterSo.characterRarity];

                    var character = new Character(characterId, characterName, characterLevel, isEquipped, isPossessed,
                        characterType, characterIconIndex, characterIcon, characterRarity, characterModelIndex,
                        characterModel, characterSkills, equippedEffect, ownedEffect);
                    AddCharacter(characterId, character);
                    characterIndex++;

                    if (character.isEquipped)
                    {
                        InstantiateModelUnderParent(characterType, characterModel, characterIcon,
                            modelSpawnPoints[(int)characterType].transform);
                        InstantiateSkillUnderParent(characterType, characterSkills,
                            skillSpawnPoints[(int)characterType].transform);

                        foreach (var effect in character.characterEquippedEffects)
                            if (effect.increaseStatType == Enum.IncreaseStatValueType.BaseStat)
                                SquadBattleManager.Instance.squadEntireStat.UpdateBaseStatBySquadConfigurePanel(
                                    effect.statType, effect.increaseValue);
                            else
                                SquadBattleManager.Instance.squadEntireStat.UpdatePercentStatBySquadConfigurePanel(
                                    effect.statType, effect.increaseValue);
                    }

                    if (character.isPossessed)
                        foreach (var effect in character.characterOwnedEffects)
                            if (effect.increaseStatType == Enum.IncreaseStatValueType.BaseStat)
                                SquadBattleManager.Instance.squadEntireStat.UpdateBaseStatBySquadConfigurePanel(
                                    effect.statType, effect.increaseValue);
                            else
                                SquadBattleManager.Instance.squadEntireStat.UpdatePercentStatBySquadConfigurePanel(
                                    effect.statType, effect.increaseValue);

                    InfiniteLoopDetector.Run();
                }

                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.UpdateSquadConfigureScrollViewItemUI(
                    characterType);
            }
        }

        /// <summary>
        ///     캐릭터 선택 시, 해당 캐릭터의 모델을 Instantiate하는 메서드
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefab"></param>
        /// <param name="characterIcon"></param>
        /// <param name="parentTransform"></param>
        private void InstantiateModelUnderParent(Enum.CharacterType type, GameObject prefab, Sprite characterIcon,
            Transform parentTransform)
        {
            var character = Instantiate(prefab, parentTransform);
            character.transform.SetParent(parentTransform);

            SquadBattleManager.Instance.squads[(int)type].GetComponent<Squad>().projectileSpawn =
                character.GetComponent<CharacterModelInfo>().projectileSpawnPosition.transform;
            SquadBattleManager.Instance.squads[(int)type].GetComponent<Squad>().spumSprite =
                character.GetComponent<CharacterModelInfo>().spumSpriteList;
            SquadBattleManager.Instance.squads[(int)type].GetComponent<Squad>().SetAllSpritesList();

            switch (type)
            {
                case Enum.CharacterType.Warrior:
                    UIManager.Instance.squadSkillCoolTimerUI.warriorIcon.sprite = characterIcon;
                    break;
                case Enum.CharacterType.Archer:
                    UIManager.Instance.squadSkillCoolTimerUI.archerIcon.sprite = characterIcon;
                    break;
                case Enum.CharacterType.Wizard:
                    UIManager.Instance.squadSkillCoolTimerUI.wizardIcon.sprite = characterIcon;
                    break;
            }
        }

        /// <summary>
        ///     캐릭터 선택 시, 해당 캐릭터의 스킬을 Instantiate하는 메서드
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefab"></param>
        /// <param name="parentTransform"></param>
        private void InstantiateSkillUnderParent(Enum.CharacterType type, IList<CharacterSkill> prefab,
            Transform parentTransform)
        {
            switch (type)
            {
                case Enum.CharacterType.Warrior:
                    SquadBattleManager.Instance.warriorSkillDamagePercent.Clear();
                    for (var i = 0; i < prefab.Count; i++)
                    {
                        var characterSkill = Instantiate(prefab[i].skillObject, parentTransform);
                        characterSkill.transform.SetParent(parentTransform);

                        SquadBattleManager.Instance.warriorSkillCoolTimer[i].skill = characterSkill;
                        SquadBattleManager.Instance.warriorSkillCoolTimer[i].isSkillReady = true;
                        SquadBattleManager.Instance.warriorSkillCoolTimer[i].maxSkillCoolTime =
                            prefab[i].maxSkillCoolTime;
                        SquadBattleManager.Instance.warriorSkillCoolTimer[i].remainedSkillCoolTime =
                            prefab[i].maxSkillCoolTime;
                        SquadBattleManager.Instance.warriorSkillCoolTimer[i].orderToInstantiate = false;

                        UIManager.Instance.squadSkillCoolTimerUI.warriorSkillCoolTimerUI[i].skillIcon.sprite =
                            SpriteManager.Instance.GetSkillSprite(type, prefab[i].skillIconIndex);

                        SquadBattleManager.Instance.warriorSkillDamagePercent.Add(prefab[i].skillDamagePercent);
                    }

                    break;
                case Enum.CharacterType.Archer:
                    SquadBattleManager.Instance.archerSkillDamagePercent.Clear();
                    for (var i = 0; i < prefab.Count; i++)
                    {
                        var characterSkill = Instantiate(prefab[i].skillObject, parentTransform);
                        characterSkill.transform.SetParent(parentTransform);

                        SquadBattleManager.Instance.archerSkillCoolTimer[i].skill = characterSkill;
                        SquadBattleManager.Instance.archerSkillCoolTimer[i].isSkillReady = true;
                        SquadBattleManager.Instance.archerSkillCoolTimer[i].maxSkillCoolTime =
                            prefab[i].maxSkillCoolTime;
                        SquadBattleManager.Instance.archerSkillCoolTimer[i].remainedSkillCoolTime =
                            prefab[i].maxSkillCoolTime;
                        SquadBattleManager.Instance.archerSkillCoolTimer[i].orderToInstantiate = false;

                        UIManager.Instance.squadSkillCoolTimerUI.archerSkillCoolTimerUI[i].skillIcon.sprite =
                            SpriteManager.Instance.GetSkillSprite(type, prefab[i].skillIconIndex);

                        SquadBattleManager.Instance.archerSkillDamagePercent.Add(prefab[i].skillDamagePercent);
                    }

                    break;
                case Enum.CharacterType.Wizard:
                    SquadBattleManager.Instance.wizardSkillDamagePercent.Clear();
                    for (var i = 0; i < prefab.Count; i++)
                    {
                        var characterSkill = Instantiate(prefab[i].skillObject, parentTransform);
                        characterSkill.transform.SetParent(parentTransform);

                        SquadBattleManager.Instance.wizardSkillCoolTimer[i].skill = characterSkill;
                        SquadBattleManager.Instance.wizardSkillCoolTimer[i].isSkillReady = true;
                        SquadBattleManager.Instance.wizardSkillCoolTimer[i].maxSkillCoolTime =
                            prefab[i].maxSkillCoolTime;
                        SquadBattleManager.Instance.wizardSkillCoolTimer[i].remainedSkillCoolTime =
                            prefab[i].maxSkillCoolTime;
                        SquadBattleManager.Instance.wizardSkillCoolTimer[i].orderToInstantiate = false;

                        UIManager.Instance.squadSkillCoolTimerUI.wizardSkillCoolTimerUI[i].skillIcon.sprite =
                            SpriteManager.Instance.GetSkillSprite(type, prefab[i].skillIconIndex);

                        SquadBattleManager.Instance.wizardSkillDamagePercent.Add(prefab[i].skillDamagePercent);
                    }

                    break;
            }
        }

        private void AddCharacter(string characterID, Character character)
        {
            switch (character.characterType)
            {
                case Enum.CharacterType.Warrior:
                    if (!WarriorDictionary.TryAdd(characterID, character))
                        Debug.LogWarning($"Character already exists in the dictionary: {character}");

                    break;
                case Enum.CharacterType.Archer:
                    if (!ArchersDictionary.TryAdd(characterID, character))
                        Debug.LogWarning($"Character already exists in the dictionary: {character}");

                    break;
                case Enum.CharacterType.Wizard:
                    if (!WizardsDictionary.TryAdd(characterID, character))
                        Debug.LogWarning($"Character already exists in the dictionary: {character}");

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void UpdateCharacterData(string characterId, Character character)
        {
            var targetCharacter = character.characterType switch
            {
                Enum.CharacterType.Warrior => WarriorDictionary[characterId],
                Enum.CharacterType.Archer => ArchersDictionary[characterId],
                Enum.CharacterType.Wizard => WizardsDictionary[characterId],
                _ => throw new ArgumentOutOfRangeException()
            };

            if (targetCharacter == null) return;

            targetCharacter.characterLevel = character.characterLevel;

            for (var i = 0; i < targetCharacter.characterOwnedEffects.Count; i++)
                targetCharacter.characterOwnedEffects[i].increaseValue =
                    character.characterOwnedEffects[i].increaseValue;

            targetCharacter.SaveCharacterAllInfo(targetCharacter.characterId);
        }
    }
}