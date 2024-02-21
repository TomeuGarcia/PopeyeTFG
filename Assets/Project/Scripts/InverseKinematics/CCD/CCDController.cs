using System.Collections.Generic;
using Project.Scripts.InverseKinematics.Quaternions;
using UnityEngine;

namespace Project.Scripts.InverseKinematics.CCD
{
    public class CCDController
    {
        private float _distanceToEndEffectorTolerance;

        private readonly List<CCDJointChain> _jointChains;

        public CCDController()
        {
            _distanceToEndEffectorTolerance = 0.01f;

            _jointChains = new List<CCDJointChain>(1);
        }

        public void AddJointChain(CCDJointChain jointChain)
        {
            _jointChains.Add(jointChain);
        }

        public void RemoveJointChains()
        {
            _jointChains.Clear();
        }

        public void Update()
        {
            foreach (var jointChain in _jointChains)
            {
                UpdateJointChain(jointChain);
            }
        }
        
        void UpdateJointChain(CCDJointChain jointChain)
        {
            Transform[] joints = jointChain.Joints;
            Vector3 targetPosition = jointChain.TargetPosition;


            Vector3 targetToEffector = joints[^1].position - targetPosition;
            bool done = targetToEffector.magnitude < _distanceToEndEffectorTolerance;
            
            // the target has moved, reset tries to 0 and change PreviousTargetPosition
            if (targetPosition != jointChain.PreviousTargetPosition)
            {
                jointChain.IterationTriesCounter = 0;
                jointChain.PreviousTargetPosition = targetPosition;
            }
            
            
            if (!done && jointChain.IterationTriesCounter <= jointChain.MaxIterationTries)
            {
                for (int i = jointChain.NumberOfJoints - 2; i >= 0; i--)
                {
                    Quaternion rotationBetweenTargetAndEndEffector = ComputeRotationBetweenTargetAndEndEffector(
                        joints[i].position, targetPosition, joints[^1].position);

                    joints[i].rotation = rotationBetweenTargetAndEndEffector * joints[i].rotation;
                        
                    QuaternionsHelper.ClampBoneRotation(joints[i].transform, 
                        jointChain.ClampedAnglesMin, jointChain.ClampedAnglesMax, jointChain.ClampAxis);

                }
                ++jointChain.IterationTriesCounter;
            }
            
        }


        private Quaternion ComputeRotationBetweenTargetAndEndEffector(Vector3 jointPosition, 
            Vector3 targetPosition, Vector3 endEffectorPosition)
        {
            Vector3 jointToEndEffector = (endEffectorPosition - jointPosition).normalized;
            Vector3 jointToTarget = (targetPosition - jointPosition).normalized;
            float cos, sin;
            
            // To avoid dividing by tiny numbers
            if (jointToEndEffector.magnitude * jointToTarget.magnitude <= 0.001f)
            {
                cos = 1.0f;
                sin = 0.0f;
            }
            else
            {
                cos = Vector3.Dot(jointToEndEffector, jointToTarget);
                sin = Vector3.Cross(jointToEndEffector, jointToTarget).magnitude;
            }


            Vector3 rotationAxis = Vector3.Cross(jointToEndEffector, jointToTarget).normalized;

            float angleBetweenTargetAndEndEffector = Mathf.Acos(Mathf.Clamp(cos, -1f, 1f));

            //Correct angles if needed, depending on angles invert angle if sin component is negative
            if (sin < 0.0f)
                angleBetweenTargetAndEndEffector = -angleBetweenTargetAndEndEffector;

            if (angleBetweenTargetAndEndEffector > 180.0f)
            {
                angleBetweenTargetAndEndEffector = 180.0f - angleBetweenTargetAndEndEffector;
            }


            angleBetweenTargetAndEndEffector *= Mathf.Rad2Deg;

            return Quaternion.AngleAxis(angleBetweenTargetAndEndEffector, rotationAxis);
        }
        
    }
}