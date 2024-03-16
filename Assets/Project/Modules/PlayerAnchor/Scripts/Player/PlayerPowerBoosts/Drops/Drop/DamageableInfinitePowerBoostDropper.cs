using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    public class DamageableInfinitePowerBoostDropper : MonoBehaviour, IDamageHitTarget
    {
        [SerializeField] private DamageHitTargetType _hitType;
        [SerializeField] private PowerBoostDropConfig _dropConfig;
        private IPowerBoostDropFactory _dropFactory;
        
        private void Start()
        {
            _dropFactory = ServiceLocator.Instance.GetService<IPowerBoostDropFactory>();
        }
        

        public DamageHitTargetType GetDamageHitTargetType()
        {
            return _hitType;
        }

        public DamageHitResult TakeHitDamage(DamageHit damageHit)
        {
            _dropFactory.Create(transform.position, Quaternion.identity, _dropConfig);
            
            return new DamageHitResult(this, gameObject, damageHit.Damage, transform.position);
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