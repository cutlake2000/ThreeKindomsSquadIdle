using System;
using UnityEngine;
using Managers;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            AccountManager.Instance.InitAccountManager();
            SquadManager.Instance.InitSquadManager();
            SquadStatManager.Instance.InitSquadStatManager();
            EquipmentManager.Instance.InitEquipmentManager();
            SummonManager.Instance.InitSummonManager();
            // AchievementManager.instance.InitAchievementManager();
            StageManager.Instance.InitStageManager();
            DungeonManager.Instance.InitDungeonManager();
            UIManager.Instance.InitUIManager();
            
            StageManager.Instance.StartStageRunner();
            ES3.Save("Init_Game", true);
        }
    }
}