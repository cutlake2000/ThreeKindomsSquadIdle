using System;
using System.Collections.Generic;
using Data;
using Keiwando.BigInteger;
using Managers.BattleManager;
using Managers.GameManager;
using TMPro;
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

    public class OffLineRewardUI : MonoBehaviour
    {
        [SerializeField] private Button confirmBtn;

        [SerializeField] private TMP_Text timeSpanText;
        [SerializeField] private Transform slotArea;

        [SerializeField] private RewardItemUI[] rewardSlots;

        private List<Reward> rewards;

        public void InitializeEventListener()
        {
            confirmBtn.onClick.AddListener(GiveReward);
        }

        public void SetUI(int clearCount, int passedTime)
        {
            gameObject.SetActive(true);
            
            var hour = passedTime / 3600;
            var minute = passedTime % 3600 / 60;
            var second = passedTime % 3600 % 60;

            var time = "";

            if (hour > 0) time += $"{hour}시간 ";
            if (minute > 0) time += $"{minute}분 ";
            if (second > 0) time += $"{second}초";

            timeSpanText.text = time;

            SetReward(clearCount);
            SetSlots();
        }

        private void SetReward(int clearCount)
        {
            rewards = StageManager.Instance.GetCurrentStageClearReward(clearCount);
        }

        private void SetSlots()
        {
            for (var i = 0; i < rewards.Count; i++)
            {
                var reward = rewards[i];
                var type = (Enums.CurrencyType)Enum.Parse(typeof(Enums.CurrencyType), reward.rewardType.ToString());
                var typeString = type switch
                {
                    Enums.CurrencyType.Gold => "골드",
                    Enums.CurrencyType.Dia => "다이아",
                    Enums.CurrencyType.Exp => "경험치",
                    _ => throw new ArgumentOutOfRangeException()
                };
                var amount = reward.amount.ChangeMoney();

                rewardSlots[i].UpdateRewardItemUI(reward.rewardType, SpriteManager.Instance.GetCurrencySprite(type), typeString, amount);
                rewardSlots[i].gameObject.SetActive(true);
            }
        }


        private void GiveReward()
        {
            foreach (var reward in rewards)
            {
                if (reward.rewardType != Enums.RewardType.Rare_5_Sword)
                {
                    AccountManager.Instance.AddCurrency(
                        (Enums.CurrencyType)Enum.Parse(typeof(Enums.CurrencyType), reward.rewardType.ToString()),
                        reward.amount);
                }
                else
                {
                    Debug.Log("무기 지급"); //TODO : 오프라인 무기 보상
                }
            }

            gameObject.SetActive(false);
        }
    }
}