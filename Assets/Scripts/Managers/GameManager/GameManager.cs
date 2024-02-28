using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Managers.BottomMenuManager.InventoryPanel;
using Managers.BottomMenuManager.SquadPanel;
using Managers.BottomMenuManager.SummonPanel;
using Managers.BottomMenuManager.TalentPanel;
using PushOfflineReward.Scripts.OfflineReward;
using UnityEngine;
using UnityEngine.Android;

namespace Managers.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private OfflineRewardController offlineRewardController;
        
        private string[] testDeviceIds = { "f70fe41fb0676ca6a5f502abde7de006",
            "cOfe1516826c70f45a169f38a3ab2fcd",
            "c71d0c9e4ba81bf162d5e9c88c1aba92",
            "109fcd783d2c3e3fa6febf10acb3f4b3",
            "d57f06fe7ee09848dde7ea36f0eb97be",
            "c53e390c198ea335d5434c076b104df0",
            "654b99b1de9dee6125719a283b24d614"};
        
        private void Start()
        {
            SquadConfigureManager.Instance.InitSquadConfigureManager();
            SquadBattleManager.Instance.InitSquadBattleManager();
            SquadStatManager.Instance.InitSquadStatManager();
            TalentManager.Instance.InitSquadTalentManager();
            
            InventoryManager.Instance.InitEquipmentManager();
            StageManager.Instance.InitStageManager();
            QuestManager.Instance.InitQuestManager();
            
            SummonManager.Instance.InitSummonManager();
            DungeonManager.Instance.InitDungeonManager();
            AccountManager.Instance.InitAccountManager();
            MonsterManager.Instance.InitMonsterManager();
            
            UIManager.Instance.InitUIManager();
            
            offlineRewardController.InitKey();
            
            StageManager.Instance.StartStageRunner();
            ES3.Save("Init_Game", true);
            
            // 기기의 현재 고유 ID 가져오기
            string currentDeviceId = SystemInfo.deviceUniqueIdentifier;

            // 테스트 기기인지 체크
            if (IsExcludedDevice(currentDeviceId))
            {
                // Firebase 초기화 이벤트 로그 호출 막기
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                    FirebaseApp app = FirebaseApp.DefaultInstance;
                    // Firebase Analytics를 사용하지 않도록 설정
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(false);
                });

                Debug.Log("테스트 기기입니다.");
            }
        }

        private void OnApplicationPause(bool pause)
        {
            var initGame = ES3.Load("Init_Game", false);
                
            if (initGame == false) return;
            
            if (pause)
            {
                offlineRewardController.TimeReset();
                
                // 알림 예약
                if (Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
                {
                    PushManager.Instance.SendLocalNotification();
                }
                
                PushManager.Instance.SaveRewardRecieved();
            }
            else
            {
                offlineRewardController.ResetKey();
                Debug.Log("ResetKey");
            }
        }
        
        // 특정 고유 ID를 가진 기기인지 확인
        bool IsExcludedDevice(string currentDeviceId)
        {
            return System.Array.Exists(testDeviceIds, id => id.Equals(currentDeviceId));
        }
    }
}