using System;
using System.Collections.Generic;
using Creature.CreatureClass.SquadClass;
using Creature.Data;
using Data;
using Managers.BattleManager;
using Managers.BottomMenuManager.InventoryPanel;
using Module;
using ScriptableObjects.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers.BottomMenuManager.SquadPanel
{
    public class SquadConfigureManager : MonoBehaviour
    {
        public const int CharacterMaxLevel = 100;
        public static SquadConfigureManager Instance;
        private static readonly Dictionary<string, Character> AllCharactersDictionary = new();

        public bool isSquadConfigureChanged;

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
        [Header("캐릭터 스킬 스폰 좌표")] public GameObject[] skillSpawnPoints;
        public readonly Dictionary<string, Character> ArchersDictionary = new();
        public readonly Dictionary<string, Character> WarriorDictionary = new();
        public readonly Dictionary<string, Character> WizardsDictionary = new();

        private void Awake()
        {
            Instance = this;
        }

        public void InitSquadConfigureManager()
        {
            isSquadConfigureChanged = false;
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
            foreach (var characterType in Enums.characterTypes)
            {
                foreach (var characterSo in squadConfigureSo)
                {
                    if (characterType != characterSo.characterType) continue;

                    var characterId = $"{characterSo.characterRarity}_{characterSo.characterType}";
                    var characterName = characterSo.characterName;
                    var characterIconIndex = characterSo.characterIconIndex;
                    var characterIcon =
                        SpriteManager.Instance.GetCharacterSprite(characterType, characterSo.characterIconIndex);
                    var characterRarity = characterSo.characterRarity;
                    var characterModelIndex = characterSo.characterModelIndex;
                    var characterModel = characterType switch
                    {
                        Enums.CharacterType.Warrior => warriorModels[characterModelIndex],
                        Enums.CharacterType.Archer => archerModels[characterModelIndex],
                        Enums.CharacterType.Wizard => wizardModels[characterModelIndex],
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
                        UpdateSquadConfigureModelOnMenu(character);
                        UpdateSquadConfigureModelOnBattle(character);
                    }

                    if (character.isPossessed)
                        foreach (var effect in character.characterOwnedEffects)
                            if (effect.increaseStatType == Enums.IncreaseStatValueType.BaseStat)
                                SquadBattleManager.Instance.squadEntireStat.UpdateBaseStatFromSquadConfigurePanel(
                                    effect.statType, effect.increaseValue);
                            else
                                SquadBattleManager.Instance.squadEntireStat.UpdatePercentStatFromSquadConfigurePanel(
                                    effect.statType, effect.increaseValue);

                    InfiniteLoopDetector.Run();
                }

                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.UpdateSquadConfigureScrollViewItemUI(
                    characterType, true);
            }
        }

        private void CreateAllCharacters()
        {
            foreach (var characterType in Enums.characterTypes)
            {
                var characterIndex = 0;

                foreach (var characterSo in squadConfigureSo)
                {
                    if (characterType != characterSo.characterType) continue;

                    var characterId = $"{characterSo.characterRarity}_{characterSo.characterType}";
                    var characterName = characterSo.characterName;
       
                    var isEquipped = characterIndex == 0;

                    var characterLevel = isEquipped ? 1 : 0;
                    var characterIconIndex = characterSo.characterIconIndex;
                    var characterIcon =
                        SpriteManager.Instance.GetCharacterSprite(characterType, characterSo.characterIconIndex);
                    var characterRarity = characterSo.characterRarity;
                    var characterModelIndex = characterSo.characterModelIndex;
                    var characterModel = characterType switch
                    {
                        Enums.CharacterType.Warrior => warriorModels[characterModelIndex],
                        Enums.CharacterType.Archer => archerModels[characterModelIndex],
                        Enums.CharacterType.Wizard => wizardModels[characterModelIndex],
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    var characterSkills = characterSo.characterSkills;
                    var equippedEffect = squadEquippedEffectValueSoByRarity[(int)characterSo.characterRarity];
                    var ownedEffect = squadOwnedEffectValueSoByRarity[(int)characterSo.characterRarity];

                    var character = new Character(characterId, characterName, characterLevel, isEquipped, isEquipped,
                        characterType, characterIconIndex, characterIcon, characterRarity, characterModelIndex,
                        characterModel, characterSkills, equippedEffect, ownedEffect, 0);
                    AddCharacter(characterId, character);
                    characterIndex++;

                    if (character.isEquipped)
                    {
                        UpdateSquadConfigureModelOnMenu(character);
                        UpdateSquadConfigureModelOnBattle(character);
                    }

                    if (character.isPossessed)
                        foreach (var effect in character.characterOwnedEffects)
                            if (effect.increaseStatType == Enums.IncreaseStatValueType.BaseStat)
                                SquadBattleManager.Instance.squadEntireStat.UpdateBaseStatFromSquadConfigurePanel(
                                    effect.statType, effect.increaseValue);
                            else
                                SquadBattleManager.Instance.squadEntireStat.UpdatePercentStatFromSquadConfigurePanel(
                                    effect.statType, effect.increaseValue);

                    InfiniteLoopDetector.Run();
                }

                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.UpdateSquadConfigureScrollViewItemUI(
                    characterType, true);
            }
        }

        /// <summary>
        /// 현재 장착 중인 캐릭터를 찾는 메서드
        /// </summary>
        public Character FindEquippedCharacter(Enums.CharacterType characterType)
        {
            switch (characterType)
            {
                case Enums.CharacterType.Warrior:
                    return WarriorDictionary[warriors[0].characterId];
                case Enums.CharacterType.Archer:
                    return ArchersDictionary[archers[0].characterId];
                case Enums.CharacterType.Wizard:
                    return WizardsDictionary[wizards[0].characterId];
                default:
                    throw new ArgumentOutOfRangeException(nameof(characterType), characterType, null);
            }
        }

        /// <summary>
        /// 캐릭터 모델을 UI에 세팅하는 메서드
        /// </summary>
        /// <param name="character"></param>
        public void UpdateSquadConfigureModelOnMenu(Character character)
        {
            InstantiateModelOfInventoryUnderParent(character.characterType, character.characterModel);
            InstantiateModelOfConfigureUnderParent(character.characterType, character.characterModel);
        }

        /// <summary>
        /// 캐릭터 모델을 전투씬에 세팅하는 메서드
        /// </summary>
        /// <param name="character"></param>
        public void UpdateSquadConfigureModelOnBattle(Character character)
        {
            InstantiateModelOfBattleUnderParent(character.characterType, character.characterModel, SpriteManager.Instance.GetCharacterSprite(character.characterType, character.characterIconIndex),
                modelSpawnPoints[(int)character.characterType].transform);
            InstantiateSkillUnderParent(character.characterType, character.characterSkills,
                skillSpawnPoints[(int)character.characterType].transform);

            foreach (var effect in character.characterEquippedEffects)
                if (effect.increaseStatType == Enums.IncreaseStatValueType.BaseStat)
                    SquadBattleManager.Instance.squadEntireStat.UpdateBaseStatFromSquadConfigurePanel(
                        effect.statType, effect.increaseValue);
                else
                    SquadBattleManager.Instance.squadEntireStat.UpdatePercentStatFromSquadConfigurePanel(
                        effect.statType, effect.increaseValue);
        }

        /// <summary>
        /// 인벤토리 패널에 Instantiate하는 메서드
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefab"></param>
        private void InstantiateModelOfInventoryUnderParent(Enums.CharacterType type, GameObject prefab)
        {
            var parentTransform = UIManager.Instance.inventoryPanelUI.spawnTargetPosition[(int) type].transform;
            
            if (parentTransform.childCount != 0) Destroy(parentTransform.GetChild(0).gameObject);
            var character = Instantiate(prefab, parentTransform);
            character.transform.SetParent(parentTransform);
            ChangeLayerRecursively(character, 5);
        }

        /// <summary>
        /// 캐릭터 선택 시, 해당 캐릭터의 모델을 씬 위로 Instantiate하는 메서드
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefab"></param>
        /// <param name="characterIcon"></param>
        /// <param name="parentTransform"></param>
        private void InstantiateModelOfBattleUnderParent(Enums.CharacterType type, GameObject prefab, Sprite characterIcon,
            Transform parentTransform)
        {
            if (parentTransform.childCount >= 2) Destroy(parentTransform.GetChild(1).gameObject);
            
            var character = Instantiate(prefab, parentTransform);
            character.transform.SetParent(parentTransform);

            SquadBattleManager.Instance.squads[(int)type].GetComponent<Squad>().animator =
                character.gameObject.GetComponentInChildren<Animator>();
            SquadBattleManager.Instance.squads[(int)type].GetComponent<Squad>().projectileSpawn =
                character.GetComponent<CharacterModelInfo>().projectileSpawnPosition.transform;
            SquadBattleManager.Instance.squads[(int)type].GetComponent<Squad>().allSprites.Clear();
            SquadBattleManager.Instance.squads[(int)type].GetComponent<Squad>().spumSprite =
                character.GetComponent<CharacterModelInfo>().spumSpriteList;

            switch (type)
            {
                case Enums.CharacterType.Warrior:
                    UIManager.Instance.squadSkillCoolTimerUI.warriorIcon.sprite = characterIcon;
                    break;
                case Enums.CharacterType.Archer:
                    UIManager.Instance.squadSkillCoolTimerUI.archerIcon.sprite = characterIcon;
                    break;
                case Enums.CharacterType.Wizard:
                    UIManager.Instance.squadSkillCoolTimerUI.wizardIcon.sprite = characterIcon;
                    break;
            }
        }

        /// <summary>
        /// 캐릭터 선택 시, 해당 캐릭터의 모델을 스쿼드 구성 UI 위로 Instantiate하는 메서드
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefab"></param>
        public void InstantiateModelOfConfigureUnderParent(Enums.CharacterType type, GameObject prefab)
        {
            var parentTransform = UIManager.Instance.squadPanelUI.squadConfigurePanelUI.characterSpawnPosition[(int) type].transform;
            
            if (parentTransform.childCount != 0) Destroy(parentTransform.GetChild(0).gameObject);
            var character = Instantiate(prefab, parentTransform);
            character.transform.SetParent(parentTransform);
            ChangeLayerRecursively(character, 5);
        }

        /// <summary>
        /// 하위 객체들의 레이어를 전부 변경하는 메서드
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="layer"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void ChangeLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
            {
                ChangeLayerRecursively(child.gameObject, layer);
            }
        }

        /// <summary>
        /// 캐릭터 선택 시, 해당 캐릭터의 스킬을 Instantiate하는 메서드
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefab"></param>
        /// <param name="parentTransform"></param>
        private void InstantiateSkillUnderParent(Enums.CharacterType type, IList<CharacterSkill> prefab,
            Transform parentTransform)
        {
            if (parentTransform.childCount != 0) Destroy(parentTransform.GetChild(0).gameObject);
            
            switch (type)
            {
                case Enums.CharacterType.Warrior:
                    SquadBattleManager.Instance.warriorSkillDamagePercent.Clear();
                    for (var i = 0; i < prefab.Count; i++)
                    {
                        if (i != 0) // TODO : 두 번째 스킬 프리팹 추가되면 아래 else 구문만 사용
                        {
                            UIManager.Instance.squadSkillCoolTimerUI.warriorSkillCoolTimerUI[i].skillIcon.sprite =
                                SpriteManager.Instance.GetSkillSprite(type, prefab[i].skillIconIndex);
                        }
                        else
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
                    }

                    break;
                case Enums.CharacterType.Archer:
                    SquadBattleManager.Instance.archerSkillDamagePercent.Clear();
                    for (var i = 0; i < prefab.Count; i++)
                    {
                        if (i != 0) // TODO : 두 번째 스킬 프리팹 추가되면 아래 else 구문만 사용
                        {
                            UIManager.Instance.squadSkillCoolTimerUI.archerSkillCoolTimerUI[i].skillIcon.sprite =
                                SpriteManager.Instance.GetSkillSprite(type, prefab[i].skillIconIndex);
                        }
                        else
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
                    }

                    break;
                case Enums.CharacterType.Wizard:
                    SquadBattleManager.Instance.wizardSkillDamagePercent.Clear();
                    for (var i = 0; i < prefab.Count; i++)
                    {
                        if (i != 0) // TODO : 두 번째 스킬 프리팹 추가되면 아래 else 구문만 사용
                        {                     
                            UIManager.Instance.squadSkillCoolTimerUI.wizardSkillCoolTimerUI[i].skillIcon.sprite =
                                SpriteManager.Instance.GetSkillSprite(type, prefab[i].skillIconIndex);
                        }
                        else
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
                    }

                    break;
            }
        }

        private void AddCharacter(string characterID, Character character)
        {
            switch (character.characterType)
            {
                case Enums.CharacterType.Warrior:
                    if (!WarriorDictionary.TryAdd(characterID, character))
                        Debug.LogWarning($"Character already exists in the dictionary: {character}");

                    break;
                case Enums.CharacterType.Archer:
                    if (!ArchersDictionary.TryAdd(characterID, character))
                        Debug.LogWarning($"Character already exists in the dictionary: {character}");

                    break;
                case Enums.CharacterType.Wizard:
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
                Enums.CharacterType.Warrior => WarriorDictionary[characterId],
                Enums.CharacterType.Archer => ArchersDictionary[characterId],
                Enums.CharacterType.Wizard => WizardsDictionary[characterId],
                _ => throw new ArgumentOutOfRangeException()
            };

            if (targetCharacter == null) return;

            targetCharacter.characterLevel = character.characterLevel;

            for (var i = 0; i < targetCharacter.characterOwnedEffects.Count; i++)
                targetCharacter.characterOwnedEffects[i].increaseValue =
                    character.characterOwnedEffects[i].increaseValue;

            targetCharacter.SaveCharacterEquippedInfo(targetCharacter.characterId);
        }
    }
}