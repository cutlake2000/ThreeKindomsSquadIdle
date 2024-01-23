using Function;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Enum = Data.Enum;

namespace Controller.UI.BottomMenuUI.DungeonPanel
{
    public class DungeonItemUI : MonoBehaviour
    {
        public Enum.DungeonType dungeonType;
        public Button clearDungeonButton;
        public Button enterDungeonButton;
        public Toggle autoChallengeButton;
        public Button previousStageButton;
        public Button nextStageButton;
        public TMP_Text currentDungeonLevelText;
        public TMP_Text currentDungeonRewardText;

        public int currentDungeonLevel;
        public BigInteger currentDungeonReward;
        
        public void SetSquadStatUI()
        {
            currentDungeonLevelText.text = $"{currentDungeonLevel} 단계";
            currentDungeonRewardText.text = $"+ <sprite=1> {currentDungeonReward}";
        }
        public void UpdateIncreaseDungeonUI()
        {
            currentDungeonLevelText.text = $"{currentDungeonLevel} 단계";
            currentDungeonRewardText.text = $"+ <sprite=1> {currentDungeonReward}";
        }

        public void UpdateDungeonLevel(int level, int baseReward, int increaseRewardPercent)
        {
            currentDungeonLevel = level;
            currentDungeonReward = baseReward * baseReward / 100 * increaseRewardPercent;
            
            ES3.Save($"{nameof(Enum.DungeonType)}/{dungeonType}/currentLevel : ", currentDungeonLevel);
        }
    }
}