using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module
{
    public class FadeInAndOut : MonoBehaviour
    {
        public float fadeSpeed = 1.5f;
        public bool fadeInOnStart = true;
        public bool fadeOutOnExit = true;

        private CanvasGroup canvasGroup;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void StartFadeInCoroutine()
        {
            StartCoroutine(FadeIn());
        }
        
        public void StartFadeOutCoroutine()
        {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeIn()
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * fadeSpeed;
                yield return null;
            }
        }

        private IEnumerator FadeOut()
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
                yield return null;
            }
        }
    }
}