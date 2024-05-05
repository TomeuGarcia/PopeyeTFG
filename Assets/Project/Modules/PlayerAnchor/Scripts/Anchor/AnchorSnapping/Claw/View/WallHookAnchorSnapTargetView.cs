using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class WallHookAnchorSnapTargetView : MonoBehaviour, IClawAnchorSnapTargetView
    {
        [Header("ANIMATION")]
        [SerializeField] private HookAnimatorViewConfig _animatorConfig;
        [SerializeField] private Animator _animator;

        [SerializeField] private Transform _punchingTransform;
        
        [Header("AUDIO")] 
        [SerializeField] private HookAnchorSnapTargetAudio _audio;
        
        private void Awake()
        {
            _animator.SetBool(_animatorConfig.AimedParameter, false);
            _animator.SetBool(_animatorConfig.GrabbedParameter, false);
            _animator.ResetTrigger(_animatorConfig.PulledParameter);
        }
        
        public void PlayAimedAnimation()
        {
            _animator.SetBool(_animatorConfig.AimedParameter, true);
        }

        public void StopAimedAnimation()
        {
            _animator.SetBool(_animatorConfig.AimedParameter, false);
        }

        public void PlayGrabAnimation(float delay)
        {
            _animator.SetBool(_animatorConfig.GrabbedParameter, true);
            
            _animator.SetBool(_animatorConfig.AimedParameter, false);
            _animator.ResetTrigger(_animatorConfig.PulledParameter);

            DoPlayGrabAnimation(delay).Forget();
        }

        private async UniTaskVoid DoPlayGrabAnimation(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            _punchingTransform.PunchScale(_animatorConfig.StartGrabbingScalePunch, true);
            _audio.PlayGrabSound(gameObject);
        }
        

        public void PlayUsedAnimation()
        {
            _animator.SetTrigger(_animatorConfig.PulledParameter);
            
            _animator.SetBool(_animatorConfig.GrabbedParameter, false);
            _audio.PlayJumpSound(gameObject);
        }

        public void PlayPulledAnimation()
        {
            _animator.SetBool(_animatorConfig.GrabbedParameter, false);
            
            _punchingTransform.PunchScale(_animatorConfig.StopGrabbingScalePunch, true);
            _audio.PlayReleaseSound(gameObject);
        }
    }
}