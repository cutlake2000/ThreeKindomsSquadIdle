using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BattleMenuUI
{
    public class DungeonUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentDungeonNameText;
        [SerializeField] private Slider currentSlider;
        [SerializeField] private TMP_Text currentSliderText;
        [SerializeField] private TMP_Text currentTimerText;
        
        public void UpdateDungeonAllUI(string name, string progress, float value)
        {
            currentDungeonNameText.text = name;
            currentSliderText.text = progress;
            currentSlider.value = value;
        }
        
        public void UpdateDungeonSliderUI(string progress, float value)
        {
            currentSlider.value = value;
            currentSliderText.text = progress;
        }

        public void UpdateDungeonTimerUI(string time)
        {
            currentTimerText.text = time;
        }
    }
}