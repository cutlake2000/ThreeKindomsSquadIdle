using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Controller.UI
{
    [Serializable]
    public struct SkillSlotUI
    {
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

                SquadManager.Instance.runAutoSkill = false;
            }
            else
            {
                stopAutoSkill.SetActive(false);
                runAutoSkill.SetActive(true);
                
                SquadManager.Instance.runAutoSkill = true;
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

        public void InitializeEventListener()
        {
            skillAutoUseButton.InitializeEventListener();
        }
        
        public void SetSkillCoolTimerUIIcon(Enum.SquadClassType squadClassType, Sprite currentCharacterSprite)
        {
            switch (squadClassType)
            {
                case Enum.SquadClassType.Warrior:
                    warriorIcon.sprite = currentCharacterSprite;
                    break;
                case Enum.SquadClassType.Archer:
                    archerIcon.sprite = currentCharacterSprite;
                    break;
                case Enum.SquadClassType.Wizard:
                    wizardIcon.sprite = currentCharacterSprite;
                    break;
            }
        }
    }
}