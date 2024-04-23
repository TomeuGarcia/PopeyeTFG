using System;
using System.Collections.Generic;
using Popeye.Modules.Enemies.General;
using Popeye.ProjectHelpers;
using Popeye.Scripts.EntityPlacing;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.Enemies.General
{
    [CreateAssetMenu(fileName = "WorldEnemyPlacerConfig", 
        menuName = ScriptableObjectsHelper.ENEMIES_ASSET_PATH + "WorldEnemyPlacerConfig")]
    public class WorldEnemyPlacerConfig : ScriptableObject
    {
        [SerializeField] private WorldEnemyPlacer.EditorViewElement[] _viewElements;
        [SerializeField] private WorldEntityPlacerViewData _missingEnemyIdToViewData;

        private Dictionary<EnemyID, WorldEntityPlacerViewData> _enemyIdToViewData;

        private void OnValidate()
        {
            if (_viewElements != null)
            {
                InitEnemyIdToViewData();
            }
        }

        private void Awake()
        {
            InitEnemyIdToViewData();
        }

        private void InitEnemyIdToViewData()
        {
            _enemyIdToViewData = 
                new Dictionary<EnemyID, WorldEntityPlacerViewData>(_viewElements.Length);

            foreach ( WorldEnemyPlacer.EditorViewElement editorViewElement in _viewElements)
            {
                _enemyIdToViewData.Add(editorViewElement.EnemyID, editorViewElement.ViewData);
            }
        }

        public WorldEntityPlacerViewData GetViewDataForEnemy(EnemyID enemyID)
        {
            if (!_enemyIdToViewData.TryGetValue(enemyID, out var viewData))
            {
                return _missingEnemyIdToViewData;
            }

            return viewData;
        }
        
    }
}