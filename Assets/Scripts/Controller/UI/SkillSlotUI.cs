using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Controller.UI
{
    [Serializable]
    public struct SkillSlotUI
    {
        [Header("스킬 슬롯 버튼")]
        public Button skillButton;
        
        [Header("스킬 쿨타임")]
        public GameObject coolTimer;
        
        [Header("스킬 쿨타임 잔여 시간")]
        public TMP_Text coolTimeText;
        
        [Header("스킬 쿨타임 슬라이더")]
        public Image coolTimeSlider;

        public void ActivateSkillCoolTimer(bool isReady)
        {
            coolTimer.SetActive(!isReady);
        }

        public void UpdateSkillCoolTimerText(float remainedTime, float maxTime)
        {
            coolTimeSlider.fillAmount = (maxTime - remainedTime) / maxTime;
            coolTimeText.text = remainedTime.ToString("N1");
        }
    }

    [Serializable]
    public struct SkillAutoUseButtonUI
    {
        public Button skillAutoUseButton;
        public GameObject stopAutoSkill;
        public GameObject runAutoSkill;

        public void InitializeEventListener()
        {
            skillAutoUseButton.onClick.AddListener(RunAutoSkill);
        }

        public void RunAutoSkill()
        {
            if (runAutoSkill.activeInHierarchy)
            {
                stopAutoSkill.SetActive(true);
                runAutoSkill.SetActive(false);

                SquadBattleManager.Instance.autoSkill = false;
            }
            else
            {
                stopAutoSkill.SetActive(false);
                runAutoSkill.SetActive(true);
                
                SquadBattleManager.Instance.autoSkill = true;
            }
        }
    }

    [Serializable]
    public struct SquadSkillCoolTimerUI
    {
        [Header("=== Auto 버튼 ===")]
        public SkillAutoUseButtonUI skillAutoUseButton;

        [Header("=== 스쿼드 스킬 쿨타임 UI ===")]
        [Header("--- 스쿼드 아이콘 ---")]
        public Image warriorIcon;
        public Image archerIcon;
        public Image wizardIcon;
        [Space(5)]
        [Header("--- 스킬 쿨타임 UI ---")]
        public SkillSlotUI[] warriorSkillCoolTimerUI;
        public SkillSlotUI[] archerSkillCoolTimerUI;
        public SkillSlotUI[] wizardSkillCoolTimerUI;

        public void InitializeEventListeners()
        {
            skillAutoUseButton.InitializeEventListener();

            for (var i = 0; i < 2; i++)
            {
                var index = i;
                warriorSkillCoolTimerUI[i].skillButton.onClick.AddListener(() => ActivateSkill(Enum.CharacterType.Warrior, index));
                archerSkillCoolTimerUI[i].skillButton.onClick.AddListener(() => ActivateSkill(Enum.CharacterType.Archer, index));
                wizardSkillCoolTimerUI[i].skillButton.onClick.AddListener(() => ActivateSkill(Enum.CharacterType.Wizard, index));
            }
        }

        private static void ActivateSkill(Enum.CharacterType characterType, int index)
        {
            switch (characterType)
            {
                case Enum.CharacterType.Warrior:
                    if (!SquadBattleManager.Instance.warriorSkillCoolTimer[index].isSkillReady) return;
                    SquadBattleManager.Instance.warriorSkillCoolTimer[index].orderToInstantiate = true;
                    break;
                case Enum.CharacterType.Archer:
                    if (!SquadBattleManager.Instance.archerSkillCoolTimer[index].isSkillReady) return;
                    SquadBattleManager.Instance.archerSkillCoolTimer[index].orderToInstantiate = true;
                    break;
                case Enum.CharacterType.Wizard:
                    if (!SquadBattleManager.Instance.wizardSkillCoolTimer[index].isSkillReady) return;
                    SquadBattleManager.Instance.wizardSkillCoolTimer[index].orderToInstantiate = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(characterType), characterType, null);
            }
        }

        public void SetSkillCoolTimerUIIcon(Enum.CharacterType characterType, Sprite currentCharacterSprite)
        {
            switch (characterType)
            {
                case Enum.CharacterType.Warrior:
                    warriorIcon.sprite = currentCharacterSprite;
                    break;
                case Enum.CharacterType.Archer:
                    archerIcon.sprite = currentCharacterSprite;
                    break;
                case Enum.CharacterType.Wizard:
                    wizardIcon.sprite = currentCharacterSprite;
                    break;
            }
        }
    }
}