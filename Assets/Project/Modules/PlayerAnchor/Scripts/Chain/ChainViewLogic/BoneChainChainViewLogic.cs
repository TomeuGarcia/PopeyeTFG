using System;
using Popeye.InverseKinematics.Bones;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainChainViewLogic : IChainViewLogic
    {
        private readonly int _chainBoneCount;
        private readonly int _chainBoneCountMinusOne;
        
        private float MaxChainLength => _config.MaxChainLength;
        
        private readonly Vector3[] _chainPositions;
        
        private readonly Transform _chainIK;
        private readonly BoneChain _boneChain;
        private readonly BoneChainChainViewLogicConfig _config;


        public BoneChainChainViewLogic(BoneChainChainViewLogicConfig config, int chainBoneCount, Transform chainIK, BoneChain boneChain)
        {
            _chainBoneCount = chainBoneCount;
            _chainBoneCountMinusOne = _chainBoneCount - 1;
            
            _chainIK = chainIK;
            _boneChain = boneChain;
            _config = config;

            _chainPositions = new Vector3[_chainBoneCount];

            _chainIK.gameObject.SetActive(false);
        }


        public void EnterSetup(Vector3[] previousStateChainPositions, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            Array.Reverse(previousStateChainPositions);
            MakeLoopingChain(playerBindPosition, anchorBindPosition);
        }
        
        public void OnViewEnter()
        {
            _chainIK.gameObject.SetActive(true);
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
            _chainIK.gameObject.SetActive(false);
        }

        public Vector3[] GetChainPositions()
        {
            return _chainPositions;
        }


        private void MakeChainRestOnFloor()
        {
            for (int i = 0; i < _chainBoneCountMinusOne; ++i)
            {
                Vector3 oldDirection = (_boneChain.Bones[i + 1].Position - _boneChain.Bones[i].Position).normalized;
                Vector3 newDirection = Vector3.zero;
                
                
                Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
                float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;
                
                if (angle > 1.0f)
                {
                    _boneChain.Bones[i].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * _boneChain.Bones[i].Rotation);
                }
            }
        }
        
        private void MakeLoopingChain(Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            Vector3 anchorToPlayer = playerBindPosition - anchorBindPosition;
            float anchorToPlayerDistance = anchorToPlayer.magnitude;
            Vector3 anchorToPlayerDirection = anchorToPlayer / anchorToPlayerDistance;

            float distanceT = Mathf.Min(anchorToPlayerDistance / MaxChainLength, 1.0f);

            float zigZagAngle = Mathf.Acos(distanceT) * Mathf.Rad2Deg;
            
            float totalAngles = zigZagAngle * _chainBoneCountMinusOne;
            float fullLoopAngles = 360f;
            float halfLoopAngles = fullLoopAngles / 2;

            int bonesPerCircle = Random.Range(6, 9);
            int numBonesStraightBones = Random.Range(3, 6);


            int i = 0;

            while (totalAngles > fullLoopAngles && (i + numBonesStraightBones) < _chainBoneCountMinusOne)
            {
                int lastStraightBoneIndex = i + numBonesStraightBones;
                for (; i < lastStraightBoneIndex; ++i)
                {
                    SetStraightDirection(i, anchorToPlayerDirection);
                }
                totalAngles += zigZagAngle * numBonesStraightBones;

                
                if (totalAngles > fullLoopAngles && (i + 2) < _chainBoneCountMinusOne)
                {
                    ApplyEllipseToBoneChain(ref i, bonesPerCircle, halfLoopAngles, anchorToPlayerDirection);
                    totalAngles -= fullLoopAngles;
                }

                numBonesStraightBones -= 2;
                numBonesStraightBones = Mathf.Max(1, numBonesStraightBones);
            }

            for (; i < _chainBoneCountMinusOne; ++i)
            {
                SetStraightDirection(i, anchorToPlayerDirection);
            }
        }

        private void SetStraightDirection(int i, Vector3 anchorToPlayerDirection)
        {
            Vector3 oldDirection = (_boneChain.Bones[i + 1].Position - _boneChain.Bones[i].Position).normalized;
            Vector3 newDirection = anchorToPlayerDirection;
                
            Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
            float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;
                
            if (angle > 1.0f)
            {
                _boneChain.Bones[i].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * _boneChain.Bones[i].Rotation);
            }
        }

        private void ApplyEllipseToBoneChain(ref int i, int bonesPerCircle, float halfLoopAngles, Vector3 anchorToPlayerDirection)
        {
            int lastLoopingBoneIndex = Mathf.Min(i + bonesPerCircle, _chainBoneCountMinusOne);
            int correctedBonesPerCircle = lastLoopingBoneIndex - i;

            int circleDirection = (Random.Range(0, 2) < 1 ? 1 : -1);
            int firstHalfBonesPerCircle = correctedBonesPerCircle / 2;
            float angleStepCircle = halfLoopAngles / firstHalfBonesPerCircle;
            Quaternion circleStepRotation = Quaternion.AngleAxis(angleStepCircle * circleDirection, Vector3.up);
            Vector3 accumulatedCircleDirection = anchorToPlayerDirection;
            
            lastLoopingBoneIndex = i + firstHalfBonesPerCircle;
            for (; i < lastLoopingBoneIndex; ++i)
            {
                SetCircleDirection(i, ref accumulatedCircleDirection, circleStepRotation);
            }
            
            
            int secondHalfBonesPerCircle = correctedBonesPerCircle - firstHalfBonesPerCircle;
            angleStepCircle = halfLoopAngles / secondHalfBonesPerCircle;
            circleStepRotation = Quaternion.AngleAxis(angleStepCircle * circleDirection, Vector3.up);
            
            lastLoopingBoneIndex = i + secondHalfBonesPerCircle;
            for (; i < lastLoopingBoneIndex; ++i)
            {
                SetCircleDirection(i, ref accumulatedCircleDirection, circleStepRotation);
            }
        }

        private void SetCircleDirection(int i, ref Vector3 accumulatedCircleDirection, Quaternion circleStepRotation)
        {
            Vector3 oldDirection = (_boneChain.Bones[i + 1].Position - _boneChain.Bones[i].Position).normalized;
                
            accumulatedCircleDirection = circleStepRotation * accumulatedCircleDirection;
            Vector3 newDirection = accumulatedCircleDirection;

                
            Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
            float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;

            if (angle > 1.0f)
            {
                _boneChain.Bones[i]
                    .SetWorldRotation(Quaternion.AngleAxis(angle, axis) * _boneChain.Bones[i].Rotation);
            }
        }
     
        
        
        private void MakeStraightChain(Vector3[] previousStateChainPositions)
        {
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
        }

        private void MakeZigZagChain(Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            Vector3 anchorToPlayer = playerBindPosition - anchorBindPosition;
            float anchorToPlayerDistance = anchorToPlayer.magnitude;
            Vector3 anchorToPlayerDirection = anchorToPlayer / anchorToPlayerDistance;
            
            float distanceT = Mathf.Min(anchorToPlayerDistance / MaxChainLength, 1.0f);

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
            }
        }
        
        private void SetZigZagDirection(int i, Vector3 anchorToPlayerDirection, Quaternion zigZagLeftRotation, Quaternion zigZagRightRotation)
        {
            Vector3 oldDirection = (_boneChain.Bones[i + 1].Position - _boneChain.Bones[i].Position).normalized;
            Vector3 newDirection = (i % 2 == 0 ? zigZagLeftRotation : zigZagRightRotation) * anchorToPlayerDirection;
                
            Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
            float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;
                
            if (angle > 1.0f)
            {
                _boneChain.Bones[i].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * _boneChain.Bones[i].Rotation);
            }
            
            Debug.Log(newDirection);
        }

        
    }
}