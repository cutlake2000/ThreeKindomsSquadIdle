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
  
            SquadConfigureManager.Instance.InitSquadConfigureManager();

    
            
            SquadBattleManager.Instance.InitSquadManager();
            SquadStatManager.Instance.InitSquadStatManager();
            TalentManager.Instance.InitSquadTalentManager();
            
            InventoryManager.Instance.InitEquipmentManager();
            StageManager.Instance.InitStageManager();
            QuestManager.Instance.InitQuestManager();
            
            SummonManager.Instance.InitSummonManager();
            DungeonManager.Instance.InitDungeonManager();
            AccountManager.Instance.InitAccountManager();
            
            UIManager.Instance.InitUIManager();
            
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