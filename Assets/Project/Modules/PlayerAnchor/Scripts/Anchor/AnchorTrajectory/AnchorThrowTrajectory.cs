using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorThrowTrajectory
    {
        private AnchorTrajectoryEndSpot _trajectoryEndSpot;
        private AnchorThrowConfig _anchorThrowConfig;


        private Vector3[] _straightTrajectoryPath;
        private Vector3[] _curvedEndTrajectoryPath;

        private LineRenderer _debugLine;
        public void Configure(AnchorTrajectoryEndSpot trajectoryEndSpot, LineRenderer debugLine)
        {
            _trajectoryEndSpot = trajectoryEndSpot;

            _straightTrajectoryPath = new Vector3[2];
            _curvedEndTrajectoryPath = new Vector3[20];
            _debugLine = debugLine;
        }
        



        public void UpdateTrajectoryEndSpot()
        {
            if (GetFirstHitInTrajectoryPath(_curvedEndTrajectoryPath, out RaycastHit hit))
            {
                Vector3 position = hit.point + hit.normal * 0.05f;
                _trajectoryEndSpot.MatchSpot(position, hit.normal);
            }

            Vector3[] debugPositions = _curvedEndTrajectoryPath;
            _debugLine.positionCount = debugPositions.Length;
            _debugLine.SetPositions(debugPositions);
        }
        public void ShowTrajectoryEndSpot()
        {
            _trajectoryEndSpot.Show();
        }
        public void HideTrajectoryEndSpot()
        {
            _trajectoryEndSpot.Hide();
        }


        public void UpdateTrajectoryPath(Vector3 startPoint, Vector3 direction, float distance)
        {
            DoUpdateTrajectoryPath(_straightTrajectoryPath, startPoint, direction, distance);
            DoUpdateTrajectoryPath(_curvedEndTrajectoryPath, startPoint, direction, distance);
            CurveTrajectoryPath(_curvedEndTrajectoryPath);
        }
        
        private void DoUpdateTrajectoryPath(Vector3[] trajectoryPath, Vector3 startPoint, Vector3 direction, float distance)
        {
            trajectoryPath[0] = startPoint;

            int steps = trajectoryPath.Length - 1;
            float distanceStep = distance / steps;
            
            for (int i = 1; i < trajectoryPath.Length; ++i)
            {
                trajectoryPath[i] = startPoint + (direction * (distanceStep * i));
            }
        }
        
        private void CurveTrajectoryPath(Vector3[] trajectoryPath)
        {
            for (int i = trajectoryPath.Length / 2; i < trajectoryPath.Length; ++i)
            {
                float t = (float)i / (trajectoryPath.Length - 1.0f);
                t = Mathf.Pow(t, 10);
                
                if (Physics.Raycast(trajectoryPath[i], Vector3.down, out RaycastHit hit, 1000, 
                        PositioningHelper.Instance.ObstaclesLayerMask, QueryTriggerInteraction.Ignore))
                {
                    trajectoryPath[i] = Vector3.Lerp(trajectoryPath[i], hit.point, t);
                }
            }
        }
        
        
        

        private bool GetFirstHitInTrajectoryPath(Vector3[] trajectoryPath, out RaycastHit trajectoryHit)
        {
            for (int i = 0; i < trajectoryPath.Length - 1; ++i)
            {
                Vector3 pointA = trajectoryPath[i];
                Vector3 pointB = trajectoryPath[i+1];
                Vector3 AtoB = pointB - pointA;

                if (Physics.Raycast(pointA, AtoB.normalized, out trajectoryHit, AtoB.magnitude, 
                        PositioningHelper.Instance.ObstaclesLayerMask, QueryTriggerInteraction.Ignore))
                {
                    return true;
                }
            }
            
            if (Physics.Raycast(trajectoryPath[trajectoryPath.Length - 1], Vector3.down, out trajectoryHit, 
                    1000, PositioningHelper.Instance.ObstaclesLayerMask,QueryTriggerInteraction.Ignore))
            {
                return true;
            }
            
            
            trajectoryHit = new RaycastHit();
            return false;
        }

    }
}