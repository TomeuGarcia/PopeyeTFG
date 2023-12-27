using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class AnchorAutoAimController
    {
        private IAutoAimTarget _currentAutoAimTarget;

        public bool HasAutoAimTarget => _currentAutoAimTarget != null;
        public IAutoAimTarget AnchorAutoAimTarget => _currentAutoAimTarget;
        
        

        public void Configure()
        {
            _currentAutoAimTarget = null;
        }


        public void ManageNoAutoAimTargetFound()
        {
            if (HasAutoAimTarget)
            {
                RemoveCurrentAutoAimTarget();
            }
        }
        
        public void ManageAutoAimTargetFound(IAutoAimTarget autoAimTarget)
        {
            if (HasAutoAimTarget)
            {
                if (_currentAutoAimTarget != autoAimTarget)
                {
                    RemoveCurrentAutoAimTarget();
                    AddNewCurrentAutoAimTarget(autoAimTarget);
                }
            }
            else
            {
                AddNewCurrentAutoAimTarget(autoAimTarget);
            }
        }
        

        private void AddNewCurrentAutoAimTarget(IAutoAimTarget newSnapTarget)
        {
            _currentAutoAimTarget = newSnapTarget;
            _currentAutoAimTarget.OnAddedAsAimTarget();
        }
        public void RemoveCurrentAutoAimTarget()
        {
            _currentAutoAimTarget.OnRemovedFromAimTarget();
            ClearState();
        }
        
        public void UseCurrentTarget(float durationBeforeReachingTarget)
        {
            _currentAutoAimTarget.OnUsedAsAimTarget(durationBeforeReachingTarget);
        }

        
        public Vector3 GetTargetAimLockPosition()
        {
            return _currentAutoAimTarget.GetAimLockPosition();
        }
        public Quaternion GetTargetRotation()
        {
            return _currentAutoAimTarget.GetRotationForAimedTargeter();
        }

        public void ClearState()
        {
            _currentAutoAimTarget = null;
        }
    }
}