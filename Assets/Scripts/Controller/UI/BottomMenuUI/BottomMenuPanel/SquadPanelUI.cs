using Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadConfigurePanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadStatPanel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel
{
    public class SquadPanelUI : MonoBehaviour
    {
        [Header("--- 스쿼드 스탯 ---")]
        public SquadStatPanelUI squadStatPanelUI;
        
        [Header("--- 스쿼드 구성 ---")]
        public SquadConfigurePanelUI squadConfigurePanelUI;
        
        [Header("스쿼드 스탯 / 스쿼드 구성 / 스쿼드 등급 패널")]
        public GameObject[] squadPanel;
        
        [Header("스쿼드 패널 전환 버튼")]
        public Button[] squadPanelButton;
        
        [Header("스쿼드 패널 전환 버튼 On / Off 스프라이트")]
        public Sprite[] squadPanelButtonSprites;

        public void InitializeEventListeners()
        {
            squadStatPanelUI.InitializeEventListeners();
            squadConfigurePanelUI.InitializeEventListeners();
            
            for (var i = 0; i < squadPanelButton.Length; i++)
            {
                var index = i;
                squadPanelButton[i].GetComponent<Button>().onClick.AddListener(() => InitializeSquadPanelButton(index));
            }
        }
        
        private void InitializeSquadPanelButton(int index)
        {
            for (var i = 0; i < squadPanelButton.Length; i++)
            {
                if (i == index)
                {
                    squadPanel[i].SetActive(true);
                    squadPanelButton[i].image.sprite = squadPanelButtonSprites[0];
                    squadPanelButton[i].GetComponentInChildren<TMP_Text>().color = Color.black;
                }
                else
                {
                    squadPanel[i].SetActive(false);
                    squadPanelButton[i].image.sprite = squadPanelButtonSprites[1];
                    squadPanelButton[i].GetComponentInChildren<TMP_Text>().color = Color.white;
                }
            }
        }
    }
}