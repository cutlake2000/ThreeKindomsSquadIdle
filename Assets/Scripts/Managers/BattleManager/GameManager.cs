using Managers.BottomMenuManager.SquadPanel;
using Managers.BottomMenuManager.SummonPanel;
using Managers.BottomMenuManager.TalentPanel;
using UnityEngine;

namespace Managers.BattleManager
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            SquadStatManager.Instance.InitSquadStatManager();
            SquadConfigureManager.Instance.InitSquadConfigureManager();
            TalentManager.Instance.InitSquadTalentManager();
            SummonManager.Instance.InitSummonManager();
            DungeonManager.Instance.InitDungeonManager();
         
            SquadBattleManager.Instance.InitSquadManager();
            AccountManager.Instance.InitAccountManager();
            InventoryManager.Instance.InitEquipmentManager();
            
            StageManager.Instance.InitStageManager();
            QuestManager.Instance.InitQuestManager();
            
            UIManager.Instance.InitUIManager();

            StageManager.Instance.StartStageRunner();
            ES3.Save("Init_Game", true);
        }
    }
}