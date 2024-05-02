using System;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;

using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint
{
    public class AnchorHitCheckpointView : MonoBehaviour
    {
        [Header("CONFIG")]
        [SerializeField] private AnchorHitCheckpointViewConfig _viewConfig;
        
        [Header("BOUNCE ANIMATION")]
        [SerializeField] private AnchorHitCheckpointBounceView _bounceView;
        [SerializeField] private AnchorHitCheckpointVFXView _vfxView;

        [Header("TEMPORARILY")] 
        [SerializeField] private GameObject _currentActiveCheckpointView;

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
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _currentActiveCheckpointView.SetActive(true);
        }
        public void PlayStopBeingCurrentlyActiveCheckpoint()
        {
            _currentActiveCheckpointView.SetActive(false);
        }
    }
}