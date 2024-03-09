using System;
using AYellowpaper;
using Popeye.ProjectHelpers;
using Popeye.Scripts.EditorUtilities;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldBuilders
{
    
    [CreateAssetMenu(fileName = "WallBuilderConfig_NAME", 
        menuName = ScriptableObjectsHelper.WALLBUILDER_ASSETS_PATH + "WallBuilderConfig")]
    public class WallBuilderConfig : ScriptableObject
    {
        [System.Serializable]
        public class EditorViewConfig
        {
            [SerializeField] private Color _cornerButtonColor = Color.blue;
            [SerializeField] private Color _cornerBlockColor = Color.blue;
            [SerializeField] private Color _fillBlockColor = Color.yellow;
            [SerializeField] private Color _fillLineColor = Color.red;
            
            public Color CornerButtonColor => _cornerButtonColor;
            public Color CornerBlockColor => _cornerBlockColor;
            public Color FillBlockColor => _fillBlockColor;
            public Color FillLineColor => _fillLineColor;

            [SerializeField, Range(0.01f, 10.0f)] private float _lineThickness = 2.5f;
            public float LineThickness => _lineThickness;
        }


        [Header("DATA TRACKING")]
        [SerializeField] private InterfaceReference<IWallBuilderDataTracker, ScriptableObject> _wallBuilderDataTracker;
        public IWallBuilderDataTracker WallBuilderDataTracker => _wallBuilderDataTracker.Value;
        
        
        [Header("VIEW")]
        [SerializeField] private EditorViewConfig _editorView;
        [SerializeField] private DistanceFromCameraTransparencyConfig _transparencyConfig;

        public EditorViewConfig EditorView => _editorView;
        public DistanceFromCameraTransparencyConfig TransparencyConfig => _transparencyConfig;
        
        
        
        [Header("BLOCKS")]
        [SerializeField] private WallBuilder.Block _cornerBlock;
        [SerializeField] private WallBuilder.Block _fillBlock;
        public WallBuilder.Block CornerBlock => _cornerBlock;
        public WallBuilder.Block FillBlock => _fillBlock;


        [Header("PREFABS")] 
        [SerializeField] private GameObject _cornerBlockPrefab;
        [SerializeField] private GameObject _fillBlockPrefab;
        
        public GameObject CornerBlockPrefab => _cornerBlockPrefab;
        public GameObject FillBlockPrefab => _fillBlockPrefab;


        [Header("COLLIDERS")] 
        [SerializeField, Range(0.0f, 10.0f)] private float _colliderWidth = 1.0f; 
        [SerializeField, Range(0.0f, 10.0f)] private float _colliderHeight = 2.0f; 
        
        public float ColliderWidth => _colliderWidth;
        public float ColliderHeight => _colliderHeight;
        public float HalfColliderHeight { get; private set; }


        [Header("MATERIAL")] 
        [SerializeField] private Material _fakeMeshMaterial;
        public Material FakeMeshMaterial => _fakeMeshMaterial;
        

        private void OnValidate()
        {
            _cornerBlock.UpdateHalfSize();
            _fillBlock.UpdateHalfSize();

            HalfColliderHeight = ColliderHeight / 2;
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}