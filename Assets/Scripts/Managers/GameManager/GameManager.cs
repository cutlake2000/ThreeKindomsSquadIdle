using Managers.BottomMenuManager.InventoryPanel;
using Managers.BottomMenuManager.SquadPanel;
using Managers.BottomMenuManager.SummonPanel;
using Managers.BottomMenuManager.TalentPanel;
using UnityEngine;

namespace Managers.GameManager
{
    public class GameManager : MonoBehaviour
    {
        // [SerializeField] private OfflineTimerCtrl offlineTimerCtrl;
        private void Start()
        {
            // offlineTimerCtrl.StartApplication();
            
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

            StageManager.Instance.StartStageRunner();
            ES3.Save("Init_Game", true);
        }

        // private void OnApplicationPause(bool pause)
        // {
        //     if (pause) offlineTimerCtrl.TimeReset();
        //     else offlineTimerCtrl.RefilKey();
        // }
    }
}