using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.Camera.CameraShake
{
    public class CameraShakerCM : MonoBehaviour, ICameraShaker
    {
        [SerializeField] private OrbitingCameraCM _orbitingCamera;
        
        [SerializeField] private CinemachineIndependentImpulseListener _impulseListener;
        [SerializeField] private CinemachineImpulseSource _impulseSource;
        
        
        private CinemachineBasicMultiChannelPerlin _noiseComponent;
        private delegate UniTask PlayShakeFunction(CameraShakeConfig shakeConfig);

        private Dictionary<CameraShakeConfig.ShakeDirectionType, PlayShakeFunction> _shakeTypeToPlayFunction;


        private void Awake()
        {
            _noiseComponent = _orbitingCamera.VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _noiseComponent.m_AmplitudeGain = 0f;

            _shakeTypeToPlayFunction = new Dictionary<CameraShakeConfig.ShakeDirectionType, PlayShakeFunction>
            {
                { CameraShakeConfig.ShakeDirectionType.Random, PlayRandomShake },
                { CameraShakeConfig.ShakeDirectionType.Deterministic, PlayDeterministicShake }
            };
        }
        
        
        public async UniTaskVoid PlayShake(CameraShakeConfig shakeConfig)
        {
            await _shakeTypeToPlayFunction[shakeConfig.DirectionType](shakeConfig);
        }

        private async UniTask PlayRandomShake(CameraShakeConfig shakeConfig)
        {
            float strength = shakeConfig.Strength;
            _noiseComponent.m_AmplitudeGain = shakeConfig.Strength;
            
            await DOTween.To(
                    () => strength,
                    (value) => strength = _noiseComponent.m_AmplitudeGain = value,
                    0f,
                    shakeConfig.Duration
                )
                .SetEase(shakeConfig.EaseCurve)
                .AsyncWaitForCompletion();
        }
        
        private async UniTask PlayDeterministicShake(CameraShakeConfig shakeConfig)
        {
            _impulseSource.m_ImpulseDefinition.m_ImpulseDuration = shakeConfig.Duration;
            _impulseSource.m_DefaultVelocity = shakeConfig.Direction;

            _impulseListener.m_ReactionSettings.m_AmplitudeGain = shakeConfig.Strength;
            _impulseListener.m_ReactionSettings.m_Duration = shakeConfig.Duration;
            
            _impulseSource.GenerateImpulseWithForce(shakeConfig.Strength);


            await UniTask.Delay(TimeSpan.FromSeconds(shakeConfig.Duration));
        }
        
    }
}