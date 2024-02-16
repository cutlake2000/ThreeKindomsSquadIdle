using System.Collections.Generic;
using Data;
using Keiwando.BigInteger;
using Managers.GameManager;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public struct Reward
    {
        public Enums.RewardType rewardType;
        public BigInteger amount;
    
        public Reward(Enums.RewardType rewardType, BigInteger amount)
        {
            this.rewardType = rewardType;
            this.amount = amount;
        }
    }

    public class UI_OffLineReward : MonoBehaviour
    {
        [SerializeField] Button confirmBtn;

        [SerializeField] Text timeSpanText;
        [SerializeField] Slider timeSpanSlider;
        [SerializeField] Text monsterCountText;
        [SerializeField] Transform slotArea;

        private StageManager stageManager;
        // private RewardManager rewardManager;

        private List<Reward> rewards;

        public void Initialize()
        {
            SetReferences();
            AddCallbacks();
        }

        private void SetReferences()
        {
            stageManager = StageManager.Instance;
            // rewardManager = RewardManager.Instance;
        }

        private void AddCallbacks()
        {
            confirmBtn.onClick.AddListener(GiveReward);
        }

        public void SetUI(int killCount, int timePasssed)
        {
            int hour = timePasssed / 3600;
            int minute = (timePasssed % 3600) / 60;
            int second = ((timePasssed % 3600) % 60);

            string time = "";
            if (hour > 0) time += $"{hour}시간 ";
            if (minute > 0) time += $"{minute}분 ";
            if (second > 0) time += $"{second}초";

            timeSpanText.text = time;
            timeSpanSlider.value = (float)timePasssed / 10800;

            monsterCountText.text = $"{killCount}";

            SetReward(killCount);
            SetSlots();
        }

        private void SetReward(int killCount)
        {
            // rewards = stageManager.GetCurrentReward(killCount);
        }

        private void SetSlots()
        {
            foreach(Reward reward in rewards)
            {
                // RewardBaseSO data = rewardManager.GetRewardBaseData(reward.rewardType);
                // RewardSlot slot = data.GetRewardSlot(reward);
                // slot.transform.SetParent(slotArea);
            }
        }


        private void GiveReward()
        {
            // rewardManager.GiveReward(rewards,Strings.RewardTitle.OFFLINE_REWARD);
            // CloseUI();
        }
    }
}