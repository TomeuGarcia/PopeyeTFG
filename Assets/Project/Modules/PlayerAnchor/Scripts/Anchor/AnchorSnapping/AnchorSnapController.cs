using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorSnapController
    {
        private IAnchorSnapTarget _currentSnapTarget;
        private TrajectoryHitChecker _trajectoryHitChecker;

        public bool HasSnapTarget => _currentSnapTarget != null;
        public IAnchorSnapTarget AnchorSnapTarget => _currentSnapTarget;
        
        

        public void Configure(TrajectoryHitChecker trajectoryHitChecker)
        {
            _trajectoryHitChecker = trajectoryHitChecker;
        }


        public bool CheckForSnapTarget(Vector3[] trajectoryPath)
        {
            if (!_trajectoryHitChecker.GetFirstTriggerHitInTrajectoryPath(trajectoryPath,
                    out RaycastHit hit, out int trajectoryHitIndex))
            {
                if (HasSnapTarget)
                {
                    RemoveCurrentSnapTarget();
                }

                return false;
            }

            if (!CheckHitIsSnapTarget(hit, out IAnchorSnapTarget snapTarget))
            {
                if (HasSnapTarget)
                {
                    RemoveCurrentSnapTarget();
                }
                
                return false;
            }
            
            
            if (!snapTarget.CanSnapFromPosition(trajectoryPath[0]))
            {
                if (HasSnapTarget)
                {
                    RemoveCurrentSnapTarget();
                }
                
                return false;
            }
            
            
            if (HasSnapTarget)
            {
                if (_currentSnapTarget != snapTarget)
                {
                    RemoveCurrentSnapTarget();
                    AddNewCurrentSnapTarget(snapTarget);
                }
            }
            else
            {
                AddNewCurrentSnapTarget(snapTarget);
            }
                
            return true;
        }
        
        private bool CheckHitIsSnapTarget(RaycastHit hit, out IAnchorSnapTarget snapTarget)
        {
            
            return hit.collider.TryGetComponent(out snapTarget);
        }
        

        private void AddNewCurrentSnapTarget(IAnchorSnapTarget newSnapTarget)
        {
            _currentSnapTarget = newSnapTarget;
            _currentSnapTarget.EnterPrepareForSnapping();
        }
        public void RemoveCurrentSnapTarget()
        {
            _currentSnapTarget.QuitPrepareForSnapping();
            ClearState();
        }
        
        
        
        public void ConfirmCurrentTarget(float durationBeforeReachingTarget)
        {
            _currentSnapTarget.PlaySnapAnimation(durationBeforeReachingTarget).Forget();
        }

        public Vector3 GetTargetSnapPosition()
        {
            return _currentSnapTarget.GetSnapPosition();
        }
        public Quaternion GetTargetSnapRotation()
        {
            return _currentSnapTarget.GetSnapRotation();
        }

        public void ClearState()
        {
            _currentSnapTarget = null;
        }
    }
}