using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.ParticleFactories;
using Project.Scripts.TweenExtensions;
using Unity.Mathematics;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class HazardDispenserView : MonoBehaviour, IHazardDispenserView
    {
        [Header("COMPONENTS")]
        [SerializeField] private Transform _spitterHolder;
        [SerializeField] private Transform _chargePosition;
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        private Material _material;
        private HazardDispenserViewConfig _config;
        private IParticleFactory _particleFactory;
        
        public void Configure(HazardDispenserViewConfig config)
        {
            _config = config;
            _particleFactory = ServiceLocator.Instance.GetService<IParticleFactory>();
            _material = _meshRenderer.material;
        }

        public void PlayPrepareDispensingAnimation(float duration)
        {
            _particleFactory.Create(_config.ChargeParticleType, Vector3.zero, quaternion.identity, _chargePosition);
            
            /*
            _spitterHolder.DOComplete();
            _spitterHolder.DOBlendableLocalRotateBy(_config.PrepareRotation.Value, duration)
                .SetEase(_config.PrepareRotation.Ease);
            _spitterHolder.DOScale(_config.PrepareScale.Value, duration)
                .SetEase(_config.PrepareScale.Ease);
            */
        }

        public void PlayDispenseAnimation()
        {
            MaterialInterpolator.ApplyInterpolations(_material, _config.ReadyActivate).Forget();
            
            /*
            _spitterHolder.DOComplete();
            _spitterHolder.DOLocalRotateQuaternion(Quaternion.identity, _config.DispenseScalePunch.Duration)
                .SetEase(Ease.InOutSine);
            _spitterHolder.PunchScale(_config.DispenseScalePunch);
            */
        }

        public async UniTask PlayReadyToDispenseAnimation()
        {
            /*
            _spitterHolder.PunchRotation(_config.ReadyRotationPunch);
            await _spitterHolder.DOScale(Vector3.one, _config.ReadyRotationPunch.Duration)
                .SetEase(Ease.InOutSine)
                .AsyncWaitForCompletion();
            */
        }
    }
}