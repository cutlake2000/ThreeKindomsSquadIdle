using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.SquadPanel.SquadConfigurePanel
{
    public class SquadConfigurePanelUI : MonoBehaviour
    {
        [Header("워리어 / 아처 / 위자드 스크롤뷰")]
        public GameObject[] squadScrollViewPanel;
        
        [Header("워리어 / 아처 / 위자드 스크롤뷰 전환 버튼")]
        public Button[] squadScrollViewPanelButtons;
        public Image[] squadScrollViewPanelButtonIcons;
        public TMP_Text[] squadScrollViewPanelButtonTexts;
        
        [Header("스크롤뷰 전환 버튼 On / Off 스프라이트")]
        public Color[] squadScrollViewPanelButtonsColors;
        public void InitializeEventListeners()
        {
            for (var i = 0; i < squadScrollViewPanelButtons.Length; i++)
            {
                var index = i;
                squadScrollViewPanelButtons[i].GetComponent<Button>().onClick.AddListener(() => InitializeSquadPanelButton(index));
            }
        }
        
        private void InitializeSquadPanelButton(int index)
        {
            for (var i = 0; i < squadScrollViewPanel.Length; i++)
            {
                if (i == index)
                {
                    squadScrollViewPanel[i].SetActive(true);
                    squadScrollViewPanelButtons[i].image.color = squadScrollViewPanelButtonsColors[0];
                    squadScrollViewPanelButtonIcons[i].color = Color.black;
                    squadScrollViewPanelButtonTexts[i].color = Color.black;
                }
                else
                {
                    squadScrollViewPanel[i].SetActive(false);
                    squadScrollViewPanelButtons[i].image.color = squadScrollViewPanelButtonsColors[1];
                    squadScrollViewPanelButtonIcons[i].color = Color.white;
                    squadScrollViewPanelButtonTexts[i].color = Color.white;
                }
            }
        }
    }
}