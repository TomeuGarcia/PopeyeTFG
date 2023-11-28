using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Camera
{
    public class CameraShake : MonoBehaviour
    {

        private static CameraShake _instance;
        public static CameraShake Instance => _instance;


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

    }
}