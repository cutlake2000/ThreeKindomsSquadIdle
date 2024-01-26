using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI
{
    public class CameraController : MonoBehaviour
    {
        public Action<Transform> CameraTarget;
        
        [Header("Camera Info")]
        [SerializeField] private Camera mainCamera;
        [Range(-10f, 10f)] [SerializeField] private float cameraXOffset;
        [Range(-10f, 10f)] [SerializeField] private float cameraYOffset;
        [Range(-10f, 10f)] [SerializeField] private float cameraZOffset;
        [SerializeField] private float cameraMoveSpeed;
        public Transform currentCameraTarget;
        
        [Header("Button")]
        [SerializeField] private Button warriorButton;
        [SerializeField] private Button archerButton;
        [SerializeField] private Button wizardButton;

        public void InitCameraTarget(Component position)
        {
            SetButtonClickedListener();
            SetCameraTarget(position);
        }
        
        private void LateUpdate()
        {
            MoveCameraToTarget();
        }
        
        private void SetButtonClickedListener()
        {
            warriorButton.onClick.AddListener(() => SetCameraTarget(SquadBattleManager.Instance.squads[0].transform));
            archerButton.onClick.AddListener(() => SetCameraTarget(SquadBattleManager.Instance.squads[1].transform));
            wizardButton.onClick.AddListener(() => SetCameraTarget(SquadBattleManager.Instance.squads[2].transform));
        }

        private void SetCameraTarget(Component target)
        {
            if (currentCameraTarget == target || target.gameObject.activeInHierarchy == false) return;
            
            currentCameraTarget = target.transform;
        }

        private void MoveCameraToTarget()
        {
            if (currentCameraTarget == null) return;
            
            var position = currentCameraTarget.transform.position;
            mainCamera.transform.position = Vector3.Lerp( mainCamera.transform.position, new Vector3(position.x, position.y, cameraZOffset), cameraMoveSpeed * Time.deltaTime);
        }
    }
}