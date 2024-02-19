using Data;
using Managers;
using Managers.BattleManager;
using Managers.BottomMenuManager.SquadPanel;
using Managers.GameManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI
{
    public class BottomMenuPanelUI : MonoBehaviour
    {
        [Header("버튼과 패널")]
        [SerializeField] private Button[] openButtons;
        [SerializeField] private Button[] closeButtons;
        [SerializeField] private Button[] lockButtons;
        [SerializeField] private GameObject[] panels;
        
        [Header("백보드")]
        [SerializeField] private Button backboardPanel;

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
            
            for (var i = 0; i < lockButtons.Length; i++)
            {
                var index = i; // 현재 인덱스 캡처
                lockButtons[i].GetComponent<LockButtonUI>().InitializeEventListener();
            }
            
            backboardPanel.onClick.AddListener(OnClickBackboardPanel);
        }

        private void OnClickBackboardPanel()
        {
            for (var i = 0; i < panels.Length; i++)
            {
                openButtons[i].gameObject.SetActive(true);
                closeButtons[i].gameObject.SetActive(false);
                panels[i].gameObject.SetActive(false);
            }
            
            backboardPanel.gameObject.SetActive(false);
        }

        // 버튼 클릭 시 호출되는 메서드
        private void OnClickOpenPanel(int index)
        {
            backboardPanel.gameObject.SetActive(true);
            
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
            backboardPanel.gameObject.SetActive(false);
            closeButtons[index].gameObject.SetActive(false);
            panels[index].SetActive(false);
            openButtons[index].gameObject.SetActive(true);
            
            if (StageManager.Instance.initStageResult) StageManager.Instance.initStageResult = true;
            if (StageManager.Instance.stageResultUI.activeInHierarchy) StageManager.Instance.stageResultUI.SetActive(false);
        }

        public void UpdateLockButtonUI(int index)
        {
            lockButtons[index].gameObject.SetActive(false);
            openButtons[index].gameObject.SetActive(true);
        }
    }
}