using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Popeye.Core.Pool;
using Popeye.Modules.Camera.CameraZoom;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    public class AutoCollectPowerBoostDrop : RecyclableObject, IPowerBoostDrop
    {
        public int Experience { get; private set; }
        private bool _wasUsed;

        private Transform _autoCollectTransform;

        [SerializeField] private PowerBoostDropBehaviourConfig _behaviourConfig;
        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private ParticleSystem _bodyParticles;
        [SerializeField] private ParticleSystem _spawnParticles;
        [SerializeField] private ParticleSystem _collectedParticles;



        public void Init(int experience, Transform autoCollectTransform)
        {
            Experience = experience;
            _autoCollectTransform = autoCollectTransform;
            
            _bodyParticles.Play();
            _bodyParticles.gameObject.SetActive(true);
            _spawnParticles.Play();
            _trail.emitting = true;

            StartCoroutine(MoveToAutoCollect());
        }

        private IEnumerator MoveToAutoCollect()
        {
            transform.position += _behaviourConfig.RandomSpawnPositionOffset;
            
            yield return new WaitForSeconds(_behaviourConfig.DelayBeforeStartMoving);

            float movementSpeed = _behaviourConfig.RandomMovementSpeed;
            Vector3 movementBendAxis = _behaviourConfig.RandomMovementBendAxis;
            
            while (isActiveAndEnabled)
            {
                Vector3 toAutoCollect = (_autoCollectTransform.position - transform.position).normalized;
                float toAutoCollectDistance = toAutoCollect.magnitude;
                Vector3 toAutoCollectDirection = toAutoCollect / toAutoCollectDistance;

                toAutoCollectDirection += Vector3.Cross(movementBendAxis, toAutoCollectDirection);
                toAutoCollectDirection.Normalize();
                
                transform.position += toAutoCollectDirection * (movementSpeed * Time.deltaTime);
                transform.forward = toAutoCollectDirection;
                
                yield return null;
            }            
        }
        
        

        internal override void Init()
        {
            _wasUsed = false;
        }

        internal override void Release()
        {
        }

        public bool CanBeUsed()
        {
            return !_wasUsed;
        }

        public int GetExperienceAndSetUsed()
        {
            _wasUsed = true;
            Disappear().Forget();
            return Experience;
        }

        private async UniTaskVoid Disappear()
        {
            _bodyParticles.Stop();
            _bodyParticles.gameObject.SetActive(false);
            _collectedParticles.Play();
            _trail.emitting = false;

            await UniTask.WaitUntil(() => !_collectedParticles.isEmitting);
            
            Recycle();
        }
    }
}