using System;
using System.Linq;
using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainChainView : IChainView
    {
        private readonly BoneChain _boneChain;
        private readonly float _chainDistance;
        private Vector3[] _positions;
        
        public BoneChainChainView(BoneChain boneChain, int numberOfBones, float chainDistance, float boneLength)
        {
            _boneChain = boneChain;
            _chainDistance = chainDistance;
            _boneChain.AwakeConfigure(numberOfBones, true, boneLength);
            _boneChain.StartInit();
        }
        
        public void Update(Vector3[] positions)
        {
            _positions = positions;

            /*
            float positionsDistance = 0f;
            for (int i = 1; i < _positions.Length; ++i)
            {
                positionsDistance += Vector3.Distance(_positions[i - 1], _positions[i]);
            }

            float scaler = _chainDistance / positionsDistance;
            */

            
            int numberOfBonesMinusOne = _boneChain.NumberOfBones - 1;
            for (int i = 0; i < numberOfBonesMinusOne; i++)
            {
                Vector3 oldDirection = (_boneChain.Bones[i + 1].Position - _boneChain.Bones[i].Position).normalized;
                Vector3 newDirection = (positions[i + 1] - positions[i]).normalized;

                Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
                float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;

                if (angle > 1.0f)
                {
                    _boneChain.Bones[i].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * _boneChain.Bones[i].Rotation);
                }
            }
        }

        public void DrawGizmos()
        {
            for (int i = 0; i < _positions.Length-1; ++i)
            {
                Gizmos.color = Color.Lerp(Color.green, Color.red, i / (float)(_boneChain.NumberOfBones - 1));
                Gizmos.DrawLine(_positions[i], _positions[i+1]);
            }
        }
    }
}