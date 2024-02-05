using System;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

namespace Managers.GameManager
{
    public class PushNotificationManager : MonoBehaviour
    {
        public int HasPermissionChecked
        {
            get { return PlayerPrefs.GetInt("hasPermissionChecked_" + Application.productName, 0); }
            set { PlayerPrefs.SetInt("hasPermissionChecked_" + Application.productName, value); }
        }

#if UNITY_EDITOR || UNITY_ANDROID
        void Start()
        {
            // 최초 한 번만 권한 체크
            if (HasPermissionChecked == 0)
            {
                CheckNotificationPermission();
                HasPermissionChecked = 1;
            }

            // 게임시작 모든 알람 지우기
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.CancelAllScheduledNotifications();
        }

        void CheckNotificationPermission()
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
            }
            else
            {
                // 알림 예약 제거
                AndroidNotificationCenter.CancelAllNotifications();
                AndroidNotificationCenter.CancelAllScheduledNotifications();
            }
        }


        void SendLocalNotification()
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

            // 메시지 예약 시간 
            DateTime notificationTime1 = DateTime.Now.AddSeconds(5);
            AndroidNotificationCenter.SendNotification(
                new AndroidNotification("제목", "내용", notificationTime1), "channel_id");

            // 메시지 예약 시간 
            DateTime notificationTime2 = DateTime.Now.AddHours(48);
            AndroidNotificationCenter.SendNotification(
                new AndroidNotification("제목", "내용", notificationTime2), "channel_id");
        }
#endif

    }
}
