using UnityEngine;

namespace Popeye.Modules.Enemies.General
{
    public abstract class AEnemySpawnerInteractor : MonoBehaviour
    {
        [Header("ENEMY SPAWNER")] [SerializeField]
        private EnemySpawner _enemySpawner;

        private void OnEnable()
        {
            _enemySpawner.OnFirstWaveStarted += OnOnFirstEnemyWaveStartedEvent;
            _enemySpawner.OnAllWavesFinished += OnAllEnemyWavesFinishedEvent;
            _enemySpawner.OnPlayerDiedDuringWaves += OnPlayerDiedDuringWavesEvent;
        }

        private void OnDisable()
        {
            _enemySpawner.OnFirstWaveStarted -= OnOnFirstEnemyWaveStartedEvent;
            _enemySpawner.OnAllWavesFinished -= OnAllEnemyWavesFinishedEvent;
            _enemySpawner.OnPlayerDiedDuringWaves -= OnPlayerDiedDuringWavesEvent;
        }


        protected void StartEnemySpawnerWaves()
        {
            _enemySpawner.StartWaves();
        }

        protected abstract void OnOnFirstEnemyWaveStartedEvent();
        protected abstract void OnAllEnemyWavesFinishedEvent();
        protected abstract void OnPlayerDiedDuringWavesEvent();


    }
}
