using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controller.UI
{
    public class BottomMenuUIController : MonoBehaviour
    {
        [Header("버튼과 패널")]
        [SerializeField] private Button[] openButtons;
        [SerializeField] private Button[] closeButtons;
        [SerializeField] private GameObject[] panels;

        private void Start()
        {
            // 각 버튼에 이벤트 리스너 할당
            for (var i = 0; i < openButtons.Length; i++)
            {
                var index = i; // 현재 인덱스 캡처
                openButtons[i].onClick.AddListener(() => OnClickOpenPanel(index));
            }
            
            for (var i = 0; i < closeButtons.Length; i++)
            {
                var index = i; // 현재 인덱스 캡처
                closeButtons[i].onClick.AddListener(() => OnClickClosePanel(index));
            }
        }

        // 버튼 클릭 시 호출되는 메서드
        private void OnClickOpenPanel(int index)
        {
            // 모든 패널을 순회하면서 상태 설정
            for (var i = 0; i < panels.Length; i++)
            {
                panels[i].SetActive(i == index);
            }
            
            for (var i = 0; i < closeButtons.Length; i++)
            {
                openButtons[i].gameObject.SetActive(i != index);
            }
            
            for (var i = 0; i < closeButtons.Length; i++)
            {
                closeButtons[i].gameObject.SetActive(i == index);
            }
        }
        
        private void OnClickClosePanel(int index)
        {
            closeButtons[index].gameObject.SetActive(false);
            panels[index].SetActive(false);
            openButtons[index].gameObject.SetActive(true);
        }
    }
}