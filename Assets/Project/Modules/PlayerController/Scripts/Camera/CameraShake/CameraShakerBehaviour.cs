using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.Camera.CameraShake
{
    public class CameraShakerBehaviour : MonoBehaviour, ICameraShaker
    {
        [SerializeField] private OrbitingCamera _orbitingCamera;

        
        private void Update()
        {
            _orbitingCamera.focusPointOffset = _orbitingCamera.FocusTransform.localPosition;
        }

        
        public async UniTaskVoid PlayShake(CameraShakeConfig shakeConfig)
        {
            await _orbitingCamera.FocusTransform.DOPunchPosition(
                    Vector3.down * shakeConfig.Strength, shakeConfig.Duration)
                .SetEase(shakeConfig.EaseCurve)
                .AsyncWaitForCompletion();
        }
    }
}