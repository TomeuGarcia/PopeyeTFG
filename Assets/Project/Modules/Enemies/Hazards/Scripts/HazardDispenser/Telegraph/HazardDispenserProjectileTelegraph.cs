using System;
using Cysharp.Threading.Tasks;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.ParticleFactories;
using Popeye.Scripts.Collisions;
using Project.Scripts.TweenExtensions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Popeye.Modules.Enemies.Hazards.Telegraph
{
    public class HazardDispenserProjectileTelegraph : MonoBehaviour, IHazardDispenserTelegraph
    {
        [SerializeField] private float _telegraphDuration;
        [SerializeField] private Transform _telegraphHolder;
        [SerializeField] private ParticleTypes _dispenserProjectileTelegraph;
        [SerializeField] private CollisionProbingConfig _raycastConfig;
        private IParticleFactory _particleFactory;

        private void Start()
        {
            _particleFactory = ServiceLocator.Instance.GetService<IParticleFactory>();
        }

        public async UniTaskVoid TelegraphHazard(float duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(0.0f, duration - _telegraphDuration)));

            Transform particle = _particleFactory.Create(_dispenserProjectileTelegraph, Vector3.zero, quaternion.identity, _telegraphHolder);

            RaycastHit hit;
            if (Physics.Raycast(_telegraphHolder.position, _telegraphHolder.forward, out hit,
                    _raycastConfig.ProbeDistance, _raycastConfig.CollisionLayerMask,
                    _raycastConfig.QueryTriggerInteraction))
            {
                float distance = (_telegraphHolder.position - hit.point).magnitude;
                particle.localScale = Vector3.one + Vector3.forward * (distance - 1.0f);
            }
            else
            {
                particle.localScale = Vector3.forward * _raycastConfig.ProbeDistance;
            }
            
        }
    }
}