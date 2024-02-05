using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Module
{
    public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Range(0f, 3f)] [SerializeField] private float holdDuration = 0.5f;
        public UnityEvent onHold;
        public bool pauseUpgrade;
        private Button button;
        private WaitForSeconds delay;

        private bool isHoldPressed;
        private DateTime pressTime;

        public void Awake()
        {
            delay = new WaitForSeconds(0.0001f);
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

        private IEnumerator Timer()
        {
            while (isHoldPressed)
            {
                var elapsedSeconds = (DateTime.Now - pressTime).TotalSeconds;

                pauseUpgrade = false;

                if (elapsedSeconds >= holdDuration) onHold?.Invoke();

                yield return delay;
            }
        }
    }
}