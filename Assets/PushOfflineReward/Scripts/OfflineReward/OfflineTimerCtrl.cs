using System;
using Function;
using UnityEngine;
using UnityEngine.UI;

namespace PushOfflineReward.Scripts.OfflineReward
{
    public class OfflineTimerCtrl : MonoBehaviour {
        public float MaxTime;
        public Text RewardText_0;
        [SerializeField]bool panelbool;

        DateTime startTime;
        string startTimestr;

        void Start()
        {
            if (!PlayerPrefs.HasKey("OfflineTimerStr" + Application.productName)) TimeReset();
            else RefilKey();
        }

        public void RefilKey()
        {
            TimeLoad();
            TimeSpan ts = DateTime.Now - startTime;
            if (ts.TotalSeconds >= MaxTime)   //10 Minutes 600  //30 Minutes 1800  //4시간 14400
            {
                TimeReset();
                OfflinePanelOpen();
                panelbool = true; // 작동
                Debug.Log("OfflineTimer" + " + " + ts.TotalSeconds);
            }
            else
            {
                panelbool = false; // 미작동
                Debug.Log("OfflineTimer" + " + " + ts.TotalSeconds);
            }
        }

        void TimeLoad()
        {
            startTimestr = PlayerPrefs.GetString("OfflineTimerStr" + Application.productName);
            startTime = DateTime.Parse(startTimestr);
        }

        void TimeReset()
        {
            startTime = DateTime.Now;
            startTimestr = startTime.ToString();
            PlayerPrefs.SetString("OfflineTimerStr" + Application.productName, startTimestr);
        }

        // 오프라인 타임이 특정시간 이상 지났다면 보상창 띄우기
        void OfflinePanelOpen()
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            OfflineRewardCall();
        }

        // 보상받는 로직처리
        void OfflineRewardCall()
        {
            BigInteger currentBig = 100;
            currentBig = currentBig * 600;
            // RewardText_0.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(currentBig.ToString());
        }

        // Ok확인 보상받고 패널끄기 
        public void OnClickBtn_Ok()
        {
            TimeReset(); // 타이머 리셋
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}