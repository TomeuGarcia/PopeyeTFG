using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.PlayerAnchor.DropShadow;
using Popeye.Modules.VFX.ParticleFactories;
using Project.Scripts.Time.TimeHitStop;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class StretchAnchorView : MonoBehaviour, IAnchorView
    {
        [SerializeField] private Transform _meshTransform;
        [SerializeField] private Transform _specialMotionsTransform;
        
        [SerializeField] private DropShadow.DropShadowBehaviour _dropShadow;

        [Header("VERTICAL HIT")]
        [SerializeField] private Vector3 _verticalHitScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        [SerializeField, Min(0.01f)] private float _verticalTwistDelay = 0.15f;
        [SerializeField, Min(0.01f)] private float _verticalTwistDuration = 1.0f;
        [SerializeField, Range(1, 10)] private int _verticalTwistLoops = 6;
        
        [Header("THROW")]
        [SerializeField] private Vector3 _throwScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        
        [Header("PULL")]
        [SerializeField] private Vector3 _pullScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        [SerializeField, Range(0f, 5f)] private float _pulledDelay;
 
        [Header("KICK")]
        [SerializeField] private Vector3 _kickScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        
        [Header("CARRIED")]
        [SerializeField] private Vector3 _carriedScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        
        [Header("RESTING ON FLOOR")]
        [SerializeField] private Vector3 _restingOnFloorScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        
        [Header("OBSTRUCTED")]
        [SerializeField] private Vector3 _obstructedRotationPunch = new Vector3(0, 70, 30);


        [SerializeField] private MeshRenderer _landHitMesh;
        private Material _landHitMaterial;

        private void Awake()
        {
            _landHitMaterial = _landHitMesh.material;
            _landHitMesh.gameObject.SetActive(false);
            
            _dropShadow.Hide();
        }


        public async UniTaskVoid PlayVerticalHitAnimation(float duration, RaycastHit floorHit)
        {
            _dropShadow.Show();
            
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_verticalHitScalePunch, duration, 1)
                .SetEase(Ease.OutSine);
            PlayTwistLoopAnimation(_verticalTwistDelay, _verticalTwistLoops, _verticalTwistDuration).Forget();

            duration += 0.2f;
            float delayBeforeHit = duration * 0.7f;
            float delayAfterHit = duration - delayBeforeHit;
            
            
            await UniTask.Delay(TimeSpan.FromSeconds(delayBeforeHit));
            
            _landHitMesh.gameObject.SetActive(true);
            _landHitMesh.transform.up = floorHit.normal;
            _landHitMesh.transform.position = floorHit.point + floorHit.normal * 0.01f;
            _landHitMaterial.SetFloat("_StartTime", Time.time);
            _landHitMaterial.SetFloat("_WaveDuration", delayAfterHit*5);
            
            await UniTask.Delay(TimeSpan.FromSeconds(delayAfterHit));
            _landHitMesh.gameObject.SetActive(false);
            

        }

        public void Configure(IParticleFactory particleFactory)
        {
            
        }

        public void Configure(IParticleFactory particleFactory, IHitStopManager hitStopManager, ICameraShaker cameraShaker)
        {
            
        }

        public void ResetView()
        {
            _dropShadow.Hide();
            _meshTransform.DOComplete();
        }
        
        public async UniTaskVoid PlayThrownAnimation(float duration)
        {
            _dropShadow.Show();
            
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_throwScalePunch, duration, 1)
                .SetEase(Ease.InOutQuad);
        }

        public async UniTaskVoid PlayPulledAnimation(float duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_pulledDelay));

            _dropShadow.Show();
            
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_pullScalePunch, duration, 1)
                .SetEase(Ease.InOutQuad);
        }

        public void PlayKickedAnimation(float duration)
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_kickScalePunch, duration, 1)
                .SetEase(Ease.InOutQuad);
        }

        public void PlayCarriedAnimation()
        {
            _dropShadow.Hide();
            
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_carriedScalePunch, 0.2f, 1)
                .SetEase(Ease.InOutQuad);
        }

        public void PlayRestOnFloorAnimation()
        {
            _dropShadow.Hide();
            
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_restingOnFloorScalePunch, 0.2f, 1)
                .SetEase(Ease.InOutQuad);
        }

        public void PlaySpinningAnimation()
        {
            _dropShadow.Show();
        }

        public void PlayObstructedAnimation()
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchRotation(_obstructedRotationPunch, 0.3f, 10)
                .SetEase(Ease.InOutQuad);
        }

        public void StopCarry()
        {
            
        }

        public void OnDamageDealt(DamageHitResult damageHitResult)
        {
            throw new NotImplementedException();
        }


        private async UniTaskVoid PlayTwistLoopAnimation(float delay, int numberOfLoops, float duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            
            float durationStep = duration / (numberOfLoops * 2);
            
            _specialMotionsTransform.DOBlendableLocalRotateBy(new Vector3(0, 0, 180), durationStep)
                .SetEase(Ease.Linear)
                .OnComplete(() => 
                    _specialMotionsTransform.DOBlendableLocalRotateBy(new Vector3(0, 0, 0), durationStep)
                        .SetEase(Ease.Linear)
                    )
                .SetLoops(numberOfLoops);
        }
        
    }
}