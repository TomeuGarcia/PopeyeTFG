using Cysharp.Threading.Tasks;
using Popeye.Core.Services.EventSystem;
using Popeye.Modules.Enemies;
using Popeye.Modules.Enemies.General;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.BattleInteractions
{
    public class PlayerBattleInteractionsController : IPlayerBattleInteractionsController
    {
        private readonly IEventSystemService _eventSystemService;
        private int _activeEnemyFightsCounter;

        public PlayerBattleInteractionsController(IEventSystemService eventSystemService)
        {
            _eventSystemService = eventSystemService;
            _activeEnemyFightsCounter = 0;
        }
        
        public void StartListening()
        {
            _eventSystemService.Subscribe<AEnemyMediator.EnemyStartsFightingPlayer>(OnEnemyStartsFightingPlayerEvent);
            _eventSystemService.Subscribe<AEnemyMediator.EnemyStopsFightingPlayer>(OnEnemyStopsFightingPlayer);
            
            _eventSystemService.Subscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivatedEvent);
            _eventSystemService.Subscribe<EnemySpawner.OnFinishedEvent>(OnEnemySpawnerFinishedEvent);
        }

        public void StopListening()
        {
            _eventSystemService.Unsubscribe<AEnemyMediator.EnemyStartsFightingPlayer>(OnEnemyStartsFightingPlayerEvent);
            _eventSystemService.Unsubscribe<AEnemyMediator.EnemyStopsFightingPlayer>(OnEnemyStopsFightingPlayer);
            
            _eventSystemService.Unsubscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivatedEvent);
            _eventSystemService.Unsubscribe<EnemySpawner.OnFinishedEvent>(OnEnemySpawnerFinishedEvent);
        }
        
        
        private void OnEnemyStartsFightingPlayerEvent(AEnemyMediator.EnemyStartsFightingPlayer eventData)
        {
            CheckStartFighting();
        }
        private void OnEnemyStopsFightingPlayer(AEnemyMediator.EnemyStopsFightingPlayer eventData)
        {
            CheckFinishFighting();
        }
        
        private void OnEnemySpawnerActivatedEvent(EnemySpawner.OnActivatedEvent eventData)
        {
            CheckStartFighting();
        }
        private void OnEnemySpawnerFinishedEvent(EnemySpawner.OnFinishedEvent eventData)
        {
            CheckFinishFighting();
        }


        private void CheckStartFighting()
        {
            ++_activeEnemyFightsCounter;
            AsyncCheckFighting().Forget();
        }
        
        private void CheckFinishFighting()
        {
            --_activeEnemyFightsCounter;
            AsyncCheckFighting().Forget();           
        }

        private bool _isChecking;
        private int _previousCounter = 0;
        private async UniTaskVoid AsyncCheckFighting()
        {
            if (_isChecking)
            {
                return;
            }
            _isChecking = true;

            await UniTask.Yield();

            if (_previousCounter == 0 && _activeEnemyFightsCounter > 0)
            {
                DoStartFighting();
            }
            else if (_previousCounter > 0 && _activeEnemyFightsCounter == 0)
            {
                DoFinishFighting();
            }

            _previousCounter = _activeEnemyFightsCounter;
            
            _isChecking = false;
        }
        
        
        private void DoStartFighting()
        {
            Debug.Log("START " + _activeEnemyFightsCounter);
            _eventSystemService.Dispatch(new IPlayerBattleInteractionsController.OnBattleStarted());
        }
        private void DoFinishFighting()
        {
            Debug.Log("FINISH " + _activeEnemyFightsCounter);
            _eventSystemService.Dispatch(new IPlayerBattleInteractionsController.OnBattleFinished());
        }
        
    }
}