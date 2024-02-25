using System;
using System.Collections.Generic;
using Popeye.Modules.Enemies.General;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.Enemies.General
{
    [CreateAssetMenu(fileName = "WorldEnemyPlacerConfig", 
        menuName = ScriptableObjectsHelper.ENEMIES_ASSET_PATH + "WorldEnemyPlacerConfig")]
    public class WorldEnemyPlacerConfig : ScriptableObject
    {
        [SerializeField] private WorldEnemyPlacer.EditorViewElement[] _viewElements;

        private Dictionary<EnemyID, WorldEnemyPlacer.EditorViewElement.ViewData> _enemyIdToViewData;


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
                new Dictionary<EnemyID, WorldEnemyPlacer.EditorViewElement.ViewData>(_viewElements.Length);

            foreach ( WorldEnemyPlacer.EditorViewElement editorViewElement in _viewElements)
            {
                _enemyIdToViewData.Add(editorViewElement.EnemyID, editorViewElement.Data);
            }
        }

        public WorldEnemyPlacer.EditorViewElement.ViewData GetViewDataForEnemy(EnemyID enemyID)
        {
            if (!_enemyIdToViewData.TryGetValue(enemyID, out var viewData))
            {
                throw new Exception($"No ViewData for {enemyID.name}");
            }

            return viewData;
        }
        
    }
}