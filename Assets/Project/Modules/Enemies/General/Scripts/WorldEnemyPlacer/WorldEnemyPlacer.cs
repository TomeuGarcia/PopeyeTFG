using System;
using AYellowpaper;
using NaughtyAttributes;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies;
using Popeye.Modules.Enemies.EnemyFactories;
using Popeye.Modules.Enemies.General;
using Popeye.Scripts.EntityPlacing;
using UnityEngine;


namespace Project.Modules.Enemies.General
{
    public class WorldEnemyPlacer : MonoBehaviour
    {
        [System.Serializable]
        public class EditorViewElement
        {
            [SerializeField] private EnemyID _enemyID;
            [SerializeField] private WorldEntityPlacerViewData _entityViewData;
            
            public EnemyID EnemyID => _enemyID;
            public WorldEntityPlacerViewData ViewData => _entityViewData;

        }
        

        [Header("ENEMY")]
        [Required] [SerializeField] private EnemyID _enemyID;
        
        [Header("CONFIGURATION")]
        [Required] [Expandable] [SerializeField] private WorldEnemyPlacerConfig _config;

        [Header("REFERENCES")] 
        [SerializeField] private InterfaceReference<IEnemyWaypointsInitializer, MonoBehaviour> _enemyWaypointsInitializer;

        [SerializeField] private WorldEntityPreviewComponents _enemyPreview;
        
        private Vector3 SpawnPosition => transform.position;

        private void OnValidate()
        {
            if (_enemyID && _config && _enemyPreview.HasAllReferences())
            {
                _enemyPreview.UpdateView(_config.GetViewDataForEnemy(_enemyID));
            }
        }


        private void Awake()
        {
            _enemyPreview.DestroyView();
        }

        private void Start()
        {
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            IEnemyFactory enemyFactory = ServiceLocator.Instance.GetService<IEnemyFactory>();
            
            AEnemy enemy = enemyFactory.Create(_enemyID, SpawnPosition, Quaternion.identity);
            _enemyWaypointsInitializer.Value.SetEnemyWaypoints(enemy);
        }
        
        
    }
}