using System;
using System.Collections.Generic;
using System.Globalization;
using Controller.UI;
using Data;
using Managers.GameManager;
using UnityEngine;

namespace PushOfflineReward.Scripts.OfflineReward
{
    public class OfflineRewardController : MonoBehaviour
    {
        [SerializeField] private float minTime;
        [SerializeField] private float maxTime = 10800;
        [SerializeField] private float killCountPerSecond = 0.1f;
        
        private List<Reward> rewards;

        private float timePassed;
    
        // private UIManager uiManager;
        // private PushManager pushManager;
        // private OffLineRewardUI offLineRewardUI;
        
        // private RewardManager rewardManager;
    
        private DateTime startTime;
        private string startTimeString;
    
        public void InitKey()
        {
            if (!PlayerPrefs.HasKey("OfflineTimerStr" + Application.productName)) TimeReset();
            else ResetKey();
        }

        public void ResetKey()
        {
            TimeLoad();
    
            var ts = DateTime.Now - startTime;
            timePassed = (float)ts.TotalSeconds;
            
            ManagePushRewards();
    
            if (timePassed >= minTime)   //10 Minutes 600  //30 Minutes 1800  // 3시간 10800  //4시간 14400
            {
                OfflinePanelOpen();
                TimeReset();
                Debug.Log("OfflineTimer" + " + " + timePassed);
            }
            else
            {
                Debug.Log("OfflineTimer" + " + " + timePassed);
            }
        }
    
        private void TimeLoad()
        {
            startTimeString = PlayerPrefs.GetString("OfflineTimerStr" + Application.productName);
            startTime = DateTime.Parse(startTimeString);
        }

        public void TimeReset()
        {
            startTime = DateTime.Now;
            startTimeString = startTime.ToString(CultureInfo.CurrentCulture);
            PlayerPrefs.SetString("OfflineTimerStr" + Application.productName, startTimeString);
        }
    
        // 오프라인 타임이 특정시간 이상 지났다면 보상창 띄우기
        private void OfflinePanelOpen()
        {
            timePassed = Mathf.Min(timePassed, maxTime);
            var stageClearCount = (int) timePassed / 60;
    
            UIManager.Instance.offLineRewardUI.SetUI(stageClearCount, (int)timePassed, (int)maxTime);
            UIManager.Instance.offLineRewardUI.gameObject.SetActive(true);
        }
    
        // Ok확인 보상받고 패널끄기 
        public void OnClickBtn_Ok()
        {
            TimeReset(); // 타이머 리셋
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    
        private void ManagePushRewards()
        {
            const int timerSet = 1; // 1 - 초, 60 - 분, 3600 - 시간

            var pushDatas = PushManager.Instance.GetUnrecievedRewardDatas((int)timePassed / timerSet);
            
            rewards = new List<Reward>();
    
            foreach (var data in pushDatas)
            {
                PushManager.Instance.SetRewardRecieved(data.name);
                if (data.RewardType == Enums.RewardType.OfflineReward) continue;
    
                rewards.Add(new Reward(data.RewardType, data.Amount));
            }

            if (rewards.Count > 0)
            {
                UIManager.Instance.pushRewardUI.SetUI(rewards);
            }
        }
    }
}