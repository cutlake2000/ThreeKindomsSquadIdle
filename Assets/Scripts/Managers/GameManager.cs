using System;
using UnityEngine;
using Managers;
using Managers.BottomMenuManager;
using Managers.BottomMenuManager.SquadPanel;
using Managers.BottomMenuManager.SummonPanel;
using Managers.BottomMenuManager.TalentPanel;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            SquadBattleManager.Instance.InitSquadManager();
            SquadStatManager.Instance.InitSquadStatManager();
            SquadTalentManager.Instance.InitSquadTalentManager();
            SquadConfigureManager.Instance.InitSquadConfigureManager();
            SquadSummonManager.Instance.InitSummonManager();
            
            AccountManager.Instance.InitAccountManager();
            EquipmentManager.Instance.InitEquipmentManager();
            // AchievementManager.instance.InitAchievementManager();
            StageManager.Instance.InitStageManager();
            DungeonManager.Instance.InitDungeonManager();
            UIManager.Instance.InitUIManager();
            
            StageManager.Instance.StartStageRunner();
            ES3.Save("Init_Game", true);
        }
    }
}