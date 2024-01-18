using System;
using Creature.SquadScripts.SquadClass;
using UnityEngine;
using Managers;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            AccountManager.Instance.InitCurrencyManager();
            SquadManager.Instance.InitSquadManager();
            SquadStatManager.Instance.InitSquadStatManager();
            EquipmentManager.Instance.InitEquipmentManager();
            SummonManager.Instance.InitSummonManager();
            // AchievementManager.instance.InitAchievementManager();
            StageManager.Instance.InitStageManager();
            StageManager.Instance.StartStageRunner();

            ES3.Save("Init_Game", true);
        }
    }
}