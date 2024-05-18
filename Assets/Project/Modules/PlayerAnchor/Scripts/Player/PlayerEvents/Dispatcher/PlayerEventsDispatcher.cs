using Popeye.Core.Services.EventSystem;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.GameDataEvents;
using Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations;
using Popeye.Scripts.EventChannels;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerEvents
{
    public class PlayerEventsDispatcher : IPlayerEventsDispatcher
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly IEmptyEventChannelDispatcher _dashTowardsAnchorDispatcher;
        private readonly Timer _updateTimer;

        public PlayerEventsDispatcher(
            IEventSystemService eventSystemService, 
            IEmptyEventChannelDispatcher dashTowardsAnchorDispatcher)
        {
            _eventSystemService = eventSystemService;
            _dashTowardsAnchorDispatcher = dashTowardsAnchorDispatcher;
            _updateTimer = new Timer(1.0f);
        }

        

        public void DispatchOnDiedEvent()
        {
            _eventSystemService.Dispatch(new IPlayerEventsDispatcher.OnDieEvent());
        }

        public void DispatchOnRespawnFromDeathEvent()
        {
            _eventSystemService.Dispatch(new IPlayerEventsDispatcher.OnRespawnFromDeathEvent());
        }

        public void DispatchDashTowardsAnchorPerformed()
        {
            _dashTowardsAnchorDispatcher.RaiseEvent();
        }

        public void DispatchOnStartActionEvent(PlayerMovesetActions actionName, Vector3 playerPosition)
        {
            _eventSystemService.Dispatch(new OnPlayerActionEvent(playerPosition, actionName));
        }
        
        public void DispatchOnTakeDamageEvent(DamageHitResult damageHitResult, Vector3 playerPosition, int currentHealth)
        {
            _eventSystemService.Dispatch(new IPlayerEventsDispatcher.OnTakeDamageEvent());
            _eventSystemService.Dispatch(new OnPlayerTakeDamageEvent(playerPosition, damageHitResult, currentHealth));
        }
        public void DispatchOnKilledByDamageEvent(DamageHitResult damageHitResult, Vector3 playerPosition)
        {
            _eventSystemService.Dispatch(new IPlayerEventsDispatcher.OnTakeDamageEvent());
            _eventSystemService.Dispatch(new OnPlayerKilledByDamageEvent(playerPosition, damageHitResult));
        }

        public void DispatchOnHealEvent(Vector3 playerPosition, int currentHealth, int healthBeforeHealing)
        {
            _eventSystemService.Dispatch(new OnPlayerHealEvent(playerPosition, currentHealth, healthBeforeHealing));
        }

        public void Update(float deltaTime, Vector3 playerPosition)
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