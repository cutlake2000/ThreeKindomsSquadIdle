using System;
using System.Linq;
using Demo_Project;
using Managers.GameManager;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.LoginUI
{
    public class AccountGenerationPanelUI : MonoBehaviour
    {
        public Button summitButton;
        public GameObject accountGenerationPanel;
        public GameObject touchToStartButton;
        public InputField nameInputField;
        private const int minNameLength = 3;
        private const int maxNameLength = 12;

        public void Start()
        {
            summitButton.onClick.AddListener(OnNameSubmit);
        }

        public void OnNameSubmit()
        {
            string playerName = nameInputField.text.Trim();

            // 빈 문자열인지 확인
            if (string.IsNullOrEmpty(playerName))
            {
                Debug.LogError("닉네임을 입력해주세요.");
                return;
            }

            // 길이 제한 확인
            if (playerName.Length is < minNameLength or > maxNameLength)
            {
                Debug.LogError($"닉네임은 {minNameLength}자 이상, {maxNameLength}자 이하로 설정해주세요.");
                return;
            }

            // 특수 문자나 공백이 포함되어 있는지 확인 (선택적)
            if (playerName.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                Debug.LogError("닉네임에는 특수 문자나 공백을 포함할 수 없습니다.");
                return;
            }

            // TODO: 중복 닉네임 확인 로직 추가 (서버와의 통신이 필요할 수 있음)

            // 입력된 이름을 저장
            ES3.Save("accountName", playerName);
            
            accountGenerationPanel.SetActive(false);
            touchToStartButton.SetActive(true);
        }
    }
}