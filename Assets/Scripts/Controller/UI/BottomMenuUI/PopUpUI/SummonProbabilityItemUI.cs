using TMPro;
using UnityEngine;

namespace Controller.UI.BottomMenuUI.PopUpUI
{
    public class SummonProbabilityItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text probabilityText;

        public void UpdateSummonProbabilityItemUI(float probability)
        {
            probabilityText.text = $"{probability} %";
        }
    }
}