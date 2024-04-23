using System.Collections.Generic;
using UnityEngine;

namespace Popeye.InverseKinematics.FABRIK
{

    public class FABRIKController
    {
        private readonly FABRIKControllerConfig _config;

        private readonly List<FABRIKJointChain> _jointChains;

        public FABRIKController(FABRIKControllerConfig config)
        {
            _config = config;

            _jointChains = new List<FABRIKJointChain>();
        }

        public void AddJointChain(FABRIKJointChain jointChain)
        {
            _jointChains.Add(jointChain);
        }

        public void RemoveJointChains()
        {
            _jointChains.Clear();
        }

        public void Update()
        {
            foreach (FABRIKJointChain jointChain in _jointChains)
            {
                UpdateJointChain2(jointChain);
            }
        }


        private void UpdateJointChain2(FABRIKJointChain jointChain)
        {
            ResetPositionCopies(jointChain);


            Vector3 targetPosition = jointChain.IsTargetUnreachable() ? 
                jointChain.RootToTarget * (jointChain.DistancesSum + _config.DistanceToEndEffectorTolerance) : 
                jointChain.TargetPosition;

            int tries = 0;
            
            while (jointChain.EndEffectorCopyToTargetDistance(targetPosition) > _config.DistanceToEndEffectorTolerance
                   && tries++ < _config.MaxTries)
            {
                ForwardReaching(jointChain);
                BackwardReaching(jointChain);
            }

            UpdateJoints(jointChain);
        }


        private void ResetPositionCopies(FABRIKJointChain jointChain)
        {
            for (int i = 0; i < jointChain.NumberOfJoints; ++i)
            {
                jointChain.PositionCopies[i] = jointChain.Joints[i].position;
            }
        }

        private void SetPositionsStraight(FABRIKJointChain jointChain)
        {
            for (int i = 0; i < jointChain.NumberOfJoints - 1; ++i)
            {
                // Find the distance between the target and the joint
                float targetToJointDist = Vector3.Distance(jointChain.TargetPosition, jointChain.PositionCopies[i]);
                float ratio = jointChain.Distances[i] / targetToJointDist;

                // Find the new joint position
                jointChain.PositionCopies[i + 1] =
                    (1 - ratio) * jointChain.PositionCopies[i] + ratio * jointChain.TargetPosition;
            }
        }


        private void ForwardReaching(FABRIKJointChain jointChain)
        {
            // Set end effector as target
            jointChain.PositionCopies[jointChain.NumberOfJoints - 1] = jointChain.TargetPosition;

            for (int i = jointChain.NumberOfJoints - 2; i >= 0; --i)
            {
                // Find the distance between the new joint position (i+1) and the current joint (i)
                float distanceBetweenJoints =
                    Vector3.Distance(jointChain.PositionCopies[i], jointChain.PositionCopies[i + 1]);
                float ratio = jointChain.Distances[i] / distanceBetweenJoints;

                // Find the new joint position
                jointChain.PositionCopies[i] = (1 - ratio) * jointChain.PositionCopies[i + 1] +
                                               ratio * jointChain.PositionCopies[i];
            }
        }

        private void BackwardReaching(FABRIKJointChain jointChain)
        {
            // Set the root its initial position
            jointChain.PositionCopies[0] = jointChain.Joints[0].position;

            for (int i = 1; i < jointChain.NumberOfJoints - 1; ++i)
            {
                // Find the distance between the new joint position (i+1) and the current joint (i)
                float distanceJoints = Vector3.Distance(jointChain.PositionCopies[i - 1], jointChain.PositionCopies[i]);
                float ratio = jointChain.Distances[i - 1] / distanceJoints;

                // Find the new joint position
                jointChain.PositionCopies[i] = (1 - ratio) * jointChain.PositionCopies[i - 1] +
                                               ratio * jointChain.PositionCopies[i];
            }
        }

        private void UpdateJoints(FABRIKJointChain jointChain)
        {
            for (int i = 0; i < jointChain.NumberOfJoints - 1; i++)
            {
                Vector3 oldDirection = (jointChain.Joints[i + 1].position - jointChain.Joints[i].position).normalized;
                Vector3 newDirection = (jointChain.PositionCopies[i + 1] - jointChain.PositionCopies[i]).normalized;

                Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
                float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;

                if (angle > _config.AngleThreshold)
                {
                    Quaternion originRotation = jointChain.Joints[i].rotation;
                    Quaternion goalRotation = Quaternion.AngleAxis(angle, axis) * originRotation;
                    float t = Mathf.Clamp01(Time.deltaTime * _config.LerpSpeed);
                    Quaternion currentRotation = Quaternion.Slerp(originRotation, goalRotation, t);
                    jointChain.Joints[i].rotation = currentRotation;
                }
            }
        }

    }
}