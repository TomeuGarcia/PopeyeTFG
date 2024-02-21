using UnityEngine;

namespace Project.Scripts.InverseKinematics.CCD
{
    public class CCDJointChain
    {
        private readonly Transform _target;

        public Transform[] Joints { get; private set; }

        public int NumberOfJoints => Joints.Length;
        public Vector3 TargetPosition => _target.position;
        public Vector3 PreviousTargetPosition { get; set; }
        public int MaxIterationTries { get; private set; }
        public int IterationTriesCounter { get; set; }
        
        public Vector3 ClampedAnglesMin { get; private set; }
        public Vector3 ClampedAnglesMax { get; private set; }
        public Vector3 ClampAxis { get; private set; }
    
        

        public CCDJointChain(Transform[] joints, Transform target, 
            Vector3 clampedAnglesMin, Vector3 clampedAnglesMax, Vector3 clampAxis,
            int maxIterationTries = 10)
        {
            _target = target;
            Joints = joints;

            MaxIterationTries = maxIterationTries;

            ClampedAnglesMin = clampedAnglesMin;
            ClampedAnglesMax = clampedAnglesMax;
            ClampAxis = clampAxis;
        }
        
        
        

    }
}