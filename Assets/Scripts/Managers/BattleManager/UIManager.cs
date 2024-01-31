using Controller.UI;
using Controller.UI.BottomMenuUI;
using Controller.UI.BottomMenuUI.BottomMenuPanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.TalentPanel;
using Controller.UI.TopMenuUI.QuestPanel;
using UnityEngine;

namespace Managers.BattleManager
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("=== 스킬 쿨타임 ===")] public SquadSkillCoolTimerUI squadSkillCoolTimerUI;

        [Space(5)] [Header("=== 바텀 메뉴 패널 ===")] public BottomMenuPanelUI bottomMenuPanelUI;
        
        [Space(5)] [Header("=== 스쿼드 패널 ===")] public SquadPanelUI squadPanelUI;

        [Space(5)] [Header("=== 재능 패널 ===")] public TalentPanelUI talentPanelUI;
        
        [Space(5)] [Header("=== 퀘스트 패널 ===")] public QuestPanelUI questPanelUI;

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
            bottomMenuPanelUI.InitializeEventListeners();
            squadSkillCoolTimerUI.InitializeEventListeners();
            squadPanelUI.InitializeEventListeners();
            talentPanelUI.InitializeEventListeners();
            questPanelUI.InitializeEventListeners();
        }
    }
}