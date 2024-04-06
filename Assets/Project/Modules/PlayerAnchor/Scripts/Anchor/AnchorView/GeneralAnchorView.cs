using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.ParticleFactories;
using Project.Scripts.Time.TimeHitStop;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class GeneralAnchorView : IAnchorView
    {
        private readonly IAnchorView[] _subViews;

        public GeneralAnchorView(IAnchorView[] subViews)
        {
            _subViews = subViews;
        }
        
        public void Configure(IParticleFactory particleFactory, IHitStopManager hitStopManager, ICameraShaker cameraShaker)
        {
            throw new System.NotImplementedException();
        }

        public void ResetView()
        {
            foreach (IAnchorView subView in _subViews)
            {
                subView.ResetView();
            }
        }

        public async UniTaskVoid PlayVerticalHitAnimation(float duration, RaycastHit floorHit)
        {
            foreach (IAnchorView subView in _subViews)
            {
                subView.PlayVerticalHitAnimation(duration, floorHit);
            }
        }

        public async UniTaskVoid PlayThrownAnimation(float duration)
        {
            foreach (IAnchorView subView in _subViews)
            {
                subView.PlayThrownAnimation(duration);
            }
        }

        public async UniTaskVoid PlayPulledAnimation(float duration)
        {
            foreach (IAnchorView subView in _subViews)
            {
                subView.PlayPulledAnimation(duration).Forget();
            }
        }

        public void PlayKickedAnimation(float duration)
        {
            foreach (IAnchorView subView in _subViews)
            {
                subView.PlayKickedAnimation(duration);
            }
        }

        public void PlayCarriedAnimation()
        {
            foreach (IAnchorView subView in _subViews)
            {
                subView.PlayCarriedAnimation();
            }
        }

        public void PlayRestOnFloorAnimation()
        {
            foreach (IAnchorView subView in _subViews)
            {
                subView.PlayRestOnFloorAnimation();
            }
        }

        public void PlaySpinningAnimation()
        {
            foreach (IAnchorView subView in _subViews)
            {
                subView.PlaySpinningAnimation();
            }
        }

        public void PlayObstructedAnimation()
        {
            foreach (IAnchorView subView in _subViews)
            {
                subView.PlayObstructedAnimation();
            }
        }

        public void StopCarry()
        {
            foreach (IAnchorView subView in _subViews)
            {
                subView.StopCarry();
            }
        }

        public void OnDamageDealt(DamageHitResult damageHitResult)
        {
            foreach (IAnchorView subView in _subViews)
            {
                subView.OnDamageDealt(damageHitResult);
            }
        }
    }
}