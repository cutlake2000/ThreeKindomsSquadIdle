using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Managers.GameManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI
{
    public class PopUpMessagePanelItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text alertText;
        [SerializeField] private CanvasGroup group;

        private float _waitTime = 0.5f;
        private float _fadeTime = 0.5f;
        private float _height = 30.0f;
        
        private Enums.LockButtonType type;
        private readonly WaitForSeconds waitForSeconds = new(0.2f);
        
        public void SetMessage(string message)
        {
            alertText.text = message;
        }
        
        public void StartLockButtonCoroutine(Enums.LockButtonType currentType)
        {
            type = currentType;
            _waitTime = 0.5f;
            _fadeTime = 0.5f;
            _height = 30.0f;
            
            gameObject.SetActive(false);
            ResetSlot();
            gameObject.SetActive(true);
            StartCoroutine(WaitForFade());
        }

        public void StartTotalCombatPowerCoroutine()
        {
            gameObject.SetActive(false);
            ResetSlot();
            gameObject.SetActive(true);
            StartCoroutine(WaitForFade());
        }
        
        private void ResetSlot()
        {
            group.alpha = 1;
            transform.localPosition = Vector3.zero;
        }

        private IEnumerator WaitForFade()
        {
            float currentTime = 0;
        
            while (currentTime < _waitTime)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }
            
            StartCoroutine(FadeAnim());
        }

        private IEnumerator FadeAnim()
        {
            float currentTime = 0;
            
            while (currentTime < _fadeTime)
            {
                var progress = currentTime / _fadeTime;
                group.alpha = 1 - progress;
                var yPos = progress * _height;
                transform.localPosition = new Vector3(0, yPos, 0);
                currentTime += Time.deltaTime;
                yield return null;
            }

            yield return waitForSeconds;
        
            gameObject.SetActive(false);
            UIManager.Instance.popUpMessagePanelUI.activePopUpMessagePanelItems[(int)type] = false;
        }
    }
}