using Popeye.Core.Services.EventSystem;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.GameState;
using Popeye.Modules.PlayerAnchor.Player.AutoActionsQueue;
using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerEvents
{
    public class PlayerGlobalEventsListener : IPlayerGlobalEventsListener
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly IPlayerAutoActionsQueue _playerAutoActionsQueue;
        private readonly IEmptyEventChannelListenEntry _playerHealthBoost;
        private readonly IPlayerHealthUpgrader _playerHealthUpgrader;


        public PlayerGlobalEventsListener(
            IEventSystemService eventSystemService, 
            IPlayerAutoActionsQueue playerAutoActionsQueue,
            IEmptyEventChannelListenEntry playerHealthBoost, IPlayerHealthUpgrader playerHealthUpgrader)
        {
            _eventSystemService = eventSystemService;
            _playerAutoActionsQueue = playerAutoActionsQueue;
            _playerHealthBoost = playerHealthBoost;
            _playerHealthUpgrader = playerHealthUpgrader;
        }
        
        public void StartListening()
        {
            _eventSystemService.Subscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivated);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnStartUnloadingScene>(OnSceneStartsUnloading);
            
            _playerHealthBoost.Subscribe(OnPlayerHealthBoostCollected);
        }

        public void StopListening()
        {
            _eventSystemService.Unsubscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivated);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnStartUnloadingScene>(OnSceneStartsUnloading);
            
            _playerHealthBoost.Unsubscribe(OnPlayerHealthBoostCollected);
        }
        
        
        
        private void OnEnemySpawnerActivated(EnemySpawner.OnActivatedEvent eventData)
        {
            _playerAutoActionsQueue.TryQueueAnchorPull();
        }
        
        
        private void OnSceneStartsUnloading(IGameStateEventsDispatcher.OnStartUnloadingScene eventData)
        {
            _playerAutoActionsQueue.ProtectPlayerWhenSceneLoading();
        }
        
        private void OnPlayerHealthBoostCollected()
        {
            _playerHealthUpgrader.IncreaseMaxHealth();
        }
        
    }
}