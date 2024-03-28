using Popeye.Modules.CombatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerEvents
{
    public interface IPlayerEventsDispatcher
    {
        public struct OnDieEvent { }
        public struct OnRespawnFromDeathEvent { }


        void DispatchOnTakeDamageEvent(DamageHitResult damageHitResult, Vector3 playerPosition, int currentHealth);
        void DispatchOnDiedEvent(DamageHitResult damageHitResult, Vector3 playerPosition);
        void DispatchOnRespawnFromDeathEvent();
        void DispatchOnStartActionEvent(string actionName, Vector3 playerPosition);


    }
}