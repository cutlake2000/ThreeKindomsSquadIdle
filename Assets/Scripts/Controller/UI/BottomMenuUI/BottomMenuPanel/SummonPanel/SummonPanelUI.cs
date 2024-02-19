using System;
using System.Collections;
using System.Collections.Generic;
using Creature.Data;
using Managers.BattleManager;
using Managers.BottomMenuManager.SummonPanel;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SummonPanel
{
    public class SummonPanelUI : MonoBehaviour
    {
        [Header("소환 패널 구성 아이템 목록 UI")] public List<GameObject> summonPanelScrollViewItems = new();
        [Header("소환 결과창 UI")] public SummonResultPanelUI summonResultPanelUI;
        [Header("스크롤뷰")] public ScrollRect scrollBar;
        [Header("Lock Object")] public GameObject[] summonLockItems;
        
        public void InitializeEventListeners()
        {
            foreach (var summonPanelScrollViewItem in summonPanelScrollViewItems)
            {
                summonPanelScrollViewItem.GetComponent<SummonPanelItemUI>().InitializeEventListener();
            }
            
            summonResultPanelUI.InitializeEventListener();

            foreach (var t in summonLockItems)
            {
                t.GetComponent<LockButtonUI>().InitializeEventListener();
            }
        }
        
        public void SetScrollViewVerticalPosition(float position)
        {
            scrollBar.verticalNormalizedPosition = Mathf.Clamp01(position);
        }

        public void UpdateLockItemUI(int targetSubIndex)
        {
            summonPanelScrollViewItems[targetSubIndex].SetActive(true);
            summonLockItems[targetSubIndex].SetActive(false);
        }
    }
}