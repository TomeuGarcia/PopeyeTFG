using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorSnapController
    {
        private IAnchorSnapTarget _currentSnapTarget;
        private TrajectoryHitChecker _trajectoryHitChecker;

        public bool HasSnapTarget => _currentSnapTarget != null;
        
        

        public void Configure(TrajectoryHitChecker trajectoryHitChecker)
        {
            _trajectoryHitChecker = trajectoryHitChecker;
        }


        public bool CheckForSnapTarget(Vector3[] trajectoryPath)
        {
            if (_trajectoryHitChecker.GetFirstTriggerHitInTrajectoryPath(trajectoryPath, 
                    out RaycastHit hit, out int trajectoryHitIndex))
            {
                if (CheckHitIsSnapTarget(hit, out IAnchorSnapTarget snapTarget))
                {
                    if (HasSnapTarget)
                    {
                        RemoveCurrentSnapTarget();
                    }
                    AddNewCurrentSnapTarget(snapTarget);

                    return true;
                }
                
                if (HasSnapTarget)
                {
                    RemoveCurrentSnapTarget();
                }
            }
            

            return false;
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
        private void RemoveCurrentSnapTarget()
        {
            _currentSnapTarget.QuitPrepareForSnapping();
            ClearState();
        }
        
        
        
        public void ConfirmCurrentTarget(float durationBeforeReachingTarget)
        {
            _currentSnapTarget.PlaySnapAnimation(durationBeforeReachingTarget).Forget();
            ClearState();
        }

        public Vector3 GetTargetSnapPosition()
        {
            return _currentSnapTarget.GetSnapPosition();
        }
        public Quaternion GetTargetSnapRotation()
        {
            return _currentSnapTarget.GetSnapRotation();
        }

        private void ClearState()
        {
            _currentSnapTarget = null;
        }
    }
}