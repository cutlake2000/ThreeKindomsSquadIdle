using System;
using System.Collections.Generic;
using System.Linq;
using Resources.ScriptableObjects.Scripts;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

namespace Managers.GameManager
{
    public class PushManager : MonoBehaviour
    {
        public static PushManager Instance;

        private Dictionary<string, PushNotesDataSo> dataDic;
        private Dictionary<string, bool> rewardRecieved;

        private static int HasPermissionChecked
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
            SetCollections();

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

        private void SetCollections()
        {
            dataDic = new Dictionary<string, PushNotesDataSo>();
            rewardRecieved = new Dictionary<string, bool>();
        }

        private void LoadDatas()
        {
            var datas = UnityEngine.Resources.LoadAll<PushNotesDataSo>("ScriptableObjects/Data/PushMessage");

            foreach (var data in datas) dataDic[data.name] = data;

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
                // 권한이 허용되지 않은 경우 권한 요청
                Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause) return;
            
            // 알림 예약 제거
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.CancelAllScheduledNotifications();
        }


        public void SendLocalNotification()
        {
            // Android에서만 사용되는 푸시 채널 설정
            var channel = new AndroidNotificationChannel()
            {
                Id = "samI",
                Name = "samN",
                Importance = Importance.Default,
                Description = "samD"
            };

            AndroidNotificationCenter.RegisterNotificationChannel(channel);
            
            // AndroidNotificationCenter.SendNotification(new AndroidNotification("테스트 발송", "테스트 발송", DateTime.Now) ,"samI");
            // AndroidNotificationCenter.SendNotification(new AndroidNotification("테스트 발송10", "테스트 발송10", DateTime.Now.AddSeconds(10)) ,"samI");
            // AndroidNotificationCenter.SendNotification(new AndroidNotification("테스트 발송30", "테스트 발송30", DateTime.Now.AddSeconds(30)) ,"samI");
            // AndroidNotificationCenter.SendNotification(new AndroidNotification("테스트 발송60", "테스트 발송60", DateTime.Now.AddSeconds(60)) ,"samI");
            // AndroidNotificationCenter.SendNotification(new AndroidNotification("테스트 발송120", "테스트 발송120", DateTime.Now.AddMinutes(2)) ,"samI");
            
            Debug.Log("테스트 발송");
            
            foreach (var kvp in dataDic.Where(kvp => !rewardRecieved[kvp.Key]))
            {
                AndroidNotificationCenter.SendNotification(new AndroidNotification(kvp.Value.Title, kvp.Value.Desc, DateTime.Now.AddHours(kvp.Value.PushTime)), "samI");
            }
        }
#endif

        public List<PushNotesDataSo> GetUnrecievedRewardDatas(int hour)
        {
            var pushDatas = new List<PushNotesDataSo>();

            foreach (var kvp in dataDic)
                if (!rewardRecieved[kvp.Key] && kvp.Value.PushTime <= hour)
                    pushDatas.Add(kvp.Value);

            return pushDatas;
        }

        public void SetRewardRecieved(string dataName)
        {
            rewardRecieved[dataName] = true;
        }

        public void SaveRewardRecieved()
        {
            foreach (var kvp in rewardRecieved) ES3.Save($"PushRewardRecieved_{kvp.Key}", kvp.Value);
        }
    }
}