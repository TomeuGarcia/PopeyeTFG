using Popeye.Core.Services.EventSystem;
using Popeye.Modules.GameState;
using Popeye.Modules.PlayerAnchor.Player.BattleInteractions;
using Popeye.Modules.PlayerAnchor.Player.PlayerEvents;


namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    public class GameStateMusicAudioManager : IGameAudiosManager
    {
        private readonly IGameMusicTransitionController _gameMusicTransitionController;
        private readonly IPlayerStateMusicTransitionController _playerStateMusicTransitionController;
        private IEventSystemService _eventSystemService;


        public GameStateMusicAudioManager(
            IGameMusicTransitionController gameMusicTransitionController,
            IPlayerStateMusicTransitionController playerStateMusicTransitionController
        )
        {
            _gameMusicTransitionController = gameMusicTransitionController;
            _playerStateMusicTransitionController = playerStateMusicTransitionController;
        }
        
        public void Init(AFMODAudioManagerReference audioManager, IEventSystemService eventSystemService)
        {
            _eventSystemService = eventSystemService;
        }

        public void StartListeningToGameEvents()
        {
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnStartLoadingAnyScene>(OnStartLoadingAdditiveSceneEvent);
            
            _eventSystemService.Subscribe<IPlayerEventsDispatcher.OnRespawnFromDeathEvent>(OnPlayerRespawnedFromDeathEvent);
            _eventSystemService.Subscribe<IPlayerEventsDispatcher.OnDieEvent>(OnPlayerDiedEvent);
            _eventSystemService.Subscribe<IPlayerEventsDispatcher.OnTakeDamageEvent>(OnTakeDamageEvent);
            
            _eventSystemService.Subscribe<IPlayerBattleInteractionsController.OnBattleStarted>(OnBattleStartedEvent);
            _eventSystemService.Subscribe<IPlayerBattleInteractionsController.OnBattleFinished>(OnBattleFinishedEvent);
        }

        public void StopListeningToGameEvents()
        {
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnStartLoadingAnyScene>(OnStartLoadingAdditiveSceneEvent);
            
            _eventSystemService.Unsubscribe<IPlayerEventsDispatcher.OnRespawnFromDeathEvent>(OnPlayerRespawnedFromDeathEvent);
            _eventSystemService.Unsubscribe<IPlayerEventsDispatcher.OnDieEvent>(OnPlayerDiedEvent);
            _eventSystemService.Unsubscribe<IPlayerEventsDispatcher.OnTakeDamageEvent>(OnTakeDamageEvent);
            
            _eventSystemService.Unsubscribe<IPlayerBattleInteractionsController.OnBattleStarted>(OnBattleStartedEvent);
            _eventSystemService.Unsubscribe<IPlayerBattleInteractionsController.OnBattleFinished>(OnBattleFinishedEvent);
        }


        private void OnStartLoadingAdditiveSceneEvent(IGameStateEventsDispatcher.OnStartLoadingAnyScene eventData)
        {
            _gameMusicTransitionController.TransitionToSceneMusic(eventData.SceneReference);
        }
                
        private void OnPlayerRespawnedFromDeathEvent(IPlayerEventsDispatcher.OnRespawnFromDeathEvent eventData)
        {
            _playerStateMusicTransitionController.TransitionToDefault();
        }
        private void OnPlayerDiedEvent(IPlayerEventsDispatcher.OnDieEvent eventData)
        {
            _playerStateMusicTransitionController.TransitionToDeath();
        }
        
        
        private void OnTakeDamageEvent(IPlayerEventsDispatcher.OnTakeDamageEvent eventData)
        {
            _playerStateMusicTransitionController.TransitionToTakingDamage();
        }


        
        private void OnBattleStartedEvent(IPlayerBattleInteractionsController.OnBattleStarted eventData)
        {
            _playerStateMusicTransitionController.TransitionToBattle();
        }
        private void OnBattleFinishedEvent(IPlayerBattleInteractionsController.OnBattleFinished eventData)
        {
            _playerStateMusicTransitionController.TransitionOutOfBattle();
        }
        
    }
}