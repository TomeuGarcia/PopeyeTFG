using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class AnchorTrajectorySnapController
    {
        private IAnchorTrajectorySnapTarget _currentSnapTarget;

        public bool HasAutoAimTarget => _currentSnapTarget != null;
        public IAnchorTrajectorySnapTarget AnchorSnapTarget => _currentSnapTarget;
        
        

        public void Configure()
        {
            _currentSnapTarget = null;
        }


        public void ManageNoAutoAimTargetFound()
        {
            if (HasAutoAimTarget)
            {
                RemoveCurrentAutoAimTarget();
            }
        }
        
        public void ManageAutoAimTargetFound(IAnchorTrajectorySnapTarget snapTarget)
        {
            if (HasAutoAimTarget)
            {
                if (_currentSnapTarget != snapTarget)
                {
                    RemoveCurrentAutoAimTarget();
                    AddNewCurrentAutoAimTarget(snapTarget);
                }
            }
            else
            {
                AddNewCurrentAutoAimTarget(snapTarget);
            }
        }
        

        private void AddNewCurrentAutoAimTarget(IAnchorTrajectorySnapTarget newSnapTarget)
        {
            _currentSnapTarget = newSnapTarget;
            _currentSnapTarget.OnAddedAsAimTarget();
        }
        public void RemoveCurrentAutoAimTarget()
        {
            _currentSnapTarget.OnRemovedFromAimTarget();
            ClearState();
        }
        
        public void UseCurrentTarget(float durationBeforeReachingTarget, Transform user)
        {
            _currentSnapTarget.OnUsedAsAimTarget(durationBeforeReachingTarget, user);
        }

        
        public Vector3 GetTargetAimLockPosition()
        {
            return _currentSnapTarget.GetAimLockPosition();
        }
        public Quaternion GetTargetRotation()
        {
            return _currentSnapTarget.GetRotationForAimedTargeter();
        }

        public void ClearState()
        {
            _currentSnapTarget = null;
        }
    }
}