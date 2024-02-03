using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.DungeonPanel
{
    public class DungeonItemUI : MonoBehaviour
    {
        public Enums.DungeonType dungeonType;
        public Enums.CurrencyType rewardType;
        public Button clearDungeonButton;
        public Button enterDungeonButton;
        public TMP_Text currentDungeonLevelText;
        public TMP_Text currentDungeonRewardText;

        public void UpdateDungeonItemUI(int level, string reward)
        {
            currentDungeonLevelText.text = $"{level} 단계";

            switch (rewardType)
            {
                case Enums.CurrencyType.Gold:
                    currentDungeonRewardText.text = $"+ <sprite={(int)Enums.IconType.Gold}> {reward}";
                    break;
                case Enums.CurrencyType.SquadEnhanceStone:
                    currentDungeonRewardText.text = $"+ <sprite={(int)Enums.IconType.EnhanceStoneSquad}> {reward}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}