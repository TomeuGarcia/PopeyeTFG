using Popeye.Core.Services.EventSystem;
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
        
        
        public void DispatchOnDiedEvent()
        {
            _eventSystemService.Dispatch(new IPlayerEventsDispatcher.OnDieEvent());
            
            _eventSystemService.Dispatch(new OnPlayerDeathEvent());
        }

        public void DispatchOnRespawnFromDeathEvent()
        {
            _eventSystemService.Dispatch(new IPlayerEventsDispatcher.OnRespawnFromDeathEvent());
        }
    }
}