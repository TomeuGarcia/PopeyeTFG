using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class AnchorPuller : IAnchorPuller
    {
        private IPlayerMediator _player;
        private PopeyeAnchor _anchor;
        private AnchorTrajectoryMaker _anchorTrajectoryMaker;
        private AnchorPullConfig _pullConfig;

        private bool _anchorIsBeingPulled;
        
        
        public AnchorThrowResult AnchorPullResult { get; private set; }
        
        
        
        public void Configure(IPlayerMediator player, PopeyeAnchor anchor, 
            AnchorTrajectoryMaker anchorTrajectoryMaker,
            AnchorPullConfig pullConfig)
        {
            _player = player;
            _anchor = anchor;
            _anchorTrajectoryMaker = anchorTrajectoryMaker;
            _pullConfig = pullConfig;

            AnchorPullResult = new AnchorThrowResult(_pullConfig.MoveInterpolationCurve,
                _pullConfig.RotateInterpolationCurve);
        }

        private float debug_pullMultiplyMode = 0f;
        public void DebugTogglePullMode()
        {
            if (debug_pullMultiplyMode > 0.5f)
            {
                debug_pullMultiplyMode = 0f;
                Debug.Log("STRAIGHT PULL");
            }
            else
            {
                debug_pullMultiplyMode = 1f;
                Debug.Log("CHAIN PULL");
            }
        }


        public bool AnchorIsBeingPulled()
        {
            return _anchorIsBeingPulled;
        }

        public void PullAnchor()
        {
            Vector3 anchorPosition = _anchor.Position;
            Vector3 playerPosition = _player.GetAnchorThrowStartPosition();
            Vector3 pullDirection = -_player.GetFloorAlignedDirectionToAnchor();


            ComputeTrajectory(out Vector3[] trajectoryPath, out Quaternion[] rotationPath,
                out float trajectoryDistance, playerPosition, anchorPosition);

            float duration = ComputePullDuration(trajectoryDistance);

            Vector3 currentForward = _anchor.Forward;
            Vector3 goalForward = -pullDirection;
            
            
            Quaternion startRotation = _anchor.Rotation;

            Vector3 forwardRotationAxis = Vector3.Cross(currentForward, goalForward).normalized;
            float forwardRotationAngle = Mathf.Acos(Vector3.Dot(currentForward, goalForward)) * Mathf.Rad2Deg;
            Quaternion forwardOffsetRotation = Quaternion.AngleAxis(forwardRotationAngle, forwardRotationAxis);
            Quaternion endRotation = forwardOffsetRotation * startRotation;
            

            AnchorPullResult.Reset(trajectoryPath, pullDirection, startRotation, endRotation, 
                duration, false);

            
            _anchor.SetPulled(AnchorPullResult, rotationPath);
            DoPullAnchor(AnchorPullResult).Forget();
        }


        private void ComputeTrajectory(out Vector3[] trajectoryPath, out Quaternion[] rotationPath, 
            out float trajectoryDistance, Vector3 playerPosition, Vector3 anchorPosition)
        {
            trajectoryPath = _anchorTrajectoryMaker.ComputeCurvedTrajectory(anchorPosition, 
                playerPosition, 10, out trajectoryDistance);


            Vector3[] chainPositions = _anchor.GetChainPositions();
            float distanceRatio = _pullConfig.TrajectoryByDistance
                .Evaluate(ComputeDistanceRatio(_player.GetDistanceFromAnchor()))
                * debug_pullMultiplyMode;
            
            for (int i = 0; i < trajectoryPath.Length - 1; ++i)
            {
                trajectoryPath[i] = Vector3.Lerp(trajectoryPath[i], chainPositions[i], distanceRatio);
            }
            trajectoryPath[^1] = playerPosition;
            
            
            trajectoryDistance = 0;
            
            rotationPath = new Quaternion[trajectoryPath.Length];
            rotationPath[0] = Quaternion.LookRotation((anchorPosition-trajectoryPath[0]).normalized, Vector3.down);
            for (int i = 1; i < rotationPath.Length; ++i)
            {
                Vector3 toCurrent = (trajectoryPath[i - 1] - trajectoryPath[i]);
                Vector3 toCurrentDirection = toCurrent.normalized;
                Quaternion rotation = Quaternion.LookRotation(toCurrentDirection, Vector3.down);
                rotationPath[i] = rotation;

                trajectoryDistance += toCurrent.magnitude;
            }

        }
        
        
        private async UniTaskVoid DoPullAnchor(AnchorThrowResult anchorPullResult)
        {
            _anchorIsBeingPulled = true;
            await UniTask.Delay(TimeSpan.FromSeconds(anchorPullResult.Duration));
            
            _anchorIsBeingPulled = false;
        }


        private float ComputeDistanceRatio(float distance)
        {
            distance = Mathf.Clamp(distance, _pullConfig.MinPullDistance, _pullConfig.MaxPullDistance);
            distance -= _pullConfig.MinPullDistance;
            float maxRatio = _pullConfig.MaxPullDistance - _pullConfig.MinPullDistance;
            
            float distanceRatio01 = distance / maxRatio;

            return distanceRatio01;
        }
        private float ComputePullDuration(float distance)
        {
            float distanceRatio01 = ComputeDistanceRatio(distance);
            
            return Mathf.Lerp(_pullConfig.MinPullMoveDuration, _pullConfig.MaxPullMoveDuration,
                distanceRatio01);
        }
        
    }
}