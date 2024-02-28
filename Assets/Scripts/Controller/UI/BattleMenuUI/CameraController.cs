using System.Collections.Generic;
using Data;
using Managers.BottomMenuManager.SquadPanel;
using Managers.GameManager;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI.BattleMenuUI
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Info")] [SerializeField]
        private Camera mainCamera;

        [Range(-10f, 10f)] [SerializeField] private float cameraXOffset;
        [Range(-10f, 10f)] [SerializeField] private float cameraYOffset;
        [Range(-10f, 10f)] [SerializeField] private float cameraZOffset;
        [SerializeField] private float cameraMoveSpeed;
        public Transform currentCameraTarget;

        [Header("Button")]
        [SerializeField] private Button warriorButton;
        [SerializeField] private Button archerButton;
        [SerializeField] private Button wizardButton;
        
        [Header("Base Camera Target")]
        [SerializeField] private Transform baseTarget;

        [Header("SelectEffects")]
        [SerializeField] private List<GameObject> selectEffects;

        private void LateUpdate()
        {
            MoveCameraToTarget();
        }

        public void InitCameraTarget()
        {
            SetButtonClickedListener();
            SetCameraTarget(0);
        }

        private void SetButtonClickedListener()
        {
            warriorButton.onClick.AddListener(() =>
            {
                SetCameraTarget(0);
                QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.WarriorCamera, 1);
            });
            archerButton.onClick.AddListener(() =>
            {
                SetCameraTarget(1);
                QuestManager.Instance.IncreaseQuestProgressAction.Invoke(Enums.QuestType.ArcherCamera, 1);
            });
            wizardButton.onClick.AddListener(() =>
            {
                SetCameraTarget(2);
            });
        }

        public void SetCameraTarget(int targetIndex)
        {
            var target = SquadConfigureManager.Instance.modelSpawnPoints[targetIndex].transform;
            
            if (currentCameraTarget == target || target.gameObject.activeInHierarchy == false) return;
            
            UpdateSelectEffect(targetIndex);
            
            currentCameraTarget = target.transform;
        }

        private void MoveCameraToTarget()
        {
            if (currentCameraTarget == null) return;

            var position = currentCameraTarget.transform.position;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,
                new Vector3(position.x, position.y, cameraZOffset), cameraMoveSpeed * Time.deltaTime);
        }

        public void InitializeCameraPosition()
        {
            currentCameraTarget = baseTarget;
        }

        private void UpdateSelectEffect(int index)
        {
            for (var i = 0; i < selectEffects.Count; i++)
            {
                selectEffects[i].SetActive(i == index);
            }
        }
    }
}