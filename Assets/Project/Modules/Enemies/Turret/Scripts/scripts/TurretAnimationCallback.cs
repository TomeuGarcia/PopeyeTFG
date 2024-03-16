using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class TurretAnimationCallback : MonoBehaviour
    {
        protected TurretMediator _mediator;

        public void Configure(TurretMediator mediator)
        {
            _mediator = mediator;
        }

        public void Shoot()
        {
            _mediator.Shoot();
        }

        public void TurretFinishedShooting()
        {
            _mediator.StopShootingAnimation();
            _mediator.StartIdleAnimation();
        }
        
    }
}
