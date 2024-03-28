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
            public DamageHit DamageHit => new DamageHit(_damageHitConfig);
        }

        
        // ...
        
        
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
                _enemyTakeDamageParameters.DamageHit
                ));
        }
        
        // ...
    }
}