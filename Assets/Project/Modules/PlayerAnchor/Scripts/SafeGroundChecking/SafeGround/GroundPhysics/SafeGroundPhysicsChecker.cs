using System;
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
        private float _bestSafePositionDistanceStep;

        public Vector3 LastSafePosition { get; private set; }
        public Vector3 BestSafePosition => ComputeBestSafePosition();



        public SafeGroundPhysicsChecker(Transform positionTrackingTransform, IPhysicsCaster physicsCaster,
             float checkFrequencyInSeconds, float bestSafePositionDistanceStep)
        {
            _positionTrackingTransform = positionTrackingTransform;
            _physicsCaster = physicsCaster;
            _bestSafePositionDistanceStep = bestSafePositionDistanceStep;
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

        private Vector3 ComputeBestSafePosition()
        {
            Vector3 currentPosition = _positionTrackingTransform.position;
            currentPosition.y = LastSafePosition.y;

            Vector3 lastSafeToCurrent = currentPosition - LastSafePosition;
            float distanceLastSafeToCurrent = lastSafeToCurrent.magnitude;
            Vector3 lastSafeToCurrentDirection = lastSafeToCurrent / distanceLastSafeToCurrent;


            float distanceCounter = _bestSafePositionDistanceStep;

            Vector3 beforePreviousSimulatedPosition = LastSafePosition;
            Vector3 previousSimulatedPosition = LastSafePosition;

            float checkDistance = distanceLastSafeToCurrent - _bestSafePositionDistanceStep;
            
            while (distanceCounter < checkDistance)
            {
                Vector3 simulatedPosition = LastSafePosition + (lastSafeToCurrentDirection * distanceCounter);

                if (!_physicsCaster.CheckHitAtPosition(out RaycastHit groundHit, simulatedPosition))
                {
                    break;
                }

                beforePreviousSimulatedPosition = previousSimulatedPosition;
                previousSimulatedPosition = simulatedPosition;

                
                distanceCounter += _bestSafePositionDistanceStep;
            }
            
            return Vector3.Lerp(beforePreviousSimulatedPosition, previousSimulatedPosition, 0.5f);
        }
        
    }
}