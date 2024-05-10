using Popeye.Scripts.Collisions;
using Popeye.Scripts.ObjectTypes;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.OnVoid.VoidPhysics
{
    public class OnVoidPhysicsChecker : IOnVoidChecker
    {
        private readonly IPhysicsCaster _physicsCaster;
        private readonly ObjectTypeAsset _voidGroundType;
        private readonly Timer _checkVoidTimer;
        public bool IsOnVoid { get; private set; }
        

        public OnVoidPhysicsChecker(IPhysicsCaster physicsCaster, float checkFrequencyInSeconds, ObjectTypeAsset voidGroundType)
        {
            _physicsCaster = physicsCaster;
            _voidGroundType = voidGroundType;
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

            if (groundHit.collider == null) return;
            if (groundHit.collider.gameObject.TryGetComponent(out IObjectType objectType))
            {
                IsOnVoid = objectType.IsOfType(_voidGroundType);
            }
        }
    }
}