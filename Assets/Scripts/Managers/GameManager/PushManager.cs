using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

namespace Managers.GameManager
{
    public class PushManager : MonoBehaviour
    {
        public static PushManager Instance;

        private Dictionary<string, PushNotesDataSO> dataDic;
        private Dictionary<string, bool> rewardRecieved;

        public static int HasPermissionChecked
        {
            get => PlayerPrefs.GetInt("hasPermissionChecked_" + Application.productName, 0);
            set => PlayerPrefs.SetInt("hasPermissionChecked_" + Application.productName, value);
        }

#if UNITY_EDITOR || UNITY_ANDROID

        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            dataDic = new Dictionary<string, PushNotesDataSO>();
            rewardRecieved = new Dictionary<string, bool>();

            // 최초 한 번만 권한 체크
            if (HasPermissionChecked == 0)
            {
                CheckNotificationPermission();
                HasPermissionChecked = 1;
            }

            // 게임시작 모든 알람 지우기
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.CancelAllScheduledNotifications();

            LoadDatas();
        }

        private void LoadDatas()
        {
            var datas = Resources.LoadAll<PushNotesDataSO>("ScriptableObjects/Datas");

            foreach(var data in datas)
            {
                dataDic[data.name] = data;
            }

            LoadRewardRecieved();
        }

        private void LoadRewardRecieved()
        {
            foreach (var kvp in dataDic)
            {
                rewardRecieved[kvp.Key] = ES3.KeyExists($"PushRewardRecieved_{kvp.Key}") ? ES3.Load<bool>($"PushRewardRecieved_{kvp.Key}") : false;
            }
        }

        private void CheckNotificationPermission()
        {
            // 푸시 알림 권한이 허용되어 있는지 확인
            if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
            {
                // 권한이 허용되지 않은 경우 권한 요청
                Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                // 알림 예약
                if (Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
                {
                    SendLocalNotification();
                }

                SaveRewardRecieved();
            }
            else
            {
                // 알림 예약 제거
                AndroidNotificationCenter.CancelAllNotifications();
                AndroidNotificationCenter.CancelAllScheduledNotifications();
            }
        }


        private void SendLocalNotification()
        {
            // Android에서만 사용되는 푸시 채널 설정
            var channel = new AndroidNotificationChannel()
            {
                Id = "channel_id",
                Name = "Channel",
                Importance = Importance.Default,
                Description = "Description",
            };

            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            foreach (var kvp in dataDic.Where(kvp => !rewardRecieved[kvp.Key]))
            {
                Debug.Log($"Push: {kvp.Key}");
                AndroidNotificationCenter.SendNotification(new AndroidNotification(kvp.Value.Title, kvp.Value.Desc, DateTime.Now.AddMinutes(kvp.Value.PushTime)), "channel_id");
            }
        }
#endif

        public List<PushNotesDataSO> GetUnrecievedRewardDatas(int hour)
        {
            var pushDatas = new List<PushNotesDataSO>();

            foreach(var kvp in dataDic)
            {
                if (!rewardRecieved[kvp.Key] && kvp.Value.PushTime <= hour)
                {
                    pushDatas.Add(kvp.Value);
                }
            }

            return pushDatas;
        }

        public void SetRewardRecieved(string dataName)
        {
            rewardRecieved[dataName] = true;
        }

        private void SaveRewardRecieved()
        {
            foreach (var kvp in rewardRecieved)
            {
                ES3.Save($"PushRewardRecieved_{kvp.Key}", kvp.Value);
            }
        }
    }
}
