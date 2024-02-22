using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Camera;
using Popeye.Modules.Camera.CameraShake;
using Project.Scripts.Time.TimeFunctionalities;
using Project.Scripts.Time.TimeHitStop;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private Transform _meshTransform;
        [SerializeField] private MeshRenderer _mesh;
        private Material _meshMaterial;

        [SerializeField, Range(0.0f, 5.0f)] private float _takeDamageDuration = 0.6f;
        [SerializeField, Range(0.0f, 5.0f)] private float _deathDuration = 1.0f;
        [SerializeField, Range(0.0f, 5.0f)] private float _healDuration = 0.3f;

        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _damagedColor;
        [SerializeField] private Color _healColor;

        [Header("TAKE DAMAGE")]
        [SerializeField, Range(0.0f, 5.0f)] private float _takeDamagePunchDuration = 0.3f;
        [SerializeField] private Vector3 _takeDamagePunchScale = new Vector3(-0.2f, 0.4f, -0.2f);
        [SerializeField] private HitStopConfig _hitStopDamageTaken;
        [SerializeField] private CameraShakeConfig _shakeConfigDamageTaken;
        
        [Header("HEAL")]
        [SerializeField, Range(0.0f, 5.0f)] private float _healPunchDuration = 0.3f;
        [SerializeField] private Vector3 _healPunchScale = new Vector3(0.2f, -0.4f, 0.2f);
        
        [Header("DEATH")]
        [SerializeField] private Vector3 _deathRotation = new Vector3(90, 0, 0);
        
        [Header("DASH")]
        [SerializeField] private Vector3 _dashPunchScale = new Vector3(0.2f, -0.4f, 0.2f);
        [SerializeField] private Vector3 _dashPunchRotation = new Vector3(45, 0, 0);
        
        [Header("KICK")]
        [SerializeField, Range(0.0f, 5.0f)] private float _kickPunchDuration = 0.3f;
        [SerializeField] private Vector3 _kickPunchScale = new Vector3(0.2f, -0.4f, 0.2f);
        [SerializeField] private Vector3 _kickPunchRotation = new Vector3(45, 0, 0);
        
        [Header("THROW")]
        [SerializeField, Range(0.0f, 5.0f)] private float _throwPunchDuration = 0.3f;
        [SerializeField] private Vector3 _throwPunchScale = new Vector3(0.2f, -0.4f, 0.2f);
        [SerializeField] private Vector3 _throwPunchRotation = new Vector3(45, 0, 0);
        
        [Header("PULL")]
        [SerializeField, Range(0.0f, 5.0f)] private float _pullPunchDuration = 0.3f;
        [SerializeField] private Vector3 _pullPunchScale = new Vector3(0.2f, -0.4f, 0.2f);
        [SerializeField] private Vector3 _pullPunchRotation = new Vector3(45, 0, 0);
        
        
        [Header("ANCHOR OBSTRUCTED")]
        [SerializeField, Range(0.0f, 5.0f)] private float _anchorObstructedPunchDuration = 0.3f;
        [SerializeField] private Vector3 _anchorObstructedPunchRotation = new Vector3(0, 45, 0);
        
        private IHitStopManager _hitStopManager;
        private ICameraShaker _cameraShaker;
        
        private int _tiredId;
        private int _baseColorId;
        
        private void Awake()
        {
            _meshMaterial = _mesh.material;
            _tiredId = Shader.PropertyToID("_IsTired");
            _baseColorId = Shader.PropertyToID("_BaseColor");
            
            
            EndTired();
            SetMeshBaseColor(_normalColor);
        }

        private void Start()
        {
            _hitStopManager = ServiceLocator.Instance.GetService<ITimeFunctionalities>().HitStopManager; 
            _cameraShaker = ServiceLocator.Instance.GetService<ICameraFunctionalities>().CameraShaker; 
        }

        public void StartTired()
        {
            _meshMaterial.SetFloat(_tiredId, 1.0f);
        }

        public void EndTired()
        {
            _meshMaterial.SetFloat(_tiredId, 0.0f);
        }

        public void PlayTakeDamageAnimation()
        {
            _hitStopManager.QueueHitStop(_hitStopDamageTaken);
            _cameraShaker.PlayShake(_shakeConfigDamageTaken);
            
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_takeDamagePunchScale, _takeDamagePunchDuration, 2)
                .SetEase(Ease.InOutSine);
            
            int numberOfFlicks = 3;
            FlickBaseColor(numberOfFlicks, _takeDamageDuration/numberOfFlicks, _damagedColor).Forget();
        }

        public void PlayRespawnAnimation()
        {
            _meshTransform.localRotation = Quaternion.identity;
            _meshTransform.localPosition = Vector3.zero;
        }

        public void PlayDeathAnimation()
        {
            float motionDuration = _deathDuration * 0.2f;
            _meshTransform.DORotate(_deathRotation, motionDuration)
                .SetEase(Ease.InOutQuad);
            _meshTransform.DOBlendableLocalMoveBy(Vector3.down*0.75f, motionDuration)
                .SetEase(Ease.InOutQuad);
            
            int numberOfFlicks = 2;
            FlickBaseColor(numberOfFlicks, _deathDuration / numberOfFlicks, _damagedColor).Forget();
            SetMeshBaseColor(_damagedColor);
        }

        public void PlayHealAnimation()
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_healPunchScale, _healPunchDuration, 1)
                .SetEase(Ease.InOutSine);

            int numberOfFlicks = 2;
            FlickBaseColor(numberOfFlicks, _healDuration / numberOfFlicks, _healColor).Forget();
        }

        public void PlayDashAnimation(float duration)
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_dashPunchScale, duration, 1)
                .SetEase(Ease.InOutBack);
            _meshTransform.DOPunchRotation(_dashPunchRotation, duration)
                .SetEase(Ease.InOutQuad);
        }
        
        public void PlayKickAnimation()
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_kickPunchScale, _kickPunchDuration, 1)
                .SetEase(Ease.InOutBack);
            _meshTransform.DOPunchRotation(_kickPunchRotation, _kickPunchDuration)
                .SetEase(Ease.InOutQuad);
        }

        
        public void PlayThrowAnimation()
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_throwPunchScale, _throwPunchDuration, 1)
                .SetEase(Ease.InOutBack);
            _meshTransform.DOPunchRotation(_throwPunchRotation, _throwPunchDuration)
                .SetEase(Ease.InOutQuad);
        }

        public async UniTaskVoid PlayPullAnimation(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_pullPunchScale, _pullPunchDuration, 1)
                .SetEase(Ease.InOutBack);
            _meshTransform.DOPunchRotation(_pullPunchRotation, _pullPunchDuration)
                .SetEase(Ease.InOutQuad);
        }

        public void PlayAnchorObstructedAnimation()
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchRotation(_anchorObstructedPunchRotation, _anchorObstructedPunchDuration, 10)
                .SetEase(Ease.InOutQuad);
        }


        private async UniTask FlickBaseColor(int numberOfFlicks, float flickDuration, Color flickColor)
        {
            flickDuration /= 2;
            for (int i = 0; i < numberOfFlicks; ++i)
            {
                SetMeshBaseColor(flickColor);
                await UniTask.Delay(TimeSpan.FromSeconds(flickDuration));
                SetMeshBaseColor(_normalColor);
                await UniTask.Delay(TimeSpan.FromSeconds(flickDuration));
            }
        }

        private void SetMeshBaseColor(Color color)
        {
            _meshMaterial.SetColor(_baseColorId, color);
        }
    }
}