using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Popeye.Core.Services.EventSystem;
using Popeye.Modules.Enemies;
using Popeye.Modules.GameState;
using Popeye.Modules.PlayerAnchor.Player.PlayerEvents;
using UnityEngine;


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
            Print().Forget();
        }

        public void StartListeningToGameEvents()
        {
            _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnStartLoadingAnyScene>(OnStartLoadingAdditiveSceneEvent);
            
            _eventSystemService.Subscribe<IPlayerEventsDispatcher.OnRespawnFromDeathEvent>(OnPlayerRespawnedFromDeathEvent);
            _eventSystemService.Subscribe<IPlayerEventsDispatcher.OnDieEvent>(OnPlayerDiedEvent);
            _eventSystemService.Subscribe<IPlayerEventsDispatcher.OnEnterBattle>(OnPlayerEnterBattleEvent);
            _eventSystemService.Subscribe<IPlayerEventsDispatcher.OnTakeDamageEvent>(OnTakeDamageEvent);
            
            _eventSystemService.Subscribe<AEnemyMediator.EnemyStartsFightingPlayer>(StartFight);
            _eventSystemService.Subscribe<AEnemyMediator.EnemyStopsFightingPlayer>(StopFight);
        }

        public void StopListeningToGameEvents()
        {
            _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnStartLoadingAnyScene>(OnStartLoadingAdditiveSceneEvent);
            
            _eventSystemService.Unsubscribe<IPlayerEventsDispatcher.OnRespawnFromDeathEvent>(OnPlayerRespawnedFromDeathEvent);
            _eventSystemService.Unsubscribe<IPlayerEventsDispatcher.OnDieEvent>(OnPlayerDiedEvent);
            _eventSystemService.Unsubscribe<IPlayerEventsDispatcher.OnEnterBattle>(OnPlayerEnterBattleEvent);
            _eventSystemService.Unsubscribe<IPlayerEventsDispatcher.OnExitBattle>(OnPlayerExitBattleEvent);
            _eventSystemService.Unsubscribe<IPlayerEventsDispatcher.OnTakeDamageEvent>(OnTakeDamageEvent);
            
            _eventSystemService.Unsubscribe<AEnemyMediator.EnemyStartsFightingPlayer>(StartFight);
            _eventSystemService.Unsubscribe<AEnemyMediator.EnemyStopsFightingPlayer>(StopFight);
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
        private void OnPlayerEnterBattleEvent(IPlayerEventsDispatcher.OnEnterBattle eventData)
        {
            _playerStateMusicTransitionController.TransitionToBattle();
        }
        private void OnPlayerExitBattleEvent(IPlayerEventsDispatcher.OnExitBattle eventData)
        {
            _playerStateMusicTransitionController.TransitionOutOfBattle();
        }
        
        private void OnTakeDamageEvent(IPlayerEventsDispatcher.OnTakeDamageEvent eventData)
        {
            _playerStateMusicTransitionController.TransitionToTakingDamage();
        }


        private int counter = 0;
        private void StartFight(AEnemyMediator.EnemyStartsFightingPlayer eventData)
        {
            ++counter;
        }
        private void StopFight(AEnemyMediator.EnemyStopsFightingPlayer eventData)
        {
            --counter;
        }

        private async UniTaskVoid Print()
        {
            while (true)
            {
                Debug.Log(counter);
                await UniTask.Yield();
            }
        }
    }
}