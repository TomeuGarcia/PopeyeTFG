using Popeye.Scripts.Collisions;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.OnVoid.VoidPhysics
{
    public class OnVoidPhysicsChecker : IOnVoidChecker
    {
        private readonly IPhysicsCaster _physicsCaster;
        private readonly Timer _checkVoidTimer;
        public bool IsOnVoid { get; private set; }
        

        public OnVoidPhysicsChecker(IPhysicsCaster physicsCaster, float checkFrequencyInSeconds)
        {
            _physicsCaster = physicsCaster;
            _checkVoidTimer = new Timer(checkFrequencyInSeconds);
            ClearState();
        }
        
        public void UpdateChecking(float deltaTime)
        {
            _checkVoidTimer.Update(deltaTime);
            if (_checkVoidTimer.HasFinished())
            {
                _checkVoidTimer.Clear();
                UpdateIsOnVoid();
            }
        }

        public void ClearState()
        {
            IsOnVoid = false;
            _checkVoidTimer.Clear();
        }

        private void UpdateIsOnVoid()
        {
            IsOnVoid = !_physicsCaster.CheckHit(out RaycastHit groundHit);
        }
    }
}