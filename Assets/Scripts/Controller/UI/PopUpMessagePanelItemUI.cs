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
        
        private const float waitTime = 0.5f;
        private const float fadeTime = 0.5f;
        private const float height = 30.0f;
        
        private Enums.LockButtonType type;
        private readonly WaitForSeconds waitForSeconds = new(0.2f);
        
        public void SetMessage(string message)
        {
            alertText.text = message;
        }
        
        public void StartCoroutine(Enums.LockButtonType currentType)
        {
            type = currentType;
            ResetSlot();
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