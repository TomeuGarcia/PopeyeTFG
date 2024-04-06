using Popeye.Core.Services.EventSystem;
using Popeye.Modules.Enemies.General;
using Project.Modules.Enemies.General.Scripts.EnemySpwner.Audio;
using UnityEngine;

namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    public class EnemiesGameAudiosManager : MonoBehaviour, IGameAudiosManager
    {
        [Header("CONFIGURATION")] 
        [SerializeField] private EnemySpawnerAudioConfig _enemySpawnerAudioConfig;
        
        private IFMODAudioManager _audioManager;
        private IEventSystemService _eventSystemService;
        
        
    
        public void Init(IFMODAudioManager audioManager, IEventSystemService eventSystemService)
        {
            _audioManager = audioManager;
            _eventSystemService = eventSystemService;
        }

        public void StartListeningToGameEvents()
        {
            _eventSystemService.Subscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivated);
            _eventSystemService.Subscribe<EnemySpawner.OnCompletedEvent>(OnEnemySpawnerCompleted);
            _eventSystemService.Subscribe<EnemySpawner.OnHinterAppearsEvent>(OnEnemySpawnerHinterAppears);
        }

        public void StopListeningToGameEvents()
        {
            _eventSystemService.Unsubscribe<EnemySpawner.OnActivatedEvent>(OnEnemySpawnerActivated);
            _eventSystemService.Unsubscribe<EnemySpawner.OnCompletedEvent>(OnEnemySpawnerCompleted);
            _eventSystemService.Unsubscribe<EnemySpawner.OnHinterAppearsEvent>(OnEnemySpawnerHinterAppears);
        }



        private void OnEnemySpawnerActivated(EnemySpawner.OnActivatedEvent eventData)
        {
            _audioManager.PlayOneShotAttached(_enemySpawnerAudioConfig.EnemyWavesStart, eventData.spawnerGameObject);
        }
        private void OnEnemySpawnerCompleted(EnemySpawner.OnCompletedEvent eventData)
        {
            _audioManager.PlayOneShotAttached(_enemySpawnerAudioConfig.EnemyWavesCompleted, eventData.spawnerGameObject);
        }
        private void OnEnemySpawnerHinterAppears(EnemySpawner.OnHinterAppearsEvent eventData)
        {
            _audioManager.PlayOneShotAttached(eventData.hinter.Sound, eventData.hinter.gameObject);
        }
        
        
    }
}