using Popeye.Core.Services.EventSystem;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Modules.PlayerAnchor.Player.AutoActionsQueue;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerEvents
{
    public class PlayerGlobalEventsListener : IPlayerGlobalEventsListener
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly IPlayerAutoActionsQueue _playerAutoActionsQueue;

        public PlayerGlobalEventsListener(IEventSystemService eventSystemService, 
            IPlayerAutoActionsQueue playerAutoActionsQueue)
        {
            _eventSystemService = eventSystemService;
            _playerAutoActionsQueue = playerAutoActionsQueue;
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
            _playerAutoActionsQueue.TryQueueAnchorPull();
        }
        
    }
}