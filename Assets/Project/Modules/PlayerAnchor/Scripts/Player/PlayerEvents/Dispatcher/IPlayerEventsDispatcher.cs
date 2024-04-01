using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerEvents
{
    public interface IPlayerEventsDispatcher
    {
        public struct OnDieEvent { }
        public struct OnRespawnFromDeathEvent { }


        void DispatchOnDiedEvent();
        void DispatchOnRespawnFromDeathEvent();
        
        void DispatchOnStartActionEvent(string actionName, Vector3 playerPosition);
        void DispatchOnTakeDamageEvent(DamageHitResult damageHitResult, Vector3 playerPosition, int currentHealth);
        void DispatchOnHealEvent(Vector3 playerPosition, int currentHealth, int healthBeforeHealing);
        void Update(float deltaTime, Vector3 playerPosition);


    }
}