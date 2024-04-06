using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Popeye.Modules.Enemies.Components
{
    public class TurretAnimatorController : MonoBehaviour
    {
        private const string SHOOT_ANIMATOR_PARAMETER = "StartShootingAnimation";
        private const string IDLE_ANIMATOR_PARAMETER = "StartIdleAnimation";
        [CanBeNull] private const string ON_PLAYER_CLOSE_ANIMATOR_PARAMETER = "OnPlayerClose";
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
        public void PlayIdleAnimation()
        {
            _animator.SetBool(IDLE_ANIMATOR_PARAMETER, true);
        }
        public void StopIdleAnimation()
        {
            _animator.SetBool(IDLE_ANIMATOR_PARAMETER, false);
        }

        public void AppearAnimation()
        {
            _animator.SetBool(ON_PLAYER_CLOSE_ANIMATOR_PARAMETER,true);
        }
        
        public void HideAnimation()
        {
            _animator.SetBool(ON_PLAYER_CLOSE_ANIMATOR_PARAMETER,false);
        }
    }
}
