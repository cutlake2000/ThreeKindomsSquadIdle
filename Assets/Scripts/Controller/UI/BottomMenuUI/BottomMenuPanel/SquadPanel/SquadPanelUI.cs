using System;
using Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadConfigurePanel;
using Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel.SquadStatPanel;
using Controller.UI.TopMenuUI.PlayerInfoPanelUI;
using Data;
using Managers.BottomMenuManager.SquadPanel;
using Managers.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SquadPanel
{
    public class SquadPanelUI : MonoBehaviour
    {
        [Header("--- 스쿼드 스탯 ---")] public SquadStatPanelUI squadStatPanelUI;
        [Header("--- 스쿼드 구성 ---")] public SquadConfigurePanelUI squadConfigurePanelUI;

        [Header("스쿼드 스탯 / 스쿼드 구성 / 스쿼드 등급 패널")]
        public GameObject[] squadPanel;

        [Header("스쿼드 패널 전환 버튼")]
        public Button[] squadPanelOnButton;
        public Button[] squadPanelOffButton;

        public void InitializeEventListeners()
        {
            squadStatPanelUI.InitializeEventListeners();
            squadConfigurePanelUI.InitializeEventListeners();

            for (var i = 0; i < squadPanel.Length; i++)
            {
                var index = i;
                squadPanelOffButton[i].GetComponent<Button>().onClick.AddListener(() => InitializeSquadPanelButton(index));
            }
        }

        private void InitializeSquadPanelButton(int index)
        {
            for (var i = 0; i < squadPanel.Length; i++)
                if (i == index)
                {
                    squadPanel[i].SetActive(true);
                    squadPanelOnButton[i].gameObject.SetActive(true);
                    squadPanelOffButton[i].gameObject.SetActive(false);
                }
                else
                {
                    squadPanel[i].SetActive(false);
                    squadPanelOnButton[i].gameObject.SetActive(false);
                    squadPanelOffButton[i].gameObject.SetActive(true);
                }
        }
    }
}