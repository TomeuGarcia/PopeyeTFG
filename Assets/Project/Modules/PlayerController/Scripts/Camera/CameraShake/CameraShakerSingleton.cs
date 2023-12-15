using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Popeye.Modules.Camera.CameraShake;
using UnityEngine;

namespace Popeye.Modules.Camera
{
    public class CameraShakerSingleton : MonoBehaviour, ICameraShaker
    {

        private static CameraShakerSingleton _instance;
        public static CameraShakerSingleton Instance => _instance;


        [SerializeField] private Transform _shakeTransform;
        [SerializeField] private OrbitingCamera _orbitingCamera;


        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = this;
            transform.SetParent(null);
            transform.localPosition = Vector3.zero;
        }


        private void Update()
        {
            _orbitingCamera.focusPointOffset = _shakeTransform.localPosition;
        }

        public async void PlayShake(float strength, float duration)
        {
            await _shakeTransform.DOPunchPosition(Vector3.down * strength, duration)
                .AsyncWaitForCompletion();
        }
        
        public async UniTaskVoid PlayShake(CameraShakeConfig shakeConfig)
        {
            await _shakeTransform.DOPunchPosition(Vector3.down * shakeConfig.Strength, shakeConfig.Duration)
                .SetEase(shakeConfig.EaseCurve)
                .AsyncWaitForCompletion();
        }

    }
}