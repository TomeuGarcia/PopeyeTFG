using DG.Tweening;
using Popeye.InverseKinematics.Bones;
using Popeye.InverseKinematics.FABRIK;
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
        
        private readonly BoneChain _boneChainIK;
        private readonly FABRIKControllerBehaviour _controllerIK;
        private readonly BoneChainChainViewLogicConfig _config;

        private Vector3[] _previousStateChainPositions;
        private float _transitionT;

        private float FloorProbeDistance => _config.FloorCollisionProbingConfig.ProbeDistance;
        private LayerMask FloorLayerMask => _config.FloorCollisionProbingConfig.CollisionLayerMask;
        private QueryTriggerInteraction FloorQueryTriggerInteraction => _config.FloorCollisionProbingConfig.QueryTriggerInteraction;


        private float TransitionDuration => _config.StateTransitionDuration;
        private float DistanceThresholdToUseWave => _config.DistanceThresholdToUseWave;
        private Ease TransitionEase => _config.StateTransitionEase;
        private int RandomBonesPerCircle => _config.RandomBonesPerCircle;
        private int RandomBonesStraight => _config.RandomBonesStraight;
        private float FullCircleAngles => _config.FullCircleAngles;
        
        
        private int RandomStartBonesStraight => _config.RandomStartBonesStraight;
        private int BonesPerWave => _config.BonesPerWave;
        private float StartHalfWaveAngle => _config.StartHalfWaveAngle;
        private float MinHalfWaveAngle => _config.MinHalfWaveAngle;
        private float SubtractAnglePerWave => _config.SubtractAnglePerWave;


        public BoneChainChainViewLogic(BoneChainChainViewLogicConfig config, int chainBoneCount,
            BoneChain boneChainIK, FABRIKControllerBehaviour controllerIK)
        {
            _chainBoneCount = chainBoneCount;
            _chainBoneCountMinusOne = _chainBoneCount - 1;
            
            _config = config;
            _boneChainIK = boneChainIK;
            _controllerIK = controllerIK;

            _chainPositions = new Vector3[_chainBoneCount];

            _controllerIK.enabled = false;
            
            //_boneChainIK.Bones[0].gameObject.SetActive(false);
        }


        public void EnterSetup(Vector3[] previousStateChainPositions, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            _previousStateChainPositions = previousStateChainPositions;

            if (Vector3.Distance(playerBindPosition, anchorBindPosition) > DistanceThresholdToUseWave)
            {
                MakeWaveChain(playerBindPosition, anchorBindPosition);
            }
            else
            {
                MakeLoopingChain(playerBindPosition, anchorBindPosition);
            }
            MakeChainRestOnFloor();
        }
        
        public void OnViewEnter()
        {
            //_chainIK.gameObject.SetActive(true);
            _controllerIK.enabled = true;

            _transitionT = 0f;
            DOTween.To(
                () => _transitionT,
                (value) => _transitionT = value,
                1.0f,
                TransitionDuration)
                .SetEase(TransitionEase);
        }

        public void UpdateChainPositions(float deltaTime, Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            for (int i = 0; i < _chainBoneCountMinusOne; ++i)
            {
                _chainPositions[i] = Vector3.LerpUnclamped(_previousStateChainPositions[i],_boneChainIK.Bones[i].Position, _transitionT);
            }
            _chainPositions[_chainBoneCountMinusOne] = playerBindPosition;
        }

        public void OnViewExit()
        {
            //_boneChainIK.Bones[0].gameObject.SetActive(false);
            _controllerIK.enabled = false;
        }

        public Vector3[] GetChainPositions()
        {
            return _chainPositions;
        }


        private void MakeChainRestOnFloor()
        {
            for (int i = 0; i < _chainBoneCountMinusOne; ++i)
            {
                Vector3 boneStartPosition = _boneChainIK.Bones[i].Position;
                Vector3 nextBonePosition = _boneChainIK.Bones[i + 1].Position;
                Vector3 origin = nextBonePosition + (Vector3.up * 1.0f);

                if (!Physics.Raycast(origin, Vector3.down, out RaycastHit floorHit, 
                        FloorProbeDistance, FloorLayerMask, FloorQueryTriggerInteraction))
                {
                    continue;
                }

                Vector3 oldDirection = (nextBonePosition - boneStartPosition).normalized;
                Vector3 newDirection = (floorHit.point + floorHit.normal * 0.1f - boneStartPosition).normalized;


                Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
                float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;
                
                if (angle > 1.0f)
                {
                    _boneChainIK.Bones[i].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * _boneChainIK.Bones[i].Rotation);
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
            float fullLoopAngles = FullCircleAngles;
            float halfLoopAngles = fullLoopAngles / 2;

            int bonesPerCircle = RandomBonesPerCircle;
            int numBonesStraightBones = RandomBonesStraight;


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
            Vector3 oldDirection = (_boneChainIK.Bones[i + 1].Position - _boneChainIK.Bones[i].Position).normalized;
            Vector3 newDirection = anchorToPlayerDirection;
                
            Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
            float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;
                
            if (angle > 1.0f)
            {
                _boneChainIK.Bones[i].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * _boneChainIK.Bones[i].Rotation);
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
            Vector3 oldDirection = (_boneChainIK.Bones[i + 1].Position - _boneChainIK.Bones[i].Position).normalized;
                
            accumulatedCircleDirection = circleStepRotation * accumulatedCircleDirection;
            Vector3 newDirection = accumulatedCircleDirection;

                
            Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
            float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;

            if (angle > 1.0f)
            {
                _boneChainIK.Bones[i]
                    .SetWorldRotation(Quaternion.AngleAxis(angle, axis) * _boneChainIK.Bones[i].Rotation);
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
                SetZigZagDirection(i, anchorToPlayerDirection, zigZagLeftRotation, zigZagRightRotation);
            }
        }
        
        private void SetZigZagDirection(int i, Vector3 anchorToPlayerDirection, Quaternion zigZagLeftRotation, Quaternion zigZagRightRotation)
        {
            Vector3 oldDirection = (_boneChainIK.Bones[i + 1].Position - _boneChainIK.Bones[i].Position).normalized;
            Vector3 newDirection = (i % 2 == 0 ? zigZagLeftRotation : zigZagRightRotation) * anchorToPlayerDirection;
                
            Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
            float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;
                
            if (angle > 1.0f)
            {
                _boneChainIK.Bones[i].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * _boneChainIK.Bones[i].Rotation);
            }
        }

        

        private void MakeWaveChain(Vector3 playerBindPosition, Vector3 anchorBindPosition)
        {
            Vector3 anchorToPlayer = playerBindPosition - anchorBindPosition;
            float anchorToPlayerDistance = anchorToPlayer.magnitude;
            Vector3 anchorToPlayerDirection = anchorToPlayer / anchorToPlayerDistance;
            

            int i = 0;
            int lastIndex = RandomStartBonesStraight;
            for (; i < 1; ++i)
            {
                SetStraightDirection(i, anchorToPlayerDirection);
            }

            int bonesPerHalfWave = 3;
            float halfCircleAngle = StartHalfWaveAngle;

            
            int bonesPerWave = BonesPerWave;
            
            Vector3 tangent = Vector3.Cross(anchorToPlayer, Vector3.up).normalized;
            
            int waveDirection = Random.Range(0,2) < 1 ? 1 : -1;
            
            while (i + bonesPerWave < _chainBoneCount)
            {
                int bonesPerHalfCircle = bonesPerWave / 2;
                
                
                float angleStep = halfCircleAngle / (bonesPerHalfCircle + 1);
                Quaternion stepRotation = Quaternion.AngleAxis(angleStep * waveDirection, Vector3.up);
                Vector3 accumulatedCircleDirection = tangent * waveDirection;
                
                
                // left
                lastIndex = i + bonesPerHalfCircle;
                for (; i < lastIndex; ++i)
                {
                    SetCircleDirection(i, ref accumulatedCircleDirection, stepRotation);
                }
                
                
                halfCircleAngle = Mathf.Max(MinHalfWaveAngle, halfCircleAngle - SubtractAnglePerWave);
                angleStep = halfCircleAngle / (bonesPerHalfCircle + 1);
                Quaternion stepRotationInverse = Quaternion.AngleAxis(-angleStep * waveDirection, Vector3.up);
                accumulatedCircleDirection = -tangent * waveDirection;
                
                
                //right
                lastIndex = i + bonesPerHalfCircle;
                for (; i < lastIndex; ++i)
                {
                    SetCircleDirection(i, ref accumulatedCircleDirection, stepRotationInverse);
                }
                
                halfCircleAngle = Mathf.Max(MinHalfWaveAngle, halfCircleAngle - SubtractAnglePerWave);
            }
            
            for (; i < _chainBoneCountMinusOne; ++i)
            {
                SetStraightDirection(i, anchorToPlayerDirection);
            }
        }

        
    }
}