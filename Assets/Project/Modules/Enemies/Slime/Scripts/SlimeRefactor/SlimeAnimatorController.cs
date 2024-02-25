using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Popeye.Core.Pool;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.ParticleFactories;
using Unity.Mathematics;
using UnityEngine.Pool;

namespace Popeye.Modules.Enemies.Components
{
    public class SlimeAnimatorController : MonoBehaviour, IEnemyAnimator
    {
        private Core.Pool.ObjectPool _objectPool;
        
        [SerializeField] private float _squashAmountY = 0.8f;
        [SerializeField] private float _squashAmountXZ = 1.2f;
        [SerializeField] private float _stretchAmountY = 1.2f;
        [SerializeField] private float _stretchAmountXZ = 1f;
        [SerializeField] private float _squashAndStretchTime = 0.5f;

        private bool _playAnimation = false;
        private const string MOVE_ANIMATOR_PARAMETER = "Moving";
        

        protected AEnemyMediator _mediator;

        [SerializeField] private Animator _animator;
        private IParticleFactory _particleFactory;

        public void Configure(AEnemyMediator slimeMediator, IParticleFactory particleFactory)
        {
            _mediator = slimeMediator;
            _particleFactory = particleFactory;

        }

        private void SpawnExplosionParticles()
        {
            _particleFactory.Create(ParticleTypes.SlimeDeathParticles, transform.position, Quaternion.identity);
        }
        

        
        public void PlayTakeDamage()
        {
            throw new NotImplementedException();
        }

        public void PlayDeath()
        {
            SpawnExplosionParticles();
        }

        public void PlayMove()
        {
            //move animation
            _playAnimation = true;
            _animator.SetBool(MOVE_ANIMATOR_PARAMETER, true);
        }

        public void StopMove()
        {
            //idle animation
            _animator.SetBool(MOVE_ANIMATOR_PARAMETER,false);
            _playAnimation = false;
        }
    }
}
