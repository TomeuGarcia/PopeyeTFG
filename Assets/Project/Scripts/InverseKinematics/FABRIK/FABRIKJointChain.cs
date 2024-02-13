using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Popeye.InverseKinematics.FABRIK
{
    public class FABRIKJointChain
    {
        private readonly Transform _target;

        public Transform[] Joints { get; private set; }
        public Vector3[] PositionCopies { get; private set; }
        public float[] Distances { get; private set; }
        public float DistancesSum { get; private set; }

        public int NumberOfJoints => Joints.Length;
        public Vector3 TargetPosition => _target.position;
        public Vector3 RootToTarget => (_target.position - Joints[0].position).normalized;



        public FABRIKJointChain(Transform[] joints, Transform target)
        {
            _target = target;

            Joints = joints;
            PositionCopies = new Vector3[joints.Length];

            Distances = new float[joints.Length - 1];
            for (int i = 0; i < Distances.Length; ++i)
            {
                Distances[i] = Vector3.Distance(joints[i].position, joints[i + 1].position);
            }

            DistancesSum = Distances.Sum();
        }


        private float RootCopyToTargetDistance(Vector3 targetPosition)
        {
            return Vector3.Distance(PositionCopies[0], targetPosition);
        }

        public float EndEffectorCopyToTargetDistance(Vector3 targetPosition)
        {
            return Vector3.Distance(PositionCopies[NumberOfJoints - 1], targetPosition);
        }
        
        public bool IsTargetUnreachable()
        {
            return RootCopyToTargetDistance(TargetPosition) > DistancesSum;
        }


    }
}
