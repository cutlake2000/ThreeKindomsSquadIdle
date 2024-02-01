using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.DungeonPanel
{
    public class DungeonItemUI : MonoBehaviour
    {
        public Enums.DungeonType dungeonType;
        public Button clearDungeonButton;
        public Button enterDungeonButton;
        public TMP_Text currentDungeonLevelText;
        public TMP_Text currentDungeonRewardText;

        public int clearDungeonLevel;
        public int currentDungeonLevel;
        public int currentDungeonReward; //TODO :BigInteger로 수정

        public void SetDungeonUI()
        {
            currentDungeonLevelText.text = $"{currentDungeonLevel} 단계";
            currentDungeonRewardText.text = $"+ <sprite={(int)Enums.IconType.DungeonKeyGold}> {currentDungeonReward}";
        }

        public void UpdateIncreaseDungeonUI()
        {
            currentDungeonLevelText.text = $"{currentDungeonLevel} 단계";
            currentDungeonRewardText.text = $"+ <sprite={(int)Enums.IconType.DungeonKeyGold}> {currentDungeonReward}";
        }

        public void UpdateDungeonLevel(int level, int baseReward, int increaseRewardPercent)
        {
            currentDungeonLevel = level;
            currentDungeonReward = baseReward * baseReward / 100 * increaseRewardPercent;

            ES3.Save($"{nameof(Enums.DungeonType)}/{dungeonType}/currentLevel : ", currentDungeonLevel);
        }
    }
}