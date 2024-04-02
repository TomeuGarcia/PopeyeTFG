using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.ParticleFactories;
using Project.Scripts.Time.TimeHitStop;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class StretchAnchorView : IAnchorView
    {
        private readonly StretchAnchorViewConfig _viewConfig;
        private readonly Transform _meshTransform;


        public StretchAnchorView(StretchAnchorViewConfig viewConfig, Transform meshTransform)
        {
            _viewConfig = viewConfig;
            _meshTransform = meshTransform;
        }

        public void ResetView()
        {

        }
        

        public async UniTaskVoid PlayVerticalHitAnimation(float duration, RaycastHit floorHit)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration * 0.5f));
            _meshTransform.PunchScale(_viewConfig.ScaleVerticalHitPunchConfig, true);
        }

        public async UniTaskVoid PlayThrownAnimation(float duration)
        {
            _meshTransform.PunchScale(_viewConfig.ScaleThrowPunchConfig, true);
        }

        public async UniTaskVoid PlayPulledAnimation(float duration)
        {
            _meshTransform.PunchScale(_viewConfig.ScalePullPunchConfig, true);
        }

        public void PlayKickedAnimation(float duration)
        {

        }

        public void PlayCarriedAnimation()
        {
            _meshTransform.PunchScale(_viewConfig.ScaleCarriedPunchConfig, true);
        }

        public void PlayRestOnFloorAnimation()
        {
            
        }

        public void PlaySpinningAnimation()
        {

        }

        public void PlayObstructedAnimation()
        {

        }

        public void StopCarry()
        {
            
        }

        public void OnDamageDealt(DamageHitResult damageHitResult)
        {

        }


        
    }
}