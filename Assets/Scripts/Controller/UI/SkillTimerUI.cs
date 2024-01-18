using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Controller.UI
{
    [Serializable]
    public struct SkillSlot
    {
        public Image skillCoolTimeSlider;
        public GameObject skillCoolTimer;
        public TMP_Text skillCoolTimeText;
    }
    
    public class SkillTimerUI : MonoBehaviour
    {
        public static SkillTimerUI Instance;

        [Header("=== 스킬 타이머 ===")]
        [Header("워리어")]
        [SerializeField] private SkillSlot[] warriorSkillSlots;

        [Space(5)]
        [Header("아처")]
        [SerializeField] private SkillSlot[] archerSkillSlots;

        [Space(5)]
        [Header("위자드")]
        [SerializeField] private SkillSlot[] wizardSkillSlots;

        private void Awake()
        {
            Instance = this;
        }

        public void ActivateSkillTimer(Enum.SquadClassType type, int index, bool isReady)
        {
            switch (type)
            {
                case Enum.SquadClassType.Warrior:
                    warriorSkillSlots[index].skillCoolTimer.SetActive(!isReady);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void SetSkillTimerText(Enum.SquadClassType type, int index, float remainedTime, float maxTime)
        {
            switch (type)
            {
                case Enum.SquadClassType.Warrior:
                    warriorSkillSlots[index].skillCoolTimeSlider.fillAmount = (maxTime - remainedTime) / maxTime;
                    warriorSkillSlots[index].skillCoolTimeText.text = remainedTime.ToString("N1");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}