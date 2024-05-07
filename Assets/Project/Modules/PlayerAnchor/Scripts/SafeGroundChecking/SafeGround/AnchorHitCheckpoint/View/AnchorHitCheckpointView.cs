using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;

using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint
{
    public class AnchorHitCheckpointView : MonoBehaviour
    {
        [Header("CONFIG")]
        [Expandable] [SerializeField] private AnchorHitCheckpointViewConfig _viewConfig;
        
        [Header("BOUNCE ANIMATION")]
        [SerializeField] private AnchorHitCheckpointBounceView _bounceView;
        [SerializeField] private AnchorHitCheckpointVFXView _vfxView;
        
        public void Configure(bool startAsActiveCheckpoint, bool hasBeenUnlocked)
        {
            _bounceView.Configure(_viewConfig.BouncesView);
            _vfxView.Configure(_viewConfig.VFXView, startAsActiveCheckpoint, hasBeenUnlocked);

            if (startAsActiveCheckpoint)
            {
                PlayStartBeingCurrentlyActiveCheckpoint().Forget();
            }
            else
            {
                PlayStopBeingCurrentlyActiveCheckpoint();
            }
        }

        public void ComputeBounceAxis(Vector3 hitOrigin)
        {
            _bounceView.ComputeBounceAxis(hitOrigin);
        }
        
        [Button()]
        public void PlayBounceAnimation()
        {
            _bounceView.PlayBounceAnimation();
            _viewConfig.Audio.PlayHitSound(gameObject);
        }
        
        [Button()]
        public void PlayFirstTimeUsedAnimation()
        {
            _vfxView.PlayFirstTimeUsedAnimation();
        }
        
        [Button()]
        public void PlayUsedAnimation()
        {
            _vfxView.PlayUsedAnimation();
        }

        public async UniTaskVoid PlayStartBeingCurrentlyActiveCheckpoint()
        {
            _viewConfig.Audio.PlayCheckpointSetSound();            
            _vfxView.SetCurrentCheckpointView();
        }
        public void PlayStopBeingCurrentlyActiveCheckpoint()
        {
            _vfxView.StopCurrentCheckpointView();
        }
    }
}