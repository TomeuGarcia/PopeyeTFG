using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class ShieldedStun : MonoBehaviour
    {

        [SerializeField] private int _stunnedTimeInMillis;
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
            await UniTask.Delay(350);
            _mediator.SetIsInvulnerable(false);
            await UniTask.Delay(_stunnedTimeInMillis);
            if (_stunned)
            {
                CancellStun();
            }
            
        }
    }
}
