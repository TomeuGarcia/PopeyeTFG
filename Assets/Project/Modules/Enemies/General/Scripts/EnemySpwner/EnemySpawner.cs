using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.Enemies.EnemyFactories;
using Popeye.Modules.PlayerAnchor.Player.DeathDelegate;
using Popeye.Modules.PlayerAnchor.Player.EnemyInteractions;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.ParticleFactories;
using Project.Modules.Enemies.General.Scripts.EnemySpwner.Audio;

namespace Popeye.Modules.Enemies.General
{
    public class EnemySpawner : MonoBehaviour, IPlayerDeathDelegate
    {

        [System.Serializable]
        class EnemyWave
        {
            [SerializeField, Range(0.0f, 10.0f)] private float _delayBeforeWaveSpawning = 1.0f;
            [SerializeField] private SpawnSequenceBeat[] _spawnSequence;

            public float DelayBeforeWaveSpawning => _delayBeforeWaveSpawning;
            public SpawnSequenceBeat[] SpawnSequence => _spawnSequence;
            public int NumberOfEnemies => _spawnSequence.Length;


            [System.Serializable]
            public class SpawnSequenceBeat
            {
                [SerializeField, Range(0.0f, 10.0f)] private float _delayBeforeSpawn = 0.5f;
                [SerializeField] private EnemyID _enemyID;
                [SerializeField] private Transform _spawnSpot;

                public float DelayBeforeSpawn => _delayBeforeSpawn;
                public EnemyID EnemyID => _enemyID;
                public Vector3 SpawnPosition => _spawnSpot.position;
            }

        }


        [Header("CONFIGURATIONS")] 
        [SerializeField] private EnemySpawnerAudioConfig _audioConfig;
        
        [Header("ENEMY WAVES")]
        [SerializeField] private EnemyWave[] _enemyWaves;
        
        private Transform _enemyAttackTarget;
        private HashSet<AEnemy> _activeEnemies;
        private bool AllCurrentWaveEnemiesAreDead => _activeEnemies.Count == 0;
        public delegate void EnemySpawnerEvent();

        public EnemySpawnerEvent OnFirstWaveStarted;
        public EnemySpawnerEvent OnAllWavesFinished;
        public EnemySpawnerEvent OnPlayerDiedDuringWaves;
        
        private IEnemyFactory _enemyFactory;
        private IEnemyHinterFactory _enemyHinterFactory;
        private IPlayerDeathNotifier _playerDeathNotifier;
        private IPlayerEnemySpawnersInteractions _playerEnemySpawnersInteractions;
        private bool _playerDiedDuringWaves;

        private IFMODAudioManager _audioManager;
        
        private void Start()
        {
            _enemyFactory = ServiceLocator.Instance.GetService<IEnemyFactory>();
            _enemyHinterFactory = ServiceLocator.Instance.GetService<IEnemyHinterFactory>();

            IGameReferences gameReferences = ServiceLocator.Instance.GetService<IGameReferences>();
            _playerDeathNotifier = gameReferences.GetPlayerDeathNotifier();
            _enemyAttackTarget = gameReferences.GetPlayerTargetForEnemies();
            _playerEnemySpawnersInteractions = gameReferences.GetPlayerEnemySpawnersInteractions();

            _audioManager = ServiceLocator.Instance.GetService<IFMODAudioManager>();
            
            _activeEnemies = new HashSet<AEnemy>(15);
        }

        public void StartWaves()
        {
            _playerDeathNotifier.AddDelegate(this);
            _playerDiedDuringWaves = false;
            
            _playerEnemySpawnersInteractions.OnSpawnerTrapActivated();
            _audioManager.PlayOneShotAttached(_audioConfig.EnemyWavesStart, gameObject);
            
            DoStartWaves().Forget();
        }

        private async UniTaskVoid DoStartWaves()
        {
            OnFirstWaveStarted?.Invoke();

            for (int waveI = 0; waveI < _enemyWaves.Length && !_playerDiedDuringWaves; ++waveI)
            {
                await SpawnEnemyWave(_enemyWaves[waveI]);
                await UniTask.WaitUntil(() => AllCurrentWaveEnemiesAreDead || _playerDiedDuringWaves);
            }

            FinishWaves();
        }

        private void FinishWaves()
        {
            _playerDeathNotifier.RemoveDelegate(this);

            if (_playerDiedDuringWaves)
            {
                ResetSpawnerOnPlayerDied();
            }
            else
            {
                OnAllWavesFinished?.Invoke();
                _audioManager.PlayOneShotAttached(_audioConfig.EnemyWavesCompleted, gameObject);
            }
        }

        private async UniTask SpawnEnemyWave(EnemyWave enemyWave)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(enemyWave.DelayBeforeWaveSpawning));
            
            for (int i = 0; i < enemyWave.SpawnSequence.Length; ++i)
            {
                EnemyWave.SpawnSequenceBeat spawnSequenceBeat = enemyWave.SpawnSequence[i];

                await UniTask.Delay(TimeSpan.FromSeconds(spawnSequenceBeat.DelayBeforeSpawn));

                SpawnHinter(spawnSequenceBeat.EnemyID, spawnSequenceBeat.SpawnPosition, out float extraWaitDuration);
                await UniTask.Delay(TimeSpan.FromSeconds(extraWaitDuration));
                
                SpawnEnemy(spawnSequenceBeat.EnemyID, spawnSequenceBeat.SpawnPosition);
            }
        }

        private void SpawnHinter(EnemyID enemyID, Vector3 spawnPosition, out float waitDuration)
        {
            EnemySpawnHinter hinter = _enemyHinterFactory.Create(spawnPosition, Quaternion.identity, enemyID, out waitDuration);
            _audioManager.PlayOneShotAttached(hinter.Sound, gameObject);
        }
        
        private void SpawnEnemy(EnemyID enemyID, Vector3 spawnPosition)
        {
            AEnemy enemy = _enemyFactory.Create(enemyID, spawnPosition, Quaternion.identity);
            enemy.AwakeInit(_enemyAttackTarget);

            enemy.OnDeathComplete += DecrementActiveEnemiesCount;

            _activeEnemies.Add(enemy);
        }
        
        
        
        private void DecrementActiveEnemiesCount(AEnemy destroyedEnemy)
        {
            destroyedEnemy.OnDeathComplete -= DecrementActiveEnemiesCount;
            _activeEnemies.Remove(destroyedEnemy);
        }

        
        public void OnPlayerDied()
        {
        }

        public void OnPlayerRespawnedFromDeath()
        {
            _playerDiedDuringWaves = true;
        }

        private void ResetSpawnerOnPlayerDied()
        {
            foreach (AEnemy enemy in _activeEnemies)
            {
                enemy.OnDeathComplete -= DecrementActiveEnemiesCount;
                enemy.DieFromOrder();
            }
            
            _activeEnemies.Clear();
            
            OnPlayerDiedDuringWaves?.Invoke();
        }
        
    }
}
