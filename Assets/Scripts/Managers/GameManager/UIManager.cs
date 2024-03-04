using Controller.UI;
using Controller.UI.BottomMenuUI;
using Controller.UI.BottomMenuUI.BottomMenuPanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.DungeonPanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.InventoryPanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.SummonPanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.TalentPanel;
using Controller.UI.BottomMenuUI.PopUpUI;
using Controller.UI.TopMenuUI.PlayerInfoPanelUI;
using Controller.UI.TopMenuUI.QuestPanel;
using Controller.UI.TopMenuUI.SkillPanel;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers.GameManager
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [Space(5)] [Header("=== 스테이지 결과창 UI ===")] public StageRewardPanelUI stageRewardPanelUI;
        [Space(5)] [Header("=== 팝업 메시지 패널 ===")] public PopUpMessagePanelUI popUpMessagePanelUI;
        [Space(5)] [Header("=== 스쿼드 아이콘 패널 ===")] public PlayerInfoPanelUI playerInfoPanelUI;
        [Space(5)] [Header("=== 스킬 쿨타임 ===")] public SquadSkillCoolTimerUI squadSkillCoolTimerUI;
        [Space(5)] [Header("=== 바텀 메뉴 패널 ===")] public BottomMenuPanelUI bottomMenuPanelUI;
        [Space(5)] [Header("=== 스쿼드 패널 ===")] public SquadPanelUI squadPanelUI;
        [Space(5)] [Header("=== 인벤토리 패널 ===")] public InventoryPanelUI inventoryPanelUI;
        [Space(5)] [Header("=== 재능 패널 ===")] public TalentPanelUI talentPanelUI;
        [Space(5)] [Header("=== 던전 패널 ===")] public DungeonPanelUI dungeonPanelUI;
        [Space(5)] [Header("=== 퀘스트 패널 ===")] public QuestPanelUI questPanelUI;
        [Space(5)] [Header("=== 소환 패널 ===")] public SummonPanelUI summonPanelUI;
        [Space(5)] [Header("=== 오프라인 보상 패널 ===")] public OffLineRewardUI offLineRewardUI;
        [Space(5)] [Header("=== 푸쉬 보상 패널 ===")] public PushRewardUI pushRewardUI;

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
            stageRewardPanelUI.InitializeEventListener();
            dungeonPanelUI.InitializeEventListener();
            popUpMessagePanelUI.InitializeEventListener();
            bottomMenuPanelUI.InitializeEventListeners();
            squadSkillCoolTimerUI.InitializeEventListeners();
            squadPanelUI.InitializeEventListeners();
            inventoryPanelUI.InitializeEventListener();
            talentPanelUI.InitializeEventListeners();
            questPanelUI.InitializeEventListeners();
            summonPanelUI.InitializeEventListeners();
            offLineRewardUI.InitializeEventListener();
            pushRewardUI.InitializeEventListener();
        }
        
        public static string FormatCurrency(double value)
        {
            // 정수 값을 100으로 나누어 소수점 이하로 변환합니다.
            var doubleValue = value / 100.0;

            // 변환된 값을 "F2" 형식 지정자를 사용하여 소수점 이하 두 자리까지 표시합니다.
            var formattedValue = doubleValue.ToString("F2");

            return formattedValue;
        }
    }
}