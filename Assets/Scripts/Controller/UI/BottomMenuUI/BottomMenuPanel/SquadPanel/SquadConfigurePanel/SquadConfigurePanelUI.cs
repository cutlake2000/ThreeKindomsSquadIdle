using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Creature.Data;
using Data;
using Function;
using Keiwando.BigInteger;
using Managers;
using Managers.BattleManager;
using Managers.BottomMenuManager.SquadPanel;
using Managers.GameManager;
using Module;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadConfigurePanel
{
    public class SquadConfigurePanelUI : MonoBehaviour
    {
        public static event Action<Character> OnClickSquadConfigureItem;
        
        [Header("구성 중인 스쿼드를 보여주는 패널")] public GameObject configuredSquadPanel;
        [Header("선택한 캐릭터 정보를 띄워주는 패널")] public GameObject selectedSquadPanel;
        [Header("구성 중인 스쿼드 패널의 캐릭터 스폰 좌표")]public GameObject[] characterSpawnPosition;
        [Header("캐릭터 선택 마크")] public GameObject[] characterSelectMark; 
        [Header("선택 영웅")]
        public Character currentSelectedSquadConfigurePanelItem;
        [Header("선택 영웅 인덱스")]
        public int currentSelectedSquadConfigurePanelItemIndex;

        [Header("선택 영웅 정보")]
        public Image selectedCharacterIcon;
        public TMP_Text selectedCharacterName;
        public TMP_Text selectedCharacterRarity;
        public TMP_Text selectedCharacterLevel;
        public TMP_Text selectedCharacterEquippedEffect1; 
        public TMP_Text selectedCharacterEquippedEffect2;
        public TMP_Text selectedCharacterOwnedEffect1;
        public TMP_Text selectedCharacterOwnedEffect2;
        public Image selectedCharacter1SkillIcon;
        public TMP_Text selectedCharacter1SkillName;
        public TMP_Text selectedCharacter1SkillDescription;
        public TMP_Text selectedCharacter1SkillCoolTime;
        public Image selectedCharacter2SkillIcon;
        public TMP_Text selectedCharacter2SkillName;
        public TMP_Text selectedCharacter2SkillDescription;
        public TMP_Text selectedCharacter2SkillCoolTime;
        public Button selectedCharacterLevelUpButton;
        public Button selectedCharacterLevelUpLockButton;
        public Button selectedCharacterSelectButton;
        public Button selectedCharacterAlreadySelectedButton;
        public TMP_Text requiredSquadEnhanceStoneText;
        public TMP_Text requiredSquadEnhanceStoneTextLockButton;

        [Header("워리어 / 아처 / 위자드 스크롤뷰")] public GameObject[] squadScrollViewPanel;

        [Header("스쿼드 구성 패널 아이템 UI")]
        public List<GameObject> squadConfigureScrollViewItemWarriors = new();
        public List<GameObject> squadConfigureScrollViewItemArchers = new();
        public List<GameObject> squadConfigureScrollViewItemWizards = new();

        [Header("워리어 / 아처 / 위자드 스크롤뷰 전환 버튼")]
        public Button[] squadScrollViewPanelButtons;
        public Button[] squadScrollViewPanelLockButtons;

        public Character previousWarrior;
        public Character previousArcher;
        public Character previousWizard;
        
        [Header("업데이트 예정 캐릭터 알림 버튼")]
        public Button[] willBeUpdatedAlertButtons;

        private void OnEnable()
        {
            OnClickSquadConfigureItem += UpdateSquadConfigurePanelSelectedCharacterInfoUI;
        }

        private void OnDisable()
        {
            OnClickSquadConfigureItem -= UpdateSquadConfigurePanelSelectedCharacterInfoUI;
            
            if (!SquadConfigureManager.Instance.isSquadConfigureChanged) return;
            SquadConfigureManager.Instance.isSquadConfigureChanged = false;
            
            foreach (var equippedEffect in previousWarrior.characterEquippedEffects)
            {
                SquadBattleManager.Instance.squadEntireStat.UpdateStat((Enums.SquadStatType)Enum.Parse(typeof(Enums.SquadStatType), equippedEffect.statType.ToString()), -equippedEffect.increaseValue, equippedEffect.increaseStatType == Enums.IncreaseStatValueType.BaseStat);   
            }
            
            foreach (var equippedEffect in previousArcher.characterEquippedEffects)
            {
                SquadBattleManager.Instance.squadEntireStat.UpdateStat((Enums.SquadStatType)Enum.Parse(typeof(Enums.SquadStatType), equippedEffect.statType.ToString()), -equippedEffect.increaseValue, equippedEffect.increaseStatType == Enums.IncreaseStatValueType.BaseStat);   
            }
            
            foreach (var equippedEffect in previousWizard.characterEquippedEffects)
            {
                SquadBattleManager.Instance.squadEntireStat.UpdateStat((Enums.SquadStatType)Enum.Parse(typeof(Enums.SquadStatType), equippedEffect.statType.ToString()), -equippedEffect.increaseValue, equippedEffect.increaseStatType == Enums.IncreaseStatValueType.BaseStat);   
            }
            
            StageManager.Instance.StopStageRunner();
            
            SquadConfigureManager.Instance.UpdateSquadConfigureModelOnBattle(SquadConfigureManager.Instance.FindEquippedCharacter(Enums.CharacterType.Warrior));
            SquadConfigureManager.Instance.UpdateSquadConfigureModelOnBattle(SquadConfigureManager.Instance.FindEquippedCharacter(Enums.CharacterType.Archer));
            SquadConfigureManager.Instance.UpdateSquadConfigureModelOnBattle(SquadConfigureManager.Instance.FindEquippedCharacter(Enums.CharacterType.Wizard));
            
            SquadConfigureManager.Instance.UpdateSquadConfigureModelOnMenu(SquadConfigureManager.Instance.FindEquippedCharacter(Enums.CharacterType.Warrior));
            SquadConfigureManager.Instance.UpdateSquadConfigureModelOnMenu(SquadConfigureManager.Instance.FindEquippedCharacter(Enums.CharacterType.Archer));
            SquadConfigureManager.Instance.UpdateSquadConfigureModelOnMenu(SquadConfigureManager.Instance.FindEquippedCharacter(Enums.CharacterType.Wizard));
            
            StageManager.Instance.initializeStageResultChecker = true;
            StageManager.Instance.prepareNewSubStage = true;
                
            StageManager.Instance.StartStageRunner();
        }

        public void UpdateSquadConfigurePanelSelectedCharacterInfoUI(Character character)
        {
            currentSelectedSquadConfigurePanelItem = character;

            requiredSquadEnhanceStoneText.text = $"<sprite={(int)Enums.IconType.EnhanceStoneSquad}>{character.RequiredCurrencyForLevelUp().ChangeMoney()}";
            requiredSquadEnhanceStoneTextLockButton.text = requiredSquadEnhanceStoneText.text;
            
            var skill1Description = currentSelectedSquadConfigurePanelItem.characterSkills[0].skillDescription;
            var skill2Description = currentSelectedSquadConfigurePanelItem.characterSkills[1].skillDescription;

            var skill1NewDescription = skill1Description.Split('n', 2);
            var skill2NewDescription = skill2Description.Split('n', 2);

            selectedCharacterIcon.sprite = SpriteManager.Instance.GetCharacterSprite(
                currentSelectedSquadConfigurePanelItem.characterType,
                currentSelectedSquadConfigurePanelItem.characterIconIndex);
            selectedCharacterName.text = currentSelectedSquadConfigurePanelItem.characterName;
            selectedCharacterRarity.text = currentSelectedSquadConfigurePanelItem.characterRarity switch
            {
                Enums.CharacterRarity.Magic => "매직",
                Enums.CharacterRarity.Rare => "레어",
                Enums.CharacterRarity.Unique => "유니크",
                Enums.CharacterRarity.Legend => "레전드",
                _ => throw new ArgumentOutOfRangeException()
            };
            selectedCharacterLevel.text =
                $"Lv. {currentSelectedSquadConfigurePanelItem.characterLevel} / {SquadConfigureManager.CharacterMaxLevel}";
            selectedCharacterOwnedEffect1.text =
                SetCharacterEffectDescriptionToString(currentSelectedSquadConfigurePanelItem, true, 0);
            selectedCharacterOwnedEffect2.text =
                SetCharacterEffectDescriptionToString(currentSelectedSquadConfigurePanelItem, true, 1);
            selectedCharacterEquippedEffect1.text =
                SetCharacterEffectDescriptionToString(currentSelectedSquadConfigurePanelItem, false, 0);
            selectedCharacterEquippedEffect2.text =
                SetCharacterEffectDescriptionToString(currentSelectedSquadConfigurePanelItem, false, 1);
            
            selectedCharacter1SkillIcon.sprite = SpriteManager.Instance.GetSkillSprite(
                currentSelectedSquadConfigurePanelItem.characterType,
                currentSelectedSquadConfigurePanelItem.characterSkills[0].skillIconIndex);
            selectedCharacter1SkillName.text = currentSelectedSquadConfigurePanelItem.characterSkills[0].skillName;
            selectedCharacter1SkillDescription.text =
                $"{skill1NewDescription[0]}{currentSelectedSquadConfigurePanelItem.characterSkills[0].skillDamagePercent}{skill1NewDescription[1]}";
            selectedCharacter1SkillCoolTime.text =
                $"쿨타임 {currentSelectedSquadConfigurePanelItem.characterSkills[0].maxSkillCoolTime}초";
            
            selectedCharacter2SkillIcon.sprite = SpriteManager.Instance.GetSkillSprite(
                currentSelectedSquadConfigurePanelItem.characterType,
                currentSelectedSquadConfigurePanelItem.characterSkills[1].skillIconIndex);
            selectedCharacter2SkillName.text = currentSelectedSquadConfigurePanelItem.characterSkills[1].skillName;
            
            // TODO: 두 번째 스킬 프리팹 정해지면 아래 주석처리한 내용만 사용
            selectedCharacter2SkillDescription.text = $"해금이 필요합니다.";
            selectedCharacter2SkillCoolTime.text = $"쿨타임 00초";
            // selectedCharacter2SkillDescription.text =
            //     $"{skill2NewDescription[0]}{currentSelectedSquadConfigurePanelItem.characterSkills[1].skillDamagePercent}{skill2NewDescription[1]}";
            // selectedCharacter2SkillCoolTime.text =
            //     $"쿨타임 {currentSelectedSquadConfigurePanelItem.characterSkills[1].maxSkillCoolTime}초";

            if (currentSelectedSquadConfigurePanelItem.isPossessed)
            {
                selectedCharacterLevelUpButton.gameObject.SetActive(true);
                selectedCharacterSelectButton.gameObject.SetActive(!currentSelectedSquadConfigurePanelItem.isEquipped);
                selectedCharacterAlreadySelectedButton.gameObject.SetActive(currentSelectedSquadConfigurePanelItem.isEquipped);   
            }
            else
            {
                selectedCharacterLevelUpButton.gameObject.SetActive(false);
                selectedCharacterSelectButton.gameObject.SetActive(false);
                selectedCharacterAlreadySelectedButton.gameObject.SetActive(false);   
            }
        }

        private string SetCharacterEffectDescriptionToString(Character character, bool isOwnedEffect, int index)
        {
            var stringBuilder = new StringBuilder();

            if (isOwnedEffect)
            {
                var statType = character.characterOwnedEffects[index].statType switch
                {
                    Enums.SquadStatType.Attack => "공격력 ",
                    Enums.SquadStatType.Health => "체력 ",
                    _ => throw new ArgumentOutOfRangeException()
                };

                var increaseStatType = character.characterOwnedEffects[index].increaseStatType switch
                {
                    Enums.IncreaseStatValueType.BaseStat =>
                        $"{character.characterOwnedEffects[index].increaseValue} 증가",
                    Enums.IncreaseStatValueType.PercentStat =>
                        $"{UIManager.FormatCurrency(character.characterOwnedEffects[index].increaseValue)}% 증가",
                    _ => throw new ArgumentOutOfRangeException()
                };


                stringBuilder.Append(statType);
                stringBuilder.Append(increaseStatType);
            }
            else
            {
                var statType = character.characterEquippedEffects[index].statType switch
                {
                    Enums.SquadStatType.Attack => "공격력 ",
                    Enums.SquadStatType.Health => "체력 ",
                    _ => throw new ArgumentOutOfRangeException()
                };

                var increaseStatType = character.characterEquippedEffects[index].increaseStatType switch
                {
                    Enums.IncreaseStatValueType.BaseStat =>
                        $"{character.characterEquippedEffects[index].increaseValue} 증가",
                    Enums.IncreaseStatValueType.PercentStat =>
                        $"{UIManager.FormatCurrency(character.characterEquippedEffects[index].increaseValue)}% 증가",
                    _ => throw new ArgumentOutOfRangeException()
                };


                stringBuilder.Append(statType);
                stringBuilder.Append(increaseStatType);
            }

            return stringBuilder.ToString();
        }

        public void InitializeEventListeners()
        {
            for (var i = 0; i < squadScrollViewPanelLockButtons.Length; i++)
            {
                var index = i;
                squadScrollViewPanelLockButtons[i].GetComponent<Button>().onClick
                    .AddListener(() => InitializeSquadPanelButton(index));
            }

            selectedCharacterLevelUpButton.onClick.AddListener(OnClickCharacterLevelUp);
            selectedCharacterLevelUpButton.GetComponent<HoldButton>().onHold.AddListener(OnClickCharacterLevelUp);
            selectedCharacterLevelUpLockButton.GetComponent<LockButtonUI>().InitializeEventListener();
            selectedCharacterSelectButton.onClick.AddListener(OnClickCharacterEquip);

            foreach (var button in willBeUpdatedAlertButtons)
            {
                button.GetComponent<LockButtonUI>().InitializeEventListener();
            }
        }

        private void InitializeSquadPanelButton(int index)
        {
            for (var i = 0; i < squadScrollViewPanel.Length; i++)
            {
                characterSelectMark[i].SetActive(i == index);
                squadScrollViewPanel[i].SetActive(i == index);

                UpdateSquadScrollViewPanelButtonUI(i, i == index);
            }

            var selectedCharacter = index switch
            {
                0 => SquadConfigureManager.Instance.WarriorDictionary[SquadConfigureManager.Instance.targetWarrior],
                1 => SquadConfigureManager.Instance.ArchersDictionary[SquadConfigureManager.Instance.targetArcher],
                2 => SquadConfigureManager.Instance.WizardsDictionary[SquadConfigureManager.Instance.targetWizard],
                _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
            };

            UpdateSquadConfigurePanelSelectedCharacterInfoUI(selectedCharacter);
        }

        public void UpdateLevelUpButtonUI()
        {
            if (currentSelectedSquadConfigurePanelItem.characterId == string.Empty) return;
            
            var character = currentSelectedSquadConfigurePanelItem.characterType switch
            {
                Enums.CharacterType.Warrior => SquadConfigureManager.Instance.WarriorDictionary[
                    currentSelectedSquadConfigurePanelItem.characterId],
                Enums.CharacterType.Archer => SquadConfigureManager.Instance.ArchersDictionary[
                    currentSelectedSquadConfigurePanelItem.characterId],
                Enums.CharacterType.Wizard => SquadConfigureManager.Instance.WizardsDictionary[
                    currentSelectedSquadConfigurePanelItem.characterId],
                _ => null
            };

            if (character is not { isPossessed: true }) return;

            var canLevelUp = character.characterLevel < SquadConfigureManager.CharacterMaxLevel && character.RequiredCurrencyForLevelUp() < new BigInteger(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.SquadEnhanceStone));

            selectedCharacterLevelUpButton.gameObject.SetActive(canLevelUp);
            selectedCharacterLevelUpLockButton.gameObject.SetActive(!canLevelUp);
        }

        public void UpdateSquadScrollViewPanelButtonUI(int index, bool active)
        {
            squadScrollViewPanelButtons[index].gameObject.SetActive(active);
            squadScrollViewPanelLockButtons[index].gameObject.SetActive(!active);
        }

        /// <summary>
        /// 선택한 캐릭터 레벨 업하는 메서드
        /// </summary>
        private void OnClickCharacterLevelUp()
        {
            var character = currentSelectedSquadConfigurePanelItem.characterType switch
            {
                Enums.CharacterType.Warrior => SquadConfigureManager.Instance.WarriorDictionary[
                    currentSelectedSquadConfigurePanelItem.characterId],
                Enums.CharacterType.Archer => SquadConfigureManager.Instance.ArchersDictionary[
                    currentSelectedSquadConfigurePanelItem.characterId],
                Enums.CharacterType.Wizard => SquadConfigureManager.Instance.WizardsDictionary[
                    currentSelectedSquadConfigurePanelItem.characterId],
                _ => null
            };

            if (character == null || !character.isPossessed) return;

            if (character.characterLevel >= SquadConfigureManager.CharacterMaxLevel) return;

            if (character.RequiredCurrencyForLevelUp() >
                new BigInteger(AccountManager.Instance.GetCurrencyAmount(Enums.CurrencyType.SquadEnhanceStone))) return;

            AccountManager.Instance.SubtractCurrency(Enums.CurrencyType.SquadEnhanceStone, character.RequiredCurrencyForLevelUp());
            QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.LevelUpCharacter, 1);
            
            character.CharacterLevelUp();
            character.SaveCharacterDataIntoES3Loader();
            UpdateSquadConfigurePanelSelectedCharacterInfoUI(character);
            UpdateSquadConfigureScrollViewItemUI(character.characterType, false);
            SquadConfigureManager.Instance.SaveAllCharacterInfo();
        }

        /// <summary>
        /// 선택한 캐릭터를 장착하는 메서드
        /// </summary>
        private void OnClickCharacterEquip()
        {
            Character newCharacter;
            switch (currentSelectedSquadConfigurePanelItem.characterType)
            {
                case Enums.CharacterType.Warrior:
                    newCharacter = SquadConfigureManager.Instance.WarriorDictionary[currentSelectedSquadConfigurePanelItem.characterId];
                    SquadConfigureManager.Instance.targetWarrior = currentSelectedSquadConfigurePanelItem.characterId;
                    break;
                case Enums.CharacterType.Archer:
                    newCharacter = SquadConfigureManager.Instance.ArchersDictionary[currentSelectedSquadConfigurePanelItem.characterId];
                    SquadConfigureManager.Instance.targetArcher = currentSelectedSquadConfigurePanelItem.characterId;
                    break;
                case Enums.CharacterType.Wizard:
                    newCharacter = SquadConfigureManager.Instance.WizardsDictionary[currentSelectedSquadConfigurePanelItem.characterId];
                    SquadConfigureManager.Instance.targetWizard = currentSelectedSquadConfigurePanelItem.characterId;
                    break;
                default:
                    newCharacter = null;
                    break;
            }

            if (newCharacter == null || newCharacter.isEquipped) return;
            
            QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.EquipSquad, 1);
            
            SquadConfigureManager.Instance.isSquadConfigureChanged = true;
            
            configuredSquadPanel.SetActive(true);
            selectedSquadPanel.SetActive(false);
            
            switch (newCharacter.characterType)
            {
                case Enums.CharacterType.Warrior:
                    previousWarrior = SquadConfigureManager.Instance.WarriorDictionary.First(character => character.Value.isEquipped).Value;
                    previousWarrior.isEquipped = false;
                    previousWarrior.SaveCharacterDataIntoES3Loader();
                    break;
                case Enums.CharacterType.Archer:
                    previousArcher = SquadConfigureManager.Instance.ArchersDictionary.First(character => character.Value.isEquipped).Value;
                    previousArcher.isEquipped = false;
                    previousArcher.SaveCharacterDataIntoES3Loader();
                    break;
                case Enums.CharacterType.Wizard:
                    previousWizard = SquadConfigureManager.Instance.WizardsDictionary.First(character => character.Value.isEquipped).Value;
                    previousWizard.isEquipped = false;
                    previousWizard.SaveCharacterDataIntoES3Loader();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            newCharacter.isEquipped = true;
            newCharacter.SaveCharacterDataIntoES3Loader();
            
            UpdateSquadConfigureScrollViewItemUI(newCharacter.characterType, false);
            SquadConfigureManager.Instance.InstantiateModelOfConfigureUnderParent(newCharacter.characterType, newCharacter.characterModel);
            SquadConfigureManager.Instance.SaveAllCharacterInfo();
        }

        public void UpdateSquadConfigureScrollViewItemUI(Enums.CharacterType characterType, bool initialSorting)
        {
            switch (characterType)
            {
                case Enums.CharacterType.Warrior:

                    if (initialSorting)
                    {
                        SquadConfigureManager.Instance.warriors = SquadConfigureManager.Instance.WarriorDictionary.Values.OrderBy(x => x.characterRarity).ToList();   
                    }
                    
                    for (var i = 0; i < SquadConfigureManager.Instance.warriors.Count; i++)
                    {
                        var index = i;
                        var characterId = SquadConfigureManager.Instance.warriors[index].characterId;
                        
                        squadConfigureScrollViewItemWarriors[index].GetComponent<SquadConfigurePanelItemUI>()
                            .UpdateSquadConfigureAllItemUI(SquadConfigureManager.Instance.warriors[index].characterLevel,
                                SquadConfigureManager.Instance.warriors[index].isEquipped,
                                SquadConfigureManager.Instance.warriors[index].isPossessed,
                                SquadConfigureManager.Instance.warriors[index].characterName,
                                SpriteManager.Instance.GetCharacterSprite(characterType,
                                    SquadConfigureManager.Instance.warriors[index].characterIconIndex));
                        
                        squadConfigureScrollViewItemWarriors[index].GetComponent<Button>().onClick.RemoveAllListeners();
                        squadConfigureScrollViewItemWarriors[index].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            if (!UIManager.Instance.squadPanelUI.squadConfigurePanelUI.selectedSquadPanel.activeInHierarchy)
                            {
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.configuredSquadPanel.SetActive(false);
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.selectedSquadPanel.SetActive(true);
                            }
                            else if (currentSelectedSquadConfigurePanelItem.characterName == squadConfigureScrollViewItemWarriors[index].GetComponent<SquadConfigurePanelItemUI>().characterName.text)
                            {
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.configuredSquadPanel.SetActive(true);
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.selectedSquadPanel.SetActive(false);
                            }
                            
                            UIManager.Instance.squadPanelUI.squadConfigurePanelUI
                                .currentSelectedSquadConfigurePanelItemIndex = index;
                            UIManager.Instance.squadPanelUI.squadConfigurePanelUI.SelectSquadConfigureItem(characterType, characterId);
                        });
                    }

                    break;
                case Enums.CharacterType.Archer:

                    if (initialSorting)
                    {
                        SquadConfigureManager.Instance.archers = SquadConfigureManager.Instance.ArchersDictionary.Values.OrderBy(x => x.characterRarity).ToList();   
                    }
                    
                    for (var i = 0; i < SquadConfigureManager.Instance.archers.Count; i++)
                    {
                        var index = i;
                        var characterId = SquadConfigureManager.Instance.archers[index].characterId;
                        
                        squadConfigureScrollViewItemArchers[index].GetComponent<SquadConfigurePanelItemUI>()
                            .UpdateSquadConfigureAllItemUI(SquadConfigureManager.Instance.archers[index].characterLevel,
                                SquadConfigureManager.Instance.archers[index].isEquipped,
                                SquadConfigureManager.Instance.archers[index].isPossessed,
                                SquadConfigureManager.Instance.archers[index].characterName,
                                SpriteManager.Instance.GetCharacterSprite(characterType,
                                    SquadConfigureManager.Instance.archers[index].characterIconIndex));
                        
                        squadConfigureScrollViewItemArchers[index].GetComponent<Button>().onClick.RemoveAllListeners();
                        squadConfigureScrollViewItemArchers[index].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            if (!UIManager.Instance.squadPanelUI.squadConfigurePanelUI.selectedSquadPanel.activeInHierarchy)
                            {
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.configuredSquadPanel.SetActive(false);
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.selectedSquadPanel.SetActive(true);
                            }
                            else if (currentSelectedSquadConfigurePanelItem.characterName == squadConfigureScrollViewItemArchers[index].GetComponent<SquadConfigurePanelItemUI>().characterName.text)
                            {
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.configuredSquadPanel.SetActive(true);
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.selectedSquadPanel.SetActive(false);
                                return;
                            }
                            
                            UIManager.Instance.squadPanelUI.squadConfigurePanelUI
                                .currentSelectedSquadConfigurePanelItemIndex = index;
                            UIManager.Instance.squadPanelUI.squadConfigurePanelUI.SelectSquadConfigureItem(
                                characterType, characterId);
                        });
                    
                    }

                    break;
                case Enums.CharacterType.Wizard:

                    if (initialSorting)
                    {
                        SquadConfigureManager.Instance.wizards = SquadConfigureManager.Instance.WizardsDictionary.Values.OrderBy(x => x.characterRarity).ToList();   
                    }
                    
                    for (var i = 0; i < SquadConfigureManager.Instance.wizards.Count; i++)
                    {
                        var index = i;
                        var characterId = SquadConfigureManager.Instance.wizards[index].characterId;
                        
                        squadConfigureScrollViewItemWizards[index].GetComponent<SquadConfigurePanelItemUI>()
                            .UpdateSquadConfigureAllItemUI(SquadConfigureManager.Instance.wizards[index].characterLevel,
                                SquadConfigureManager.Instance.wizards[index].isEquipped,
                                SquadConfigureManager.Instance.wizards[index].isPossessed,
                                SquadConfigureManager.Instance.wizards[index].characterName,
                                SpriteManager.Instance.GetCharacterSprite(characterType,
                                    SquadConfigureManager.Instance.wizards[index].characterIconIndex));
                        
                        squadConfigureScrollViewItemWizards[index].GetComponent<Button>().onClick.RemoveAllListeners();
                        squadConfigureScrollViewItemWizards[index].GetComponent<Button>().onClick.AddListener(() =>
                        {
                            if (UIManager.Instance.squadPanelUI.squadConfigurePanelUI.selectedSquadPanel.activeInHierarchy == false)
                            {
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.configuredSquadPanel.SetActive(false);
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.selectedSquadPanel.SetActive(true);
                            }
                            else if (currentSelectedSquadConfigurePanelItem.characterName == squadConfigureScrollViewItemWizards[index].GetComponent<SquadConfigurePanelItemUI>().characterName.text)
                            {
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.configuredSquadPanel.SetActive(true);
                                UIManager.Instance.squadPanelUI.squadConfigurePanelUI.selectedSquadPanel.SetActive(false);
                                return;
                            }
                            
                            UIManager.Instance.squadPanelUI.squadConfigurePanelUI
                                .currentSelectedSquadConfigurePanelItemIndex = index;
                            UIManager.Instance.squadPanelUI.squadConfigurePanelUI.SelectSquadConfigureItem(
                                characterType, characterId);
                        });
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(characterType), characterType, null);
            }
        }

        public void SelectSquadConfigureItem(Enums.CharacterType type, string key)
        {
            switch (type)
            {
                case Enums.CharacterType.Warrior:
                    OnClickSquadConfigureItem?.Invoke(SquadConfigureManager.Instance.WarriorDictionary[key]);
                    break;
                case Enums.CharacterType.Archer:
                    OnClickSquadConfigureItem?.Invoke(SquadConfigureManager.Instance.ArchersDictionary[key]);
                    break;
                case Enums.CharacterType.Wizard:
                    OnClickSquadConfigureItem?.Invoke(SquadConfigureManager.Instance.WizardsDictionary[key]);
                    break;
            }
        }
    }
}