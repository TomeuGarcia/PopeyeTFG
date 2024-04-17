using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class ShieldedStun : MonoBehaviour
    {

        [SerializeField] private float _stunnedTime = 0.5f;
        [SerializeField] private float _timeUntilSlamDisabled = 3.5f;
        private ShieldedMediator _mediator;
        private bool _stunned = false;
        
        public void Configure(ShieldedMediator mediator)
        {
            _mediator = mediator;
        }

        public void Stun()
        {
            _stunned = true;
            _mediator.StopDashing();
            _mediator.StopMoving();
            PerformStun();
        }

        public void CancellStun()
        {
            _stunned = false;
            _mediator.SetIsInvulnerable(true);
            _mediator.ResetDashingCooldown();
            _mediator.ActivateNavigation();
            _mediator.StartChasing();
        }

        private async UniTaskVoid PerformStun()
        {
            _mediator.DeactivateNavigation();
            await UniTask.Delay(TimeSpan.FromSeconds(_timeUntilSlamDisabled));
            _mediator.SetIsInvulnerable(false);
            await UniTask.Delay(TimeSpan.FromSeconds(_stunnedTime));
            if (_stunned)
            {
                CancellStun();
            }
            
        }
    }
}
