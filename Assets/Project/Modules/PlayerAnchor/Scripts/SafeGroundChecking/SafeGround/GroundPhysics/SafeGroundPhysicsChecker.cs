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

        public Vector3 LastSafePosition { get; private set; }
        

        public SafeGroundPhysicsChecker(Transform positionTrackingTransform, IPhysicsCaster physicsCaster,
             float checkFrequencyInSeconds)
        {
            _positionTrackingTransform = positionTrackingTransform;
            _physicsCaster = physicsCaster;
            _checkGroundTimer = new Timer(checkFrequencyInSeconds);
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
                LastSafePosition = _positionTrackingTransform.position;
            }
        }
        
        
        
        
        
    }
}