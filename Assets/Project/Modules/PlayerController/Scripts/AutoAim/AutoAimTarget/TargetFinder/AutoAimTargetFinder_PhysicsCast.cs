using System.Collections.Generic;
using Popeye.Scripts.Collisions;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargetFinder_PhysicsCast : IAutoAimTargetFinder
    {
        private Vector3 TargeterPosition => _targeter.position;
        private Vector3 TargeterForwardDirection => _targeter.forward;
        private Vector3 TargeterUpDirection => _targeter.up;
        private float Radius => _finderConfig.RadiusDistance;
        private LayerMask LayerMask => _probingConfig.CollisionLayerMask;
        private QueryTriggerInteraction QueryTriggerInteraction => _probingConfig.QueryTriggerInteraction;

        
        private AutoAimTargetFinderConfig _finderConfig;
        private CollisionProbingConfig _probingConfig;
        private IAutoAimTargetFilterer _autoAimTargetFilterer;
        private Transform _targeter;
        
        private Collider[] _colliders;

        
        public void Configure(AutoAimTargetFinderConfig finderConfig, CollisionProbingConfig collisionProbingConfig, 
            IAutoAimTargetFilterer autoAimTargetFilterer, Transform targeter)
        {
            _finderConfig = finderConfig;
            _probingConfig = collisionProbingConfig;
            _autoAimTargetFilterer = autoAimTargetFilterer;
            _targeter = targeter;
            
            _colliders = new Collider[50];
        }
        
        
        public bool GetAutoAimTargetsData(out IAutoAimTarget[] targets)
        {
            int size = Physics.OverlapSphereNonAlloc(TargeterPosition, Radius, _colliders, 
                LayerMask, QueryTriggerInteraction);


            List<IAutoAimTarget> hitTargets = new List<IAutoAimTarget>(size);
            for (int i = 0; i < size; ++i)
            {
                if (!_colliders[i].TryGetComponent<IAutoAimTarget>(out IAutoAimTarget autoAimTarget))
                {
                    continue;
                }
                
                if (!_autoAimTargetFilterer.IsValidTarget(autoAimTarget, TargeterPosition, 
                        TargeterForwardDirection, TargeterUpDirection))
                {
                    continue;
                }

                hitTargets.Add(autoAimTarget);
            }

            targets = hitTargets.ToArray();
            return targets.Length > 0;
        }
        
        
    }
}