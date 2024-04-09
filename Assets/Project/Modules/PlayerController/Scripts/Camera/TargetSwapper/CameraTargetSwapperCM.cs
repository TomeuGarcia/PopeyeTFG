using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Popeye.Modules.Camera.Target;
using Project.Scripts.Core.DataStructures;
using UnityEngine;

namespace Popeye.Modules.Camera.TargetSwapper
{
    public class CameraTargetSwapperCM : ICameraTargetSwapper
    {
        private readonly CinemachineBrain _brain;
        private readonly CinemachineVirtualCamera _originalVirtualCamera;
        private readonly CircularBuffer<CinemachineVirtualCamera> _swappingVirtualCamerasBuffer;
        private readonly ICameraSwapDurationComputer _swapDurationComputer;
        private readonly bool _ignoreTimeScale;

        private const int ORIGINAL_CAMERA_PRIORITY = 1;
        private const int SWAPPING_CAMERA_ENABLED_PRIORITY = 2;
        private const int SWAPPING_CAMERA_DISABLED_PRIORITY = 0;

        private CinemachineVirtualCamera _currentSwappingVirtualCamera;
        private Transform OriginalCameraFollow => _originalVirtualCamera.Follow;
        public float ActiveSwapDuration { get; private set; }


        public CameraTargetSwapperCM(
            CinemachineBrain brain,
            CinemachineVirtualCamera originalVirtualCamera,
            CinemachineVirtualCamera[] swappingVirtualCamerasBuffer,
            ICameraSwapDurationComputer swapDurationComputer,
            bool ignoreTimeScale
        )
        {
            _brain = brain;
            _originalVirtualCamera = originalVirtualCamera;
            _swappingVirtualCamerasBuffer = new CircularBuffer<CinemachineVirtualCamera>(swappingVirtualCamerasBuffer);
            _swapDurationComputer = swapDurationComputer;
            _ignoreTimeScale = ignoreTimeScale;

            _brain.m_IgnoreTimeScale = _ignoreTimeScale;
            
            _originalVirtualCamera.Priority = ORIGINAL_CAMERA_PRIORITY;
            RestoreOriginalCameraTarget();
            
            CinemachineCore.GetBlendOverride += OnBlendStart;
        }

        ~CameraTargetSwapperCM()
        {
            CinemachineCore.GetBlendOverride -= OnBlendStart;
        }
        
        
        public void RestoreOriginalCameraTarget()
        {
            foreach (CinemachineVirtualCamera virtualCamera in _swappingVirtualCamerasBuffer.Elements)
            {
                virtualCamera.Priority = SWAPPING_CAMERA_DISABLED_PRIORITY;
            }
        }

        public async UniTask SwapCameraTarget(CameraTarget cameraTarget)
        {
            _currentSwappingVirtualCamera = _swappingVirtualCamerasBuffer.GetNext();
            
            _currentSwappingVirtualCamera.Follow = cameraTarget.TargetTransform;
            _currentSwappingVirtualCamera.LookAt = cameraTarget.TargetTransform;
            _currentSwappingVirtualCamera.Priority = SWAPPING_CAMERA_ENABLED_PRIORITY;

            UpdateCameraBlendDuration(cameraTarget);
            
            await UniTask.Delay(TimeSpan.FromSeconds(ActiveSwapDuration), ignoreTimeScale: _ignoreTimeScale);
        }

        private void UpdateCameraBlendDuration(CameraTarget cameraTarget)
        {
            ActiveSwapDuration =
                _swapDurationComputer.ComputeDuration(OriginalCameraFollow, cameraTarget.TargetTransform);
        }


        private CinemachineBlendDefinition OnBlendStart(
            ICinemachineCamera fromVcam, ICinemachineCamera toVcam,
            CinemachineBlendDefinition defaultBlend,
            MonoBehaviour owner)
        {
            return new CinemachineBlendDefinition(defaultBlend.m_Style, ActiveSwapDuration);
        }
        
    }
}