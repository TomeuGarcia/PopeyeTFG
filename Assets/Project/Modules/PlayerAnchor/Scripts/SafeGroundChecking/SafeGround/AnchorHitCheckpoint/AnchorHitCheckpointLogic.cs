using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.CombatSystem;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint
{
    public class AnchorHitCheckpointLogic : MonoBehaviour, IDamageHitTarget
    {
        [SerializeField] private AnchorHitCheckpointView _view;
        

        
        private void OnGameObjectEnters(GameObject other)
        {
            
        }



        public DamageHitTargetType GetDamageHitTargetType()
        {
            return DamageHitTargetType.Interactable;
        }

        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            _view.ComputeBounceAxis(damageHit.DamageSourcePosition);
            _view.PlayBounceAnimation();
            
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