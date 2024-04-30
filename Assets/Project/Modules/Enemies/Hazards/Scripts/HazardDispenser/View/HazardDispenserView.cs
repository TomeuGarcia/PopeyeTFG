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
        }

        public void PlayDispenseAnimation()
        {
            MaterialInterpolator.ApplyInterpolations(_material, _config.ReadyActivate).Forget();
        }

        public async UniTask PlayReadyToDispenseAnimation()
        {
            
        }
    }
}