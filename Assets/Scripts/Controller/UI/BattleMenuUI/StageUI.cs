using Data;
using Managers.BattleManager;
using Managers.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BattleMenuUI
{
    public class StageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentStageNameText;
        [SerializeField] private TextMeshProUGUI currentWaveText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Slider currentWaveSlider;
        [SerializeField] private Button loopButton;
        [SerializeField] private Button challengeButton;

        private void Start()
        {
            loopButton.onClick.AddListener(() =>
            {
                OnClickStageProgressButton(true);
                QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.TouchLoopButton, 1);
            });
              
            challengeButton.onClick.AddListener(() =>
            {
                OnClickStageProgressButton(false);
                QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.TouchChallengeButton, 1);
            });
            
            StageManager.CheckStageProgressType += SetStageProgressButton;
        }

        public void OnClickStageProgressButton(bool challenge)
        {
            StageManager.CheckStageProgressType.Invoke(challenge);
            ES3.Save($"{nameof(StageManager.Instance.challengeProgress)}", challenge);
        }

        public void SetStageProgressButton(bool challenge)
        {
            loopButton.gameObject.SetActive(challenge == false);
            challengeButton.gameObject.SetActive(challenge);
        }

        public void SetUIText(Enums.UITextType textType, string value)
        {
            switch (textType)
            {
                case Enums.UITextType.CurrentStageName:
                    currentStageNameText.text = value;
                    break;
                case Enums.UITextType.CurrentWave:
                    currentWaveText.text = value;
                    break;
                case Enums.UITextType.Timer:
                    timerText.text = value;
                    break;
            }
        }

        public void SetUISlider(Enums.UISliderType sliderType, float value)
        {
            switch (sliderType)
            {
                case Enums.UISliderType.CurrentWaveSlider:
                    currentWaveSlider.value = value;
                    break;
            }
        }
    }
}