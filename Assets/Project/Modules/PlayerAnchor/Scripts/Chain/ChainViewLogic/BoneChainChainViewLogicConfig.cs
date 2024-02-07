using System;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    [CreateAssetMenu(fileName = "BoneChainChainViewLogicConfig", 
        menuName = ScriptableObjectsHelper.ANCHORCHAIN_ASSETS_PATH + "BoneChainChainViewLogicConfig")]
    public class BoneChainChainViewLogicConfig : ScriptableObject
    {
        [SerializeField] private CollisionProbingConfig _floorCollisionProbingConfig;

        [SerializeField] private ChainConfig _chainConfig;


        [Header("TRANSITION")]
        [SerializeField, Range(0.01f, 2.0f)] private float _stateTransitionDuration = 0.3f;
        [SerializeField] private Ease _stateTransitionEase = Ease.InOutQuart;

        [Header("CURLED vs WAVE")] 
        [SerializeField, Range(0f, 20f)] private float _distanceThresholdToUseWave = 6.0f;
        

        [Header("CURLED")] 
        [SerializeField, Range(2, 10)] private int _minBonesStraight = 3;
        [SerializeField, Range(2, 10)] private int _maxBonesStraight = 5;
        [Space(10)]
        [SerializeField, Range(2, 10)] private int _minBonesPerCircle = 6;
        [SerializeField, Range(2, 10)] private int _maxBonesPerCircle = 8;
        [SerializeField, Range(0f, 360f)] private float _fullCircleAngles = 360f;


        [Header("WAVE")] 
        [SerializeField, Range(2, 10)] private int _minStartBonesStraight = 1;
        [SerializeField, Range(2, 10)] private int _maxStartBonesStraight = 2;
        [SerializeField, Range(1, 15)] private int _bonesPerWave = 8;
        [SerializeField, Range(90f, 180f)] private float _startHalfWaveAngle = 130f;
        [SerializeField, Range(90f, 180f)] private float _minHalfWaveAngle = 100f;
        [SerializeField, Range(0f, 30f)] private float _subtractAnglePerWave = 10f;
        
        
        
        
        public CollisionProbingConfig FloorCollisionProbingConfig => _floorCollisionProbingConfig;
        public float MaxChainLength => _chainConfig.MaxChainLength;
        
        
        public float StateTransitionDuration => _stateTransitionDuration;
        public Ease StateTransitionEase => _stateTransitionEase;


        public float DistanceThresholdToUseWave => _distanceThresholdToUseWave;

        public int RandomBonesStraight => Random.Range(_minBonesStraight, _maxBonesStraight + 1);
        public int RandomBonesPerCircle => Random.Range(_minBonesPerCircle, _maxBonesPerCircle + 1);
        public float FullCircleAngles => _fullCircleAngles;



        public int RandomStartBonesStraight => Random.Range(_minStartBonesStraight, _maxStartBonesStraight + 1);
        public int BonesPerWave => _bonesPerWave;
        public float StartHalfWaveAngle => _startHalfWaveAngle;
        public float MinHalfWaveAngle => _minHalfWaveAngle;
        public float SubtractAnglePerWave => _subtractAnglePerWave;
        
        
        private void OnValidate()
        {
            _maxBonesStraight = Mathf.Max(_minBonesStraight, _maxBonesStraight);
            _maxBonesPerCircle = Mathf.Max(_minBonesPerCircle, _maxBonesPerCircle);
            _maxStartBonesStraight = Mathf.Max(_minStartBonesStraight, _maxStartBonesStraight);
        }
    }
}