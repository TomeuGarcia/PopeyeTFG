using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class TurretAnimatorController : MonoBehaviour
    {
        private const string SHOOT_ANIMATOR_PARAMETER = "StartShootingAnimation";
        protected AEnemyMediator _mediator;

        [SerializeField] private Animator _animator;

        public void Configure(AEnemyMediator slimeMediator)
        {
            _mediator = slimeMediator;
        }

        public void PlayShootingAnimation()
        {
            _animator.SetBool(SHOOT_ANIMATOR_PARAMETER, true);
        }
        public void StopShootingAnimation()
        {
            _animator.SetBool(SHOOT_ANIMATOR_PARAMETER, false);
        }
    }
}
