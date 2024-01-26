using System;
using Controller.UI;
using Controller.UI.BottomMenuUI.SquadMenu;
using Creature.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [Header("=== 스킬 쿨타임 ===")] public SquadSkillCoolTimerUI squadSkillCoolTimerUI;
        [Header("=== 스쿼드 패널 ===")] public SquadPanelUI squadPanelUI;
        
        private void Awake()
        {
            Instance = this;
        }

        public void InitUIManager()
        {
            InitializeEventListeners();
        }
        
        private void InitializeEventListeners()
        {
            squadSkillCoolTimerUI.InitializeEventListeners();
            squadPanelUI.InitializeEventListeners();
        }
    }
}