using Data;
using Managers;
using Managers.BattleManager;
using Managers.BottomMenuManager.SquadPanel;
using Managers.GameManager;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI
{
    public class BottomMenuPanelUI : MonoBehaviour
    {
        [Header("버튼과 패널")]
        [SerializeField] private Button[] openButtons;
        [SerializeField] private Button[] closeButtons;
        [SerializeField] private GameObject[] panels;

        public void InitializeEventListeners()
        {
            // 각 버튼에 이벤트 리스너 할당
            for (var i = 0; i < openButtons.Length; i++)
            {
                var index = i; // 현재 인덱스 캡처
                if (i == 3) continue; // TODO : 유물 버튼 락
                openButtons[i].onClick.AddListener(() => OnClickOpenPanel(index));
            }

            for (var i = 0; i < closeButtons.Length; i++)
            {
                var index = i; // 현재 인덱스 캡처
                if (i == 3) continue; // TODO : 유물 버튼 락
                closeButtons[i].onClick.AddListener(() => OnClickClosePanel(index));
            }
        }

        // 버튼 클릭 시 호출되는 메서드
        private void OnClickOpenPanel(int index)
        {
            // 모든 패널을 순회하면서 상태 설정
            for (var i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(i == index);

                switch (i)
                {
                    case 0:
                        UIManager.Instance.squadPanelUI.squadStatPanelUI.InitializeLevelUpMagnificationButton(0);
                        break;
                    case 2:
                        UIManager.Instance.talentPanelUI.InitializeLevelUpMagnificationButton(0);
                        break;
                }
            }

            for (var i = 0; i < closeButtons.Length; i++) openButtons[i].gameObject.SetActive(i != index);

            for (var i = 0; i < closeButtons.Length; i++) closeButtons[i].gameObject.SetActive(i == index);
        }

        private void OnClickClosePanel(int index)
        {
            closeButtons[index].gameObject.SetActive(false);
            panels[index].SetActive(false);
            openButtons[index].gameObject.SetActive(true);
            
            if (StageManager.Instance.initStageResult) StageManager.Instance.initStageResult = false;
            if (StageManager.Instance.stageResultUI.activeInHierarchy) StageManager.Instance.stageResultUI.SetActive(false);
            
            if (!SquadConfigureManager.Instance.isSquadConfigureChanged) return;
            
            StageManager.Instance.StopStageRunner();
            SquadConfigureManager.Instance.UpdateSquadConfigureModelOnBattle(SquadConfigureManager.Instance.FindEquippedCharacter(Enums.CharacterType.Warrior));
            SquadConfigureManager.Instance.UpdateSquadConfigureModelOnBattle(SquadConfigureManager.Instance.FindEquippedCharacter(Enums.CharacterType.Archer));
            SquadConfigureManager.Instance.UpdateSquadConfigureModelOnBattle(SquadConfigureManager.Instance.FindEquippedCharacter(Enums.CharacterType.Wizard));
            StageManager.Instance.initStageResult = true;
            StageManager.Instance.goToNextSubStage = true;
                
            StageManager.Instance.StartStageRunner();
        }
    }
}