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

namespace Controller.UI.BottomMenuUI.BottomMenuPanel.SummonPanel
{
    public class SummonPanelUI : MonoBehaviour
    {
        private readonly Vector3 initScale = new(1, 1, 1);
        
        
        [Header("소환 패널 구성 아이템 목록 UI")] public List<GameObject> summonPanelScrollViewItems = new();
        [Header("소환 결과창 UI")] public SummonResultPanelUI summonResultPanelUI;
        
        public void InitializeEventListeners()
        {
            foreach (var summonPanelScrollViewItem in summonPanelScrollViewItems)
            {
                summonPanelScrollViewItem.GetComponent<SummonPanelItemUI>().InitializeEventListener();
            }
            
            summonResultPanelUI.InitializeEventListener();
        }


        // private IEnumerator SetSummonedEquipmentOnPanel()
        // {
        //     for (var i = summonLists.Count - 1; i >= 0; i--)
        //     {
        //         // summonLists[i].gameObject.SetActive(true);
        //         // summonLists[i].SetSummonUI();
        //         yield return summonWaitForSeconds;
        //     }
        // }
    }
}