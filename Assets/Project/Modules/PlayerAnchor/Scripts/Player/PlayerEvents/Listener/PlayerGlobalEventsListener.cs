using Popeye.Core.Services.EventSystem;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.GameState;
using Popeye.Modules.PlayerAnchor.Player.AutoActionsQueue;
using Popeye.Modules.PlayerAnchor.Player.PlayerFocus;
using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerEvents
{
    public class PlayerGlobalEventsListener : IPlayerGlobalEventsListener
    {
        private readonly IEventSystemService _eventSystemService;
        private readonly IPlayerAutoActionsQueue _playerAutoActionsQueue;
        
        private readonly IEmptyEventChannelListenEntry _playerHealthBoostEvent;
        private readonly IPlayerHealthUpgrader _playerHealthUpgrader;
        
        private readonly IEmptyEventChannelListenEntry _playerFocusBoostEvent;
        private readonly IPlayerFocusUpgrader _playerFocusUpgrader;


        public PlayerGlobalEventsListener(
            IEventSystemService eventSystemService, 
            IPlayerAutoActionsQueue playerAutoActionsQueue,
            IEmptyEventChannelListenEntry playerHealthBoostEvent, IPlayerHealthUpgrader playerHealthUpgrader,
            IEmptyEventChannelListenEntry playerFocusBoostEvent, IPlayerFocusUpgrader playerFocusUpgrader)
        {
            _eventSystemService = eventSystemService;
            _playerAutoActionsQueue = playerAutoActionsQueue;
            
            _playerHealthBoostEvent = playerHealthBoostEvent;
            _playerHealthUpgrader = playerHealthUpgrader;
            
            _playerFocusBoostEvent = playerFocusBoostEvent;
            _playerFocusUpgrader = playerFocusUpgrader;
        }
        
        public void StartListening()
        {
            _eventSystemService.Subscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivated);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnStartUnloadingScene>(OnSceneStartsUnloading);
            
            _playerHealthBoostEvent.Subscribe(OnPlayerHealthBoostCollected);
            _playerFocusBoostEvent.Subscribe(OnPlayerFocusBoostCollected);
        }

        public void StopListening()
        {
            _eventSystemService.Unsubscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivated);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnStartUnloadingScene>(OnSceneStartsUnloading);
            
            _playerHealthBoostEvent.Unsubscribe(OnPlayerHealthBoostCollected);
            _playerFocusBoostEvent.Unsubscribe(OnPlayerFocusBoostCollected);
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
        private void OnPlayerFocusBoostCollected()
        {
            _playerFocusUpgrader.IncreaseMaxFocus();
        }
        
    }
}