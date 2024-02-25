using System;
using NaughtyAttributes;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Enemies;
using Popeye.Modules.Enemies.EnemyFactories;
using Popeye.Modules.Enemies.General;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Project.Modules.Enemies.General
{
    [ExecuteInEditMode]
    public class WorldEnemyPlacer : MonoBehaviour
    {
        [System.Serializable]
        public class EditorViewElement
        {
            [System.Serializable]
            public class ViewData
            {
                [SerializeField] private Mesh _enemyMesh;
                [SerializeField] private Material _enemyMaterial;
                [SerializeField] private float _meshSize = 1f;
                [SerializeField] private string _viewName = "Preview_ENEMY-NAME";

                public Mesh EnemyMesh => _enemyMesh;
                public Material EnemyMaterial => _enemyMaterial;
                public float MeshSize => _meshSize;
                public string ViewName => _viewName;
            }
            
            [SerializeField] private EnemyID _enemyID;
            [SerializeField] private ViewData _viewData;
            
            public EnemyID EnemyID => _enemyID;
            public ViewData Data => _viewData;

        }
        

        [Header("ENEMY")]
        [Required] [SerializeField] private EnemyID _enemyID;
        
        [Header("CONFIGURATION")]
        [Required] [Expandable] [SerializeField] private WorldEnemyPlacerConfig _config;
        
        [Header("REFERENCES")]
        [Required] [SerializeField] private GameObject _viewGameObject;
        [Required] [SerializeField] private MeshFilter _meshFilter;
        [Required] [SerializeField] private MeshRenderer _meshRenderer;

        private Vector3 SpawnPosition => transform.position;
        
        
        
        private void Awake()
        {
            Destroy(_viewGameObject);
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            IEnemyFactory enemyFactory = ServiceLocator.Instance.GetService<IEnemyFactory>();
            AEnemy enemy = enemyFactory.Create(_enemyID, SpawnPosition, Quaternion.identity);
            
            // TODO: give waypoints to the enemy
        }

        
#if UNITY_EDITOR
        private void Update()
        {
            if (!EditorApplication.isPlaying)
            {
                if (_enemyID && _config && _viewGameObject && _meshFilter && _meshRenderer)
                {
                    UpdateEditorView();
                }
            }
        }
        
        private void UpdateEditorView()
        {
            EditorViewElement.ViewData viewData = _config.GetViewDataForEnemy(_enemyID);
            _viewGameObject.transform.localScale = Vector3.one * viewData.MeshSize;
            _viewGameObject.name = viewData.ViewName;
            _meshFilter.mesh = viewData.EnemyMesh;
            _meshRenderer.material = viewData.EnemyMaterial;
        }

#endif
        
    }
}