using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Data;
using Keiwando.BigInteger;
using Managers.GameManager;
using UI;

public class OfflineTimerCtrl : MonoBehaviour
{
    // private float minTime = 60;
    // private float maxTime = 10800;
    //
    // private float killCountPerSecond = 0.1f;
    //
    // private float timePassed;
    //
    // private UIManager uiManager;
    // private PushNotificationManager pushManager;
    // // private RewardManager rewardManager;
    //
    // private UI_OffLineReward ui_offLineReward;
    //
    // private DateTime startTime;
    // private string startTimestr;
    //
    // public void StartApplication()
    // {
    //     SetReferences();
    //     InitKey();
    // }
    //
    // private void SetReferences()
    // {
    //     uiManager = UIManager.Instance;
    //     pushManager = PushNotificationManager.Instance;
    //     // rewardManager = RewardManager.Instance;
    //
    //     ui_offLineReward = uiManager.UIOffLineReward;
    //     ui_offLineReward.Initialize();
    // }
    //
    // private void InitKey()
    // {
    //     if (!PlayerPrefs.HasKey("OfflineTimerStr" + Application.productName)) TimeReset();
    //     else RefilKey();
    // }
    //
    // public void RefilKey()
    // {
    //     TimeLoad();
    //
    //     TimeSpan ts = DateTime.Now - startTime;
    //     timePassed = (float)ts.TotalSeconds;
    //
    //     ManagePushRewards();
    //
    //     if (timePassed >= minTime)   //10 Minutes 600  //30 Minutes 1800  // 3시간 10800  //4시간 14400
    //     {
    //         OfflinePanelOpen();
    //         TimeReset();
    //         Debug.Log("OfflineTimer" + " + " + timePassed);
    //     }
    //     else
    //     {
    //         Debug.Log("OfflineTimer" + " + " + timePassed);
    //     }
    // }
    //
    // private void TimeLoad()
    // {
    //     startTimestr = PlayerPrefs.GetString("OfflineTimerStr" + Application.productName);
    //     startTime = DateTime.Parse(startTimestr);
    // }
    //
    // public void TimeReset()
    // {
    //     startTime = DateTime.Now;
    //     startTimestr = startTime.ToString();
    //     PlayerPrefs.SetString("OfflineTimerStr" + Application.productName, startTimestr);
    // }
    //
    // // 오프라인 타임이 특정시간 이상 지났다면 보상창 띄우기
    // private void OfflinePanelOpen()
    // {
    //     timePassed = Mathf.Min(timePassed, maxTime);
    //     int killCount = (int)((int)(timePassed) * killCountPerSecond);
    //
    //     ui_offLineReward.SetUI(killCount, (int)timePassed);
    //     // ui_offLineReward.OpenUI();
    //     ui_offLineReward.gameObject.SetActive(true);
    // }
    //
    // // Ok확인 보상받고 패널끄기 
    // public void OnClickBtn_Ok()
    // {
    //     TimeReset(); // 타이머 리셋
    //     gameObject.transform.GetChild(0).gameObject.SetActive(false);
    // }
    //
    // private void ManagePushRewards()
    // {
    //     List<PushNotesDataSO> pushDatas = pushManager.GetUnrecievedRewardDatas((int)timePassed / 3600);
    //     List<Reward> rewards = new List<Reward>();
    //
    //     foreach (PushNotesDataSO data in pushDatas)
    //     {
    //         pushManager.SetRewardRecieved(data.name);
    //         if (data.RewardType == Enums.RewardType.None) continue;
    //
    //         rewards.Add(new Reward(data.RewardType, data.Amount));
    //     }
    //
    //     // if (rewards.Count > 0) rewardManager.GiveReward(rewards, "특별 보상");
    // }

}