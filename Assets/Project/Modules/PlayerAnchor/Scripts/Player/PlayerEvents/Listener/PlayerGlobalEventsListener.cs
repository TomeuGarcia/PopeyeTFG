using Popeye.Core.Services.EventSystem;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerEvents
{
    public class PlayerGlobalEventsListener : IPlayerGlobalEventsListener
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly IPlayerMediator _player;
        private readonly IAnchorMediator _anchor;


        public PlayerGlobalEventsListener(IEventSystemService eventSystemService, 
            IPlayerMediator player, IAnchorMediator anchor)
        {
            _eventSystemService = eventSystemService;
            _player = player;
            _anchor = anchor;
        }
        
        public void StartListening()
        {
            _eventSystemService.Subscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivated);
        }

        public void StopListening()
        {
            _eventSystemService.Unsubscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivated);
        }
        
        
        
        private void OnEnemySpawnerActivated(EnemySpawner.OnActivatedEvent data)
        {
            if (!_anchor.IsBeingCarried() && !_anchor.IsBeingPulled())
            {
                _player.QueuePullAnchor().Forget();
            }
        }
        
    }
}