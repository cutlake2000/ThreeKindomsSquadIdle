using System.Collections;
using Module;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers.GameManager
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public Slider sliderUI;

        public GameObject touchToStartButton;
        public IdGenerator IdGenerator;

        [SerializeField] private string accountName; 

        private float dummyTimeRange_Min;
        private float dummyTimeRange_Max;

        // 최초 게이지가 증가되는 더미 시간 -> 이후 본 로딩 시작
        private float dummyTime;

        // 로딩 이후 시작 버튼 처리 
        private bool toPlayButton;

        private void Awake()
        {
            dummyTimeRange_Min = 0.8f;
            dummyTimeRange_Max = 1.5f;
            dummyTime = 0;

            // 버튼의 상태를 초기화 한다.
            toPlayButton = false;
        }

        private void Start()
        {
            // UI를 처리 한다.
            sliderUI.gameObject.SetActive(true);
            touchToStartButton.SetActive(false);

            IdGenerator = IdGenerator.GetComponent<IdGenerator>();

            // 로딩을 즉시 실행
            StartCoroutine(LoadAsynchronously(1));
        }

        private IEnumerator LoadAsynchronously(int sceneIndex)
        {
            // 더미 타이머로 진행할 값을 설정
            dummyTime = Random.Range(dummyTimeRange_Min, dummyTimeRange_Max);

            // 게이지로 표현되는 loading 값 변수들
            var loadingTime = 0.0f; // 시간 계산 용
            var progress = 0.0f; // 게이지 용


            // 타이머 게이지 처리 
            while (loadingTime <= dummyTime)
            {
                // 프레임 당 시간을 증가 
                loadingTime += Time.deltaTime;

                // AsyncOperation 를 통한 추가 로딩 처리를 위해 0.9 값 을 백분율화 
                // 이후 opertaion.progress 는 0.9 까치 처리 되고 완료 된다.
                progress = Mathf.Clamp01(loadingTime / (0.9f + dummyTime));

                // 슬라이더바의 값 증가 처리
                sliderUI.value = progress;

                yield return null;
            }

            // "AsyncOperation"라는 "비동기적인 연산을 위한 코루틴을 제공"
            var operation = SceneManager.LoadSceneAsync(sceneIndex);

            // 로딩 후 스타트 버튼 처리를 위한 선언
            // 이 항목이 없으면 바로 로딩 후 Scene 이동이 처리 됨
            operation.allowSceneActivation = false;

            // 로딩이 종료되기 전까지의 로딩창 게이지 처리
            while (!operation.isDone)
            {
                if (toPlayButton)
                {
                    operation.allowSceneActivation = true;
                }
                else
                {
                    // 비동기 로딩 진행에 따른 게이지 처리
                    progress = Mathf.Clamp01((operation.progress + loadingTime) / (0.9f + dummyTime));

                    // 슬라이더 증가 처리
                    sliderUI.value = progress;

                    yield return null;

                    // 로딩 이후의 처리 , 버튼을 누르면 씬 이동이 처리 된다.
                    // UI를 반전 처리 한다.
                    sliderUI.gameObject.SetActive(false);

                    if (ES3.KeyExists($"{nameof(accountName)}") == false)
                    {
                        ES3.Save("accountName", IdGenerator.GenerateRandomId());
                    }
                    
                    touchToStartButton.SetActive(true);
                }
                
                yield return null;
            }
        }

        // 버튼을 입력 한다.
        public void ToPlayButton()
        {
            toPlayButton = true;
        }
    }
}