using Data;
using Managers;
using Managers.BattleManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI
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
            loopButton.onClick.AddListener(() => OnClickStageProgressButton(true));
            challengeButton.onClick.AddListener(() => OnClickStageProgressButton(false));
            StageManager.CheckStageProgressType += SetStageProgressButton;
        }

        private void OnClickStageProgressButton(bool challenge)
        {
            SetStageProgressButton(challenge);
            StageManager.CheckStageProgressType.Invoke(challenge);
        }

        private void SetStageProgressButton(bool challenge)
        {
            loopButton.gameObject.SetActive(!challenge);
            challengeButton.gameObject.SetActive(challenge);
        }

        public void SetUIText(Enum.UITextType textType, string value)
        {
            switch (textType)
            {
                case Enum.UITextType.CurrentStageName:
                    currentStageNameText.text = value;
                    break;
                case Enum.UITextType.CurrentWave:
                    currentWaveText.text = value;
                    break;
                case Enum.UITextType.Timer:
                    timerText.text = value;
                    break;
            }
        }

        public void SetUISlider(Enum.UISliderType sliderType, float value)
        {
            switch (sliderType)
            {
                case Enum.UISliderType.CurrentWaveSlider:
                    currentWaveSlider.value = value;
                    break;
            }
        }
    }
}