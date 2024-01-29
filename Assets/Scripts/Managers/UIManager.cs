using Controller.UI;
using Controller.UI.BottomMenuUI.BottomMenuPanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.TalentPanel;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [Header("=== 스킬 쿨타임 ===")] public SquadSkillCoolTimerUI squadSkillCoolTimerUI;
        [Space(5)]
        [Header("=== 스쿼드 패널 ===")] public SquadPanelUI squadPanelUI;
        [Space(5)]
        [Header("=== 재능 패널 ===")] public TalentPanelUI talentPanelUI;
        
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
            talentPanelUI.InitializeEventListeners();
        }
    }
}