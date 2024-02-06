using System;
using DG.Tweening;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.ProjectHelpers;
using UnityEngine;
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


        [Header("STRAIGHT")] 
        [SerializeField, Range(2, 10)] private int _minBonesStraight = 3;
        [SerializeField, Range(2, 10)] private int _maxBonesStraight = 5;
        
        [Header("LOOPING")] 
        [SerializeField, Range(2, 10)] private int _minBonesPerCircle = 6;
        [SerializeField, Range(2, 10)] private int _maxBonesPerCircle = 8;
        [SerializeField, Range(0f, 360f)] private float _fullCircleAngles = 360f;


        [Header("WAVE")] 
        [SerializeField] private AnimationCurve _waveCurve;
        [SerializeField, Range(0f, 5f)] private float _maxWaveAmplitude = 2.0f;
        
        
        public CollisionProbingConfig FloorCollisionProbingConfig => _floorCollisionProbingConfig;
        public float MaxChainLength => _chainConfig.MaxChainLength;
        
        
        public float StateTransitionDuration => _stateTransitionDuration;
        public Ease StateTransitionEase => _stateTransitionEase;


        public int RandomBonesStraight => Random.Range(_minBonesStraight, _maxBonesStraight + 1);
        public int RandomBonesPerCircle => Random.Range(_minBonesPerCircle, _maxBonesPerCircle + 1);
        public float FullCircleAngles => _fullCircleAngles;


        public AnimationCurve WaveCurve => _waveCurve;
        public float MaxWaveAmplitude => _maxWaveAmplitude;
        
        
        private void OnValidate()
        {
            _maxBonesStraight = Mathf.Max(_minBonesStraight, _maxBonesStraight);
            _maxBonesPerCircle = Mathf.Max(_minBonesPerCircle, _maxBonesPerCircle);
        }
    }
}