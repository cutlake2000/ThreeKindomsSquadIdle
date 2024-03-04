using System;
using System.Collections;
using Module;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Managers.GameManager
{
    public class LoadingSceneManager : MonoBehaviour
    {
        [SerializeField] private Slider sliderUI;
        [SerializeField] private Button touchToStartButton;

        private IdGenerator idGenerator;
        private AsyncOperation asyncOperation;
        private bool loadComplete;
        private WaitForSeconds delayTime;
        
        public string accountName;

        private void Awake()
        {
            loadComplete = false;
            sliderUI.gameObject.SetActive(true);
            touchToStartButton.gameObject.SetActive(false);
            
            idGenerator = GetComponent<IdGenerator>();

            delayTime = new WaitForSeconds(1.0f);
            
            touchToStartButton.onClick.AddListener(ToPlayButton);
        }

        private void Start()
        {
            StartCoroutine(LoadNextSceneAsync());
        }

        private IEnumerator LoadNextSceneAsync()
        {
            asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                var progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9까지의 진행률 조정
                sliderUI.value = progress;

                if (asyncOperation.progress >= 0.9f && !loadComplete) // 로딩이 거의 완료되었고, 로딩 작업이 아직 완전히 끝나지 않았다면
                {
                    sliderUI.value = progress;

                    yield return delayTime;
                    
                    loadComplete = true; // 로딩 작업 완료 플래그 설정
                    
                    sliderUI.gameObject.SetActive(false);
                    touchToStartButton.gameObject.SetActive(true);
                }

                yield return null;
            }
        }

        public void ToPlayButton()
        {
            if (!loadComplete) return; // 로딩 작업이 완료되었을 때만 다음 씬으로 전환
            
            accountName = idGenerator.GenerateRandomId();
            touchToStartButton.gameObject.SetActive(false);
            ES3.Save($"{nameof(accountName)}", accountName);
            
            Debug.Log($"{accountName}");
            asyncOperation.allowSceneActivation = true; // 씬 전환 허용
        }
    }
}