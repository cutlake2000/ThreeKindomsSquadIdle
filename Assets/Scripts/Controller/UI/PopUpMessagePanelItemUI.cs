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

        private float waitTime;
        private float fadeTime;
        private float height;
        
        private Enums.LockButtonType type;
        private readonly WaitForSeconds waitForSeconds = new(0.2f);
        
        public void SetMessage(string message)
        {
            alertText.text = message;
        }
        
        public void StartLockButtonCoroutine(Enums.LockButtonType currentType)
        {
            type = currentType;
            waitTime = 0.5f;
            fadeTime = 0.5f;
            height = 30.0f;
            
            gameObject.SetActive(false);
            StopCoroutine(WaitForFade());
            StopCoroutine(FadeAnim());
            
            ResetSlot();
            gameObject.SetActive(true);
            StartCoroutine(WaitForFade());
        }

        public void StartTotalCombatPowerCoroutine()
        {
            waitTime = 0.3f;
            fadeTime = 0.2f;
            height = 30.0f;
            
            gameObject.SetActive(false);
            StopCoroutine(WaitForFade());
            StopCoroutine(FadeAnim());
            
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
        
            while (currentTime < waitTime)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }
            
            StartCoroutine(FadeAnim());
        }

        private IEnumerator FadeAnim()
        {
            float currentTime = 0;
            
            while (currentTime < fadeTime)
            {
                var progress = currentTime / fadeTime;
                group.alpha = 1 - progress;
                var yPos = progress * height;
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