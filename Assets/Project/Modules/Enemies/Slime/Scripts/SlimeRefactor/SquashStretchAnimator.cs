using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Popeye.Modules.Enemies.Components
{
    public class SquashStretchAnimator : MonoBehaviour, IEnemyAnimator
    {

        [SerializeField] private GameObject _explosionParticles;

        [SerializeField] private float _squashAmountY = 0.8f;
        [SerializeField] private float _squashAmountXZ = 1.2f;
        [SerializeField] private float _stretchAmountY = 1.2f;
        [SerializeField] private float _stretchAmountXZ = 1f;
        [SerializeField] private float _squashAndStretchTime = 0.5f;

        private bool _playAnimation = false;
        

        private Transform _transform;

        protected IEnemyMediator _mediator;


        public void Configure(IEnemyMediator slimeMediator,Transform transform)
        {
            _mediator = slimeMediator;
            _transform = transform;
        }
        

        private void SpawnExplosionParticles()
        {
            Instantiate(_explosionParticles, transform.position, Quaternion.identity);
        }


        

        private async void SquashAndStretch()
        {
            // Squash
           await _transform.DOScale(new Vector3(_squashAmountXZ, _squashAmountY, _squashAmountXZ), _squashAndStretchTime).AsyncWaitForCompletion();
           //Stretch
           await _transform.DOScale(new Vector3(_stretchAmountXZ, _stretchAmountY, _stretchAmountXZ), _squashAndStretchTime).AsyncWaitForCompletion();
           
           if(_playAnimation)
           SquashAndStretch();

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
            Debug.Log("playMovebro");
            _playAnimation = true;
            SquashAndStretch();
        }

        public void StopMove()
        {
            _playAnimation = false;
        }
    }
}
