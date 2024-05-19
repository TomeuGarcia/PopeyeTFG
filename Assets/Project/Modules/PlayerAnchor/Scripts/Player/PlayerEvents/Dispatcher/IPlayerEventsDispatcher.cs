using Popeye.Modules.CombatSystem;
using Popeye.Modules.GameDataEvents;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerEvents
{
    public interface IPlayerEventsDispatcher
    {
        public struct OnTakeDamageEvent { }
        public struct OnDieEvent { }
        public struct OnRespawnFromDeathEvent { }


        void DispatchOnDiedEvent();
        void DispatchOnRespawnFromDeathEvent();
        
        void DispatchDashTowardsAnchorPerformed();
        void DispatchOnStartActionEvent(PlayerMovesetActions actionName, Vector3 playerPosition);
        void DispatchOnTakeDamageEvent(DamageHitResult damageHitResult, Vector3 playerPosition, int currentHealth);
        void DispatchOnKilledByDamageEvent(DamageHitResult damageHitResult, Vector3 playerPosition);
        void DispatchOnHealEvent(Vector3 playerPosition, int currentHealth, int healthBeforeHealing);
        void Update(float deltaTime, Vector3 playerPosition);


    }
}