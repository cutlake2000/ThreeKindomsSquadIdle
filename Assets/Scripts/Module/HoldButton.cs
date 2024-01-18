using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Function
{
    public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Range(0.3f, 5f)] public float holdDuration = 2f;
        public UnityEvent onHold;

        private bool isHoldPressed = false;
        public bool pauseUpgrade = false;
        private DateTime pressTime;
        private Button button;
        private WaitForSeconds delay;

        public void Awake()
        {
            delay = new WaitForSeconds(0.3f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isHoldPressed = true;
            pauseUpgrade = true;
            pressTime = DateTime.Now;
        
            StartCoroutine(Timer());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isHoldPressed = false;
        }
    
        private IEnumerator Timer() {
            while (isHoldPressed)
            {
                double elapsedSeconds = (DateTime.Now - pressTime).TotalSeconds;
            
                pauseUpgrade = false;

                if (elapsedSeconds >= holdDuration)
                {
                    onHold?.Invoke();
                }

                yield return delay;
            }
        }
    }
}