using System;
using System.Collections;
using Controller.UI.BottomMenuUI.BottomMenuPanel;
using Controller.UI.BottomMenuUI.PopUpUI;
using Data;
using Managers.GameManager;
using Module;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI
{
    public class LockButtonUI : MonoBehaviour
    {
        [SerializeField] private Enums.LockButtonType lockButtonType;

        public void InitializeEventListener()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(UpdatePopUpMessageUI);
        }

        private void UpdatePopUpMessageUI()
        {
            UIManager.Instance.popUpMessagePanelUI.GetComponent<PopUpMessagePanelUI>().UpdateLockButtonPopUpMessagePanelUI(lockButtonType);
        }
    }
}