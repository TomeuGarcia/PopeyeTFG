using System;
using DG.Tweening;
using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint
{
    [CreateAssetMenu(fileName = "AnchorHitCheckpointView", 
        menuName = ScriptableObjectsHelper.PLAYERCHECKPOINTS_ASSETS_PATH + "AnchorHitCheckpointView")]
    public class AnchorHitCheckpointViewConfig : ScriptableObject
    {

        [System.Serializable]
        public class BouncesViewConfig
        {
            [Header("BELL")]
            [SerializeField] private Ease _startEase = Ease.OutQuint;
            [SerializeField] private Ease _middleEase = Ease.InOutQuint;
            [SerializeField] private Ease _endEase = Ease.InQuint;
            [SerializeField] private AnimationCurve _anglesCurve = AnimationCurve.Linear(1,100,0,0);
            [SerializeField] private AnimationCurve _durationCurve = AnimationCurve.Linear(1,1,0,0);
            [SerializeField, Range(2, 20)] private int _numberOfBounces = 7;
            
            [Header("CLAPPER")]
            [SerializeField, Range(-2.0f, 2.0f)] private float _clapperAngleMultiplier = 0.2f;
            [SerializeField, Range(0.0f, 2.0f)] private float _clapperDurationMultiplier = 0.3f;
            
            public Ease StartEase => _startEase;
            public Ease MiddleEase => _middleEase;
            public Ease EndEase => _endEase;
            public AnimationCurve AnglesCurve => _anglesCurve;
            public AnimationCurve DurationCurve => _durationCurve;
            public int NumberOfBounces => _numberOfBounces;
            public float ClapperAngleMultiplier => _clapperAngleMultiplier;
            public float ClapperDurationMultiplier => _clapperDurationMultiplier;

            public void ApplyCorrections()
            {
                _numberOfBounces = Mathf.Max(2, _numberOfBounces);
            }
        }


        [System.Serializable]
        public class VFXViewConfig
        {
            [SerializeField, Range(0.01f, 10.0f)] private float _firstTimeUsedDuration = 3.0f;
            [SerializeField] private string _animationTProperty = "_AnimationT";
            [SerializeField] private Color _lockedColor = Color.green;
            [SerializeField] private Color _unlockedColor = Color.yellow;
            
            public float FirstTimeUsedDuration => _firstTimeUsedDuration;
            public int AnimationTPropertyID { get; private set; }
            public Color LockedColor => _lockedColor;
            public Color UnlockedColor => _unlockedColor;

            public void PrepareForUser()
            {
                AnimationTPropertyID = Shader.PropertyToID(_animationTProperty);
            } 
        }

        [System.Serializable]
        public class AudioConfig
        {
            [SerializeField] private AFMODAudioManagerReference _audioManager;
            [SerializeField] private OneShotFMODSound _checkpointSetSound;
            [SerializeField] private OneShotFMODSound _hitSound;

            public void PlayCheckpointSetSound()
            {
                _audioManager.PlayOneShot(_checkpointSetSound);
            }
            public void PlayHitSound(GameObject source)
            {
                _audioManager.PlayOneShotAttached(_hitSound, source);
            }
        }


        [SerializeField] private BouncesViewConfig _bouncesView;
        [SerializeField] private VFXViewConfig _vfxView;
        [SerializeField] private AudioConfig _audio;
        public BouncesViewConfig BouncesView => _bouncesView;
        public VFXViewConfig VFXView => _vfxView;
        public AudioConfig Audio => _audio;



        private void OnValidate()
        {
            _bouncesView.ApplyCorrections();
        }

        private void OnEnable()
        {
            OnValidate();
            _vfxView.PrepareForUser();
        }
    }
}