using System;
using Cysharp.Threading.Tasks;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Camera.Target;
using Popeye.Modules.Camera.TargetSwapper;
using Popeye.Modules.GameState;
using Popeye.Modules.WorldElements.WorldInteractors.Relay;
using UnityEngine;

namespace Popeye.Modules.Camera.TargetSeizing
{
    
    [RequireComponent(typeof(CameraTarget))]
    public class CameraSeizer : MonoBehaviour, IWorldInteractorRelayListener
    {
        [Header("CAMERA TARGET")]
        [SerializeField] private CameraTarget _cameraTarget;

        private ICameraTargetSwapper _cameraTargetSwapper;
        private IGameStateEventsDispatcher _gameStateEventsDispatcher;
        

        private void Start()
        {
            _cameraTargetSwapper = ServiceLocator.Instance.GetService<ICameraFunctionalities>().CameraTargetSwapper;
            _gameStateEventsDispatcher = ServiceLocator.Instance.GetService<IGameStateEventsDispatcher>();
        }


        public async UniTask OnActivateRelayStarted()
        {
            _gameStateEventsDispatcher.InvokeOnStartCameraAnimation();
            await _cameraTargetSwapper.SwapCameraTarget(_cameraTarget);
        }

        public void OnActivateRelayFinished()
        {
            _gameStateEventsDispatcher.InvokeOnFinishCameraAnimation();
            _cameraTargetSwapper.RestoreOriginalCameraTarget();
        }
        
    }
}