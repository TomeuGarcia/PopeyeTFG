using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class TrajectoryHitChecker
    {
        private readonly int _obstacleLayerMask;
        private readonly int _triggerLayerMask;

        
        public TrajectoryHitChecker(int obstacleLayerMask, int triggerLayerMask)
        {
            _obstacleLayerMask = obstacleLayerMask;
            _triggerLayerMask = triggerLayerMask;
        }
        

        public bool GetFirstObstacleHitInTrajectoryPath(Vector3[] trajectoryPath, out RaycastHit trajectoryHit, 
            out int trajectoryIndex)
        {
            return GetFirstHitInTrajectoryPath(trajectoryPath, out trajectoryHit, out trajectoryIndex,
                _obstacleLayerMask, QueryTriggerInteraction.Ignore);
        }
        
        public bool GetFirstTriggerHitInTrajectoryPath(Vector3[] trajectoryPath, out RaycastHit trajectoryHit, 
            out int trajectoryIndex)
        {
            return GetFirstHitInTrajectoryPath(trajectoryPath, out trajectoryHit, out trajectoryIndex,
                _triggerLayerMask, QueryTriggerInteraction.Collide);
        }
        
        
        private bool GetFirstHitInTrajectoryPath(Vector3[] trajectoryPath, out RaycastHit trajectoryHit, 
            out int trajectoryIndex, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
        {
            for (int i = 0; i < trajectoryPath.Length - 1; ++i)
            {
                Vector3 pointA = trajectoryPath[i];
                Vector3 pointB = trajectoryPath[i+1];
                Vector3 AtoB = pointB - pointA;

                if (Physics.Raycast(pointA, AtoB.normalized, out trajectoryHit, AtoB.magnitude + 0.2f, 
                        layerMask, queryTriggerInteraction))
                {
                    trajectoryIndex = i;
                    return true;
                }
            }
            
            trajectoryIndex = -1;
            trajectoryHit = new RaycastHit();
            return false;
        }
        
    }
    
}