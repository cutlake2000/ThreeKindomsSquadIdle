using System;
using Controller.UI;
using Creature.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [Header("=== 스킬 쿨타임 관련===")] public SquadSkillCoolTimerUI squadSkillCoolTimerUI;
        
        private void Awake()
        {
            Instance = this;
        }

        public void InitUIManager()
        {
            InitializeEventListener();
        }
        
        private void InitializeEventListener()
        {
            squadSkillCoolTimerUI.InitializeEventListener();
        }
    }
}