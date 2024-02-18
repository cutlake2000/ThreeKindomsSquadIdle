using Managers.BottomMenuManager.InventoryPanel;
using Managers.BottomMenuManager.SquadPanel;
using Managers.BottomMenuManager.SummonPanel;
using Managers.BottomMenuManager.TalentPanel;
using PushOfflineReward.Scripts.OfflineReward;
using UnityEngine;

namespace Managers.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private OfflineRewardController offlineRewardController;
        
        private void Start()
        {
            SquadStatManager.Instance.InitSquadStatManager();
            SquadConfigureManager.Instance.InitSquadConfigureManager();
            TalentManager.Instance.InitSquadTalentManager();
            SummonManager.Instance.InitSummonManager();
            DungeonManager.Instance.InitDungeonManager();
            AccountManager.Instance.InitAccountManager();
            InventoryManager.Instance.InitEquipmentManager();
            
            SquadBattleManager.Instance.InitSquadManager();
            StageManager.Instance.InitStageManager();
            QuestManager.Instance.InitQuestManager();
            
            UIManager.Instance.InitUIManager();
            
            PushManager.Instance.InitializePushManager();
            offlineRewardController.InitKey();

            StageManager.Instance.StartStageRunner();
            ES3.Save("Init_Game", true);
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause) offlineRewardController.TimeReset();
            else offlineRewardController.ResetKey();
        }
    }
}