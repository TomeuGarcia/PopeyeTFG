using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class ClawAnchorSnapTargetView
    {
        private readonly Transform _clawsTransform;
        private readonly Transform[] _claws;
        private readonly ClawAnchorSnapTargetViewConfig _viewConfig;

        private bool _isOpen;
        private bool _isPlayingHint;
        
        public ClawAnchorSnapTargetView(Transform clawsTransform, Transform[] claws, 
            ClawAnchorSnapTargetViewConfig viewConfig)
        {
            _clawsTransform = clawsTransform;
            _claws = claws;
            _viewConfig = viewConfig;
            _isOpen = false;
            _isPlayingHint = false;
        }
        
        
        public async UniTaskVoid PlaySnapAnimation(float delay)
        {
            float delay1 = delay * 0.7f;
            float delay2 = delay * 0.2f;

            _isOpen = false;
            //PlayOpenAnimation(delay1);
            await UniTask.Delay(MathUtilities.SecondsToMilliseconds(delay1));

            PlayCloseAnimation();
            await UniTask.Delay(MathUtilities.SecondsToMilliseconds(delay2));
            
            _clawsTransform.PunchScale(_viewConfig.ScalePunchSnapClawParent);
        }

        public void PlayOpenAnimation()
        {
            foreach (var claw in _claws)
            {
                claw.LocalRotate(_viewConfig.RotateOpenClaws, true);
            }

            _isOpen = true;
            if (!_isPlayingHint)
            {
                PlayHintAnimation().Forget();
            }
        }
        
        public void PlayCloseAnimation()
        {
            _clawsTransform.DOComplete();
            foreach (var claw in _claws)
            {
                claw.LocalRotate(_viewConfig.RotateCloseClaws, true);
            }

            _isOpen = false;
        }
        
        private async UniTaskVoid PlayHintAnimation()
        {
            _isPlayingHint = true;
            
            while (_isOpen)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.HintingDelay));
                if (!_isOpen) break;

                _clawsTransform.PunchRotation(_viewConfig.RotatePunchHintClawParent);
                _clawsTransform.PunchPosition(_viewConfig.MovePunchHintClawParent);
                foreach (var claw in _claws)
                {
                    claw.PunchRotation(_viewConfig.RotatePunchHintClaws, true);
                }
                await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.HintingDuration));
            }
            
            _isPlayingHint = false;
        }


        public async UniTaskVoid PlayUsedForDashAnimation()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_viewConfig.UsedForDashDelay));
            
            _clawsTransform.PunchRotation(_viewConfig.RotatePunchUsedClawParent, true);
            _clawsTransform.PunchPosition(_viewConfig.MovePunchUsedClawParent);
            _clawsTransform.PunchScale(_viewConfig.ScalePunchUsedClawParent);
            foreach (var claw in _claws)
            {
                claw.PunchRotation(_viewConfig.RotatePunchUsedClaws, true);
            }
        }
    }
}