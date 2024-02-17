using System;
using Popeye.Scripts.Collisions;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking
{
    public class SafeGroundPhysicsChecker : ISafeGroundChecker
    {
        private readonly Transform _positionTrackingTransform;
        private readonly IPhysicsCaster _physicsCaster;
        private readonly Timer _checkGroundTimer;
        private readonly ISafeGroundPhysicsRequirement[] _requirements;

        public Vector3 LastSafePosition { get; private set; }
        

        public SafeGroundPhysicsChecker(Transform positionTrackingTransform, IPhysicsCaster physicsCaster,
             float checkFrequencyInSeconds, ISafeGroundPhysicsRequirement[] requirements = null)
        {
            _positionTrackingTransform = positionTrackingTransform;
            _physicsCaster = physicsCaster;
            _checkGroundTimer = new Timer(checkFrequencyInSeconds);
            
            _requirements = requirements ?? Array.Empty<ISafeGroundPhysicsRequirement>();
        }
        
        
        public void UpdateChecking(float deltaTime)
        {
            _checkGroundTimer.Update(deltaTime);
            if (_checkGroundTimer.HasFinished())
            {
                _checkGroundTimer.Clear();
                CheckLastSafePosition();
            }
        }


        private void CheckLastSafePosition()
        {
            if (_physicsCaster.CheckHit(out RaycastHit groundHit))
            {
                foreach (ISafeGroundPhysicsRequirement requirement in _requirements)
                {
                    if (!requirement.MeetsRequirement(groundHit))
                    {
                        return;
                    }
                }
                
                LastSafePosition = _positionTrackingTransform.position;
            }
        }
        
        
        
    }
}