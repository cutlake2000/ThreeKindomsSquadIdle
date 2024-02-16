using System;
using System.Numerics;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BigInteger = Keiwando.BigInteger.BigInteger;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.DungeonPanel
{
    public class DungeonItemUI : MonoBehaviour
    {
        public Enums.DungeonType dungeonType;
        public Enums.CurrencyType rewardType;
        public Button clearDungeonButton;
        public Button enterDungeonButton;
        public Button enterLockDungeonButton;
        public TMP_Text currentDungeonLevelText;
        public TMP_Text currentDungeonRewardText;

        public void UpdateDungeonItemUI(int level, BigInteger reward)
        {
            currentDungeonLevelText.text = $"{level} 단계";

            switch (rewardType)
            {
                case Enums.CurrencyType.Gold:
                    currentDungeonRewardText.text = $"+ <sprite={(int)Enums.IconType.Gold}> {reward.ChangeMoney()}";
                    break;
                case Enums.CurrencyType.SquadEnhanceStone:
                    currentDungeonRewardText.text = $"+ <sprite={(int)Enums.IconType.EnhanceStoneSquad}> {reward.ChangeMoney()}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void UpdateButtonUI(bool active)
        {
            enterDungeonButton.gameObject.SetActive(active);
            enterLockDungeonButton.gameObject.SetActive(!active);
        }
    }
}