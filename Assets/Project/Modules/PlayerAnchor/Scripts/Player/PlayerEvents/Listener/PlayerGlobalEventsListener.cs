using Popeye.Core.Services.EventSystem;
using Popeye.Modules.Enemies;
using Popeye.Modules.Enemies.General;
using Popeye.Modules.GameState;
using Popeye.Modules.PlayerAnchor.Player.AutoActionsQueue;
using Popeye.Modules.PlayerAnchor.Player.BattleInteractions;
using Popeye.Modules.PlayerAnchor.Player.PlayerFocus;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint;
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
        private readonly IPlayerBattleInteractionsController _battleInteractionsController;
        private readonly PlayerHealth _playerHealth;


        public PlayerGlobalEventsListener(
            IEventSystemService eventSystemService, 
            IPlayerAutoActionsQueue playerAutoActionsQueue,
            IEmptyEventChannelListenEntry playerHealthBoostEvent, IPlayerHealthUpgrader playerHealthUpgrader,
            IEmptyEventChannelListenEntry playerFocusBoostEvent, IPlayerFocusUpgrader playerFocusUpgrader,
            IPlayerBattleInteractionsController battleInteractionsController,
            PlayerHealth playerHealth)
        {
            _eventSystemService = eventSystemService;
            _playerAutoActionsQueue = playerAutoActionsQueue;
            
            _playerHealthBoostEvent = playerHealthBoostEvent;
            _playerHealthUpgrader = playerHealthUpgrader;
            
            _playerFocusBoostEvent = playerFocusBoostEvent;
            _playerFocusUpgrader = playerFocusUpgrader;
            
            _battleInteractionsController = battleInteractionsController;
            _playerHealth = playerHealth;
        }
        
        public void StartListening()
        {
            _eventSystemService.Subscribe<AnchorHitCheckpoint.OnCheckpointSet>(OnCheckpointSetEvent);
            
            _eventSystemService.Subscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivated);
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnStartUnloadingScene>(OnSceneStartsUnloading);
            
            _playerHealthBoostEvent.Subscribe(OnPlayerHealthBoostCollected);
            _playerFocusBoostEvent.Subscribe(OnPlayerFocusBoostCollected);
            
            _battleInteractionsController.StartListening();
        }

        public void StopListening()
        {
            _eventSystemService.Unsubscribe<AnchorHitCheckpoint.OnCheckpointSet>(OnCheckpointSetEvent);
            
            _eventSystemService.Unsubscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivated);
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnStartUnloadingScene>(OnSceneStartsUnloading);
            
            _playerHealthBoostEvent.Unsubscribe(OnPlayerHealthBoostCollected);
            _playerFocusBoostEvent.Unsubscribe(OnPlayerFocusBoostCollected);
            
            _battleInteractionsController.StopListening();
        }


        private void OnCheckpointSetEvent(AnchorHitCheckpoint.OnCheckpointSet eventData)
        {
            if (!_playerHealth.IsMaxHealth())
            {
                _playerHealth.HealToMax();    
            }            
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