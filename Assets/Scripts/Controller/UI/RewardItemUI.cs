using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI
{
    public class RewardItemUI : MonoBehaviour
    {
        [SerializeField] private Enums.RewardType rewardType;
        [SerializeField] private Image rewardIcon;
        [SerializeField] private TMP_Text rewardAmountText;

        public void UpdateRewardItemUI(Enums.RewardType type, Sprite icon, string typeString, string amount, bool multiply)
        {
            rewardType = type;
            rewardIcon.sprite = icon;
            var operatorString = multiply switch
            {
                true => "x",
                false => "+"
            };
            rewardAmountText.text = $"{typeString}\n{operatorString}{amount}";
        }
    }
}