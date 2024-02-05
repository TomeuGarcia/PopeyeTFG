using System;
using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainChainViewLogic : IChainViewLogic
    {
        private readonly int _chainBoneCount;
        private readonly int _chainBoneCountMinusOne;
        
        private readonly float _maxChainDistance;
        
        private readonly Vector3[] _chainPositions;
        
        private readonly Transform _chainIK;
        private readonly BoneChain _boneChain;
        


        public BoneChainChainViewLogic(int chainBoneCount, Transform chainIK, BoneChain boneChain)
        {
            _chainBoneCount = chainBoneCount;
            _chainBoneCountMinusOne = _chainBoneCount - 1;

            _maxChainDistance = 11f;
            
            _chainIK = chainIK;
            _boneChain = boneChain;
            
            _chainPositions = new Vector3[_chainBoneCount];

            //_chainIK.gameObject.SetActive(false);
        }


        public void EnterSetup(Vector3[] previousStateChainPositions, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            Array.Reverse(previousStateChainPositions);
/*
            playerBindPosition += Vector3.ProjectOnPlane((playerBindPosition - anchorBindPosition).normalized, Vector3.up) * -0.4f;
            

            for (int i = 0; i < _chainBoneCountMinusOne; ++i)
            {
                Vector3 oldDirection = (_boneChain.Bones[i + 1].Position - _boneChain.Bones[i].Position).normalized;
                Vector3 newDirection = (previousStateChainPositions[i + 1] - previousStateChainPositions[i]).normalized;
                
                Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
                float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;
                
                if (angle > 1.0f)
                {
                    _boneChain.Bones[i].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * _boneChain.Bones[i].Rotation);
                }
            }
          */  

            Vector3 anchorToPlayer = playerBindPosition - anchorBindPosition;
            float anchorToPlayerDistance = anchorToPlayer.magnitude;
            Vector3 anchorToPlayerDirection = anchorToPlayer / anchorToPlayerDistance;
            
            float distanceT = Mathf.Min(anchorToPlayerDistance / _maxChainDistance, 1.0f);

            float zigZagAngle = Mathf.Acos(distanceT) * Mathf.Rad2Deg;
            
            Quaternion zigZagLeftRotation = Quaternion.AngleAxis(zigZagAngle, Vector3.up);
            Quaternion zigZagRightRotation = Quaternion.Inverse(zigZagLeftRotation);
            
            
            
            for (int i = 0; i < _chainBoneCountMinusOne; ++i)
            {
                Vector3 oldDirection = (_boneChain.Bones[i + 1].Position - _boneChain.Bones[i].Position).normalized;
                Vector3 newDirection = (i % 2 == 0 ? zigZagLeftRotation : zigZagRightRotation) * anchorToPlayerDirection;
                
                Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
                float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;
                
                if (angle > 1.0f)
                {
                    _boneChain.Bones[i].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * _boneChain.Bones[i].Rotation);
                }
                
                Debug.Log(_boneChain.Bones[i].Position);
            }
            
            

        }
        
        public void OnViewEnter()
        {
            //_chainIK.gameObject.SetActive(false);
        }

        public void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            _chainPositions[0] = anchorBindPosition;
            
            for (int i = 1; i < _chainBoneCount; ++i)
            {
                _chainPositions[i] = _boneChain.Bones[i].Position;
            }
        }

        public void OnViewExit()
        {
            //_chainIK.gameObject.SetActive(false);
        }

        public Vector3[] GetChainPositions()
        {
            return _chainPositions;
        }
    }
}