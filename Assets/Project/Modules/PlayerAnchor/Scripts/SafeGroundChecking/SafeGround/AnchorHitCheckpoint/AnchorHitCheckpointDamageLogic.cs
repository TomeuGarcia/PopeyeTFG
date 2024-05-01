using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint
{
    public class AnchorHitCheckpointDamageLogic : MonoBehaviour, IDamageHitTarget
    {
        private IAnchorHitCheckpointMediator _mediator;

        public void Configure(IAnchorHitCheckpointMediator mediator)
        {
            _mediator = mediator;
        }
        

        public DamageHitTargetType GetDamageHitTargetType()
        {
            return DamageHitTargetType.Interactable;
        }

        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            _mediator.OnWasHitByAnchor(damageHit.DamageSourcePosition);
            return new DamageHitResult(this, gameObject, damageHit, 0, transform.position);
        }

        public bool CanBeDamaged(DamageHit damageHit)
        {
            return true;
        }

        public bool IsDead()
        {
            return false;
        }
    }
}