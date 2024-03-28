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
            _eventSystemService.Dispatch(new OnPlayerTakeDamageEvent(playerPosition, damageHitResult.DamageHit, currentHealth));
        }

        public void DispatchOnDiedEvent(DamageHitResult damageHitResult, Vector3 playerPosition)
        {
            _eventSystemService.Dispatch(new IPlayerEventsDispatcher.OnDieEvent());
            
            _eventSystemService.Dispatch(new OnPlayerDeathEvent(playerPosition, damageHitResult.DamageHit));
        }

        public void DispatchOnRespawnFromDeathEvent()
        {
            _eventSystemService.Dispatch(new IPlayerEventsDispatcher.OnRespawnFromDeathEvent());
        }

        public void DispatchOnStartActionEvent(PlayerStates.PlayerStates playerState, Vector3 playerPosition)
        {
            _eventSystemService.Dispatch(new OnPlayerActionEvent(playerPosition, playerState));
        }
    }
}