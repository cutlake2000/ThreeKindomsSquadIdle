using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Function
{
    public class NestedScroll : MonoBehaviour, IEndDragHandler
    {
        public Scrollbar scrollbar;

        [SerializeField] private GameObject[] summonCategories;
    
        private int categoriesIndex, curPos, targetPos;

        private void Start()
        {
            curPos = 0;
            categoriesIndex = summonCategories.Length;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            CalculateTargetPos();

            StartCoroutine(SmootheScrollView());
        }

        private void CalculateTargetPos()
        {
            switch (scrollbar.value)
            {
                case <= -0.2f:
                    targetPos = curPos - 1;
                    break;
                case >= 0.2f:
                    targetPos = curPos + 1;
                    break;
                default:
                    targetPos = curPos;
                    break;
            }

            if (targetPos < 0) targetPos = categoriesIndex - 1;
            else if (targetPos > categoriesIndex - 1) targetPos = 0;
        }

        private IEnumerator SmootheScrollView()
        {
            float elapsedTime = 0.0f;

            while (elapsedTime < 0.1f)
            {
                elapsedTime += Time.deltaTime;

                scrollbar.value = Mathf.Lerp(curPos, targetPos, elapsedTime / 0.1f);
                
                yield return null;
            }

            curPos = targetPos;
        
            yield return null;
        }
    }
}