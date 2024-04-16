using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class TurretAnimationCallback : MonoBehaviour
    {
        [SerializeField] private bool _multipleShot = false;
        protected TurretMediator _mediator;

        public void Configure(TurretMediator mediator)
        {
            _mediator = mediator;
        }

        public void Shoot()
        {
            if (_multipleShot)
            {
                _mediator.MultipleShoot();
            }
            else
            {
                _mediator.Shoot();
            }
            
        }

        public void TurretFinishedShooting()
        {
            _mediator.StopShootingAnimation();
            _mediator.StartIdleAnimation();
        }

        public void OutOfGround()
        {
            _mediator.SetOutOfGround();
        }
        
        public void InsideGround()
        {
            _mediator.SetInsideGround();
        }

        public void SetVulnerable()
        {
            _mediator.SetVulnerable();
        }

        public void SetInvulnerable()
        {
            _mediator.SetInvulnerable();
        }
    }
}
