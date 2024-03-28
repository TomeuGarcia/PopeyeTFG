using Popeye.Core.Services.EventSystem;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.GameDataEvents;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerEvents
{
    public class PlayerEventsDispatcher : IPlayerEventsDispatcher
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly Timer _updateTimer;

        public PlayerEventsDispatcher(IEventSystemService eventSystemService, float updateFrequency)
        {
            _eventSystemService = eventSystemService;
            _updateTimer = new Timer(updateFrequency);
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
        
        public void DispatchOnTakeDamageEvent(DamageHitResult damageHitResult, Vector3 playerPosition, int currentHealth)
        {
            _eventSystemService.Dispatch(new OnPlayerTakeDamageEvent(playerPosition, damageHitResult, currentHealth));
        }

        public void UpdateDispatchOnUpdateEvent(float deltaTime, Vector3 playerPosition)
        {
            _updateTimer.Update(deltaTime);
            if (_updateTimer.HasFinished())
            {
                _updateTimer.Clear();
                _eventSystemService.Dispatch(new OnPlayerUpdateEvent(playerPosition));
            }
        }
    }
}