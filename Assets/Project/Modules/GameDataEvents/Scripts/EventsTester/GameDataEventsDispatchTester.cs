using NaughtyAttributes;
using Popeye.Core.Services.EventSystem;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.Enemies.General;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.GameDataEvents
{
    [CreateAssetMenu(fileName = "GameDataEvents_DispatchTester", 
        menuName = ScriptableObjectsHelper.GAMEDATAEVENTS_ASSETS_PATH + "DispatchTester")]
    public class GameDataEventsDispatchTester : ScriptableObject
    {
        private IEventSystemService _eventSystemService;
        
        
        
        [Header("ENEMY SEES PLAYER")] 
        [SerializeField] private EnemySeesPlayerParameters _enemySeesPlayerParameters;
        [System.Serializable]
        public class EnemySeesPlayerParameters
        {
            [SerializeField] private EnemyID _id;
            public EnemyID Id => _id;
        }

        
        [Header("ENEMY TAKE DAMAGE")] 
        [SerializeField] private EnemyTakeDamageParameters _enemyTakeDamageParameters;
        [System.Serializable]
        public class EnemyTakeDamageParameters
        {
            [SerializeField] private EnemyID _id;
            [SerializeField] private Vector3 _position;
            [SerializeField] private DamageHitConfig _damageHitConfig;
            public EnemyID Id => _id;
            public Vector3 Position => _position;
            public DamageHitResult DamageHitResult => 
                new DamageHitResult(
                    null, null, 
                    new DamageHit(_damageHitConfig),
                    _damageHitConfig.Damage, _position);
        }
        
        
        [Header("ENEMY WAVE START")] 
        [SerializeField] private EnemyWaveStartParameters _enemyWaveStartParameters;
        [System.Serializable]
        public class EnemyWaveStartParameters
        {
        }
        
        
        [Header("ENEMY WAVES COMPLETED")] 
        [SerializeField] private EnemyWavesCompletedParameters _enemyWavesCompletedParameters;
        [System.Serializable]
        public class EnemyWavesCompletedParameters
        {
        }

        
        [Header("PLAYER ACTION")] 
        [SerializeField] private PlayerActionParameters _playerActionParameters;
        [System.Serializable]
        public class PlayerActionParameters
        {
            [SerializeField] private string _actionName;
            [SerializeField] private Vector3 _position;
            public string ActionName => _actionName;
            public Vector3 Position => _position;
        }
        
        [Header("PLAYER UPDATE")] 
        [SerializeField] private PlayerUpdateParameters _playerUpdateParameters;
        [System.Serializable]
        public class PlayerUpdateParameters
        {
            [SerializeField] private Vector3 _position;
            public Vector3 Position => _position;
        }
        
        [Header("PLAYER HEAL")] 
        [SerializeField] private PlayerHealParameters _playerHealParameters;
        [System.Serializable]
        public class PlayerHealParameters
        {
            [SerializeField] private Vector3 _position;
            [SerializeField] private int _healthBeforeHealing;
            [SerializeField] private int _healthAfterHealing;
            public Vector3 Position => _position;
            public int CurrentHealth => _healthAfterHealing;
            public int HealthBeforeHealing => _healthBeforeHealing;
        }
        
        [Header("PLAYER TAKE DAMAGE")] 
        [SerializeField] private PlayerTakeDamageParameters _playerTakeDamageParameters;
        [System.Serializable]
        public class PlayerTakeDamageParameters
        {
            [SerializeField] private Vector3 _position;
            [SerializeField] private DamageHitConfig _damageHitConfig;
            [SerializeField] private int _healthAfterTakingDamage;
            public Vector3 Position => _position;
            public int CurrentHealth => _healthAfterTakingDamage;
            public DamageHitResult DamageHitResult => 
                new DamageHitResult(
                    null, null, 
                    new DamageHit(_damageHitConfig),
                    _damageHitConfig.Damage, _position);
        }

        
        // ...
        
        
        [Space(30)]
        [SerializeField] private string _; // Just to separate buttons
        
        
        
        public void Init(IEventSystemService eventSystemService)
        {
            _eventSystemService = eventSystemService;
        }

        
        [Button()]
        private void InvokeOnEnemySeesPlayer()
        {
            _eventSystemService.Dispatch(new OnEnemySeesPlayerEvent(
                _enemySeesPlayerParameters.Id
            ));
        }
        
        [Button()]
        private void InvokeOnEnemyTakeDamage()
        {
            _eventSystemService.Dispatch(new OnEnemyTakeDamageEvent(
                _enemyTakeDamageParameters.Id,
                _enemyTakeDamageParameters.Position, 
                _enemyTakeDamageParameters.DamageHitResult
                ));
        }
        
        [Button()]
        private void InvokeOnEnemyWaveStart()
        {
            _eventSystemService.Dispatch(new OnEnemyWaveStartEvent());
        }
        [Button()]
        private void InvokeOnEnemyWavesCompletedStart()
        {
            _eventSystemService.Dispatch(new OnAllEnemyWavesCompletedEvent());
        }
        
        
        [Button()]
        private void InvokePlayerAction()
        {
            _eventSystemService.Dispatch(new OnPlayerActionEvent(
                _playerActionParameters.Position,
                _playerActionParameters.ActionName 
                ));
        }
        
        [Button()]
        private void InvokePlayerUpdate()
        {
            _eventSystemService.Dispatch(new OnPlayerUpdateEvent(
                _playerUpdateParameters.Position
                ));
        }
        
        [Button()]
        private void InvokePlayerHeal()
        {
            _eventSystemService.Dispatch(new OnPlayerHealEvent(
                _playerHealParameters.Position,
                _playerHealParameters.CurrentHealth,
                _playerHealParameters.HealthBeforeHealing
                ));
        }
        
        [Button()]
        private void InvokePlayerTakeDamage()
        {
            _eventSystemService.Dispatch(new OnPlayerTakeDamageEvent(
                _playerTakeDamageParameters.Position,
                _playerTakeDamageParameters.DamageHitResult,
                _playerTakeDamageParameters.CurrentHealth
                ));
        }

        
        // ...
    }
}