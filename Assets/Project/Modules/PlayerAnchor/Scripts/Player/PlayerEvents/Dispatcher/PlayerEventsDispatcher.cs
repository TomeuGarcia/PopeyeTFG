using Popeye.Core.Services.EventSystem;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.GameDataEvents;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerEvents
{
    public class PlayerEventsDispatcher : IPlayerEventsDispatcher
    {
        private readonly IEventSystemService _eventSystemService;

        public PlayerEventsDispatcher(IEventSystemService eventSystemService)
        {
            _eventSystemService = eventSystemService;
        }


        public void DispatchOnTakeDamageEvent(DamageHitResult damageHitResult, Vector3 playerPosition, int currentHealth)
        {
            _eventSystemService.Dispatch(new OnPlayerTakeDamageEvent(playerPosition, damageHitResult, currentHealth));
        }

        public void DispatchOnDiedEvent()
        {
            _eventSystemService.Dispatch(new IPlayerEventsDispatcher.OnDieEvent());
        }

        public void DispatchOnRespawnFromDeathEvent()
        {
            _eventSystemService.Dispatch(new IPlayerEventsDispatcher.OnRespawnFromDeathEvent());
        }

        public void DispatchOnStartActionEvent(string actionName, Vector3 playerPosition)
        {
            _eventSystemService.Dispatch(new OnPlayerActionEvent(playerPosition, actionName));
        }
    }
}