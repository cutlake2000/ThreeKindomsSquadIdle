using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI
{
    public class LockButtonUI : MonoBehaviour
    {
    //     [SerializeField] private Text alertText;
    //     [SerializeField] private CanvasGroup group;
    //     [SerializeField] private float waitTime;
    //     [SerializeField] private float fadeTime;
    //     [SerializeField] private float height;
    //
    //     private Coroutine waitCoroutine;
    //     private Coroutine fadeCoroutine;
    //
    //     private bool isFadeStarted;
    //
    //     // public event Action<PopUpMessageUI> OnAnimEnd;
    //
    //     public void SetMessage(string message)
    //     {
    //         alertText.text = message;
    //     }
    //
    //     public void OnEnable()
    //     {
    //         ResetSlot();
    //         waitCoroutine = StartCoroutine(WaitForFade());
    //     }
    //
    //     private void ResetSlot()
    //     {
    //         group.alpha = 1;
    //         transform.localPosition = Vector3.zero;
    //     }
    //
    //     public void QuickAnim()
    //     {
    //         if (isFadeStarted) return;
    //
    //         StopCoroutine(waitCoroutine);
    //         StartFade();
    //     }
    //
    //     IEnumerator WaitForFade()
    //     {
    //         float currentTime = 0;
    //
    //         while (currentTime < waitTime)
    //         {
    //             currentTime += Time.deltaTime;
    //             yield return null;
    //         }
    //
    //         StartFade();
    //     }
    //
    //     private void StartFade()
    //     {
    //         isFadeStarted = true;
    //
    //         fadeCoroutine = StartCoroutine(FadeAnim());
    //     }
    //
    //     IEnumerator FadeAnim()
    //     {
    //         float currentTime = 0;
    //
    //         while (currentTime < waitTime)
    //         {
    //             var progress = currentTime / waitTime;
    //             group.alpha = 1 - progress;
    //             var yPos = progress * height;
    //             transform.localPosition = new Vector3(0, yPos, 0);
    //             currentTime += Time.deltaTime;
    //             yield return null;
    //         }
    //
    //         AnimEnd();
    //     }
    //
    //     private void AnimEnd()
    //     {
    //         waitCoroutine = null;
    //         fadeCoroutine = null;
    //         isFadeStarted = false;
    //
    //         // OnAnimEnd?.Invoke(this);
    //     }
    }
}