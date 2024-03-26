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
            [SerializeField] private Color _cornerBlockColor = Color.blue;
            [SerializeField] private Color _fillBlockColor = Color.yellow;
            [SerializeField] private Color _fillLineColor = Color.red;
            [SerializeField] private Color _buttonColor = Color.magenta;
            [SerializeField] private Color _textColor = Color.red;
            [SerializeField] private Texture2D _textBackground;
            
            
            public Color CornerBlockColor => _cornerBlockColor;
            public Color FillBlockColor => _fillBlockColor;
            public Color FillLineColor => _fillLineColor;
            public Color ButtonColor => _buttonColor;
            public Color TextColor => _textColor;
            public Texture2D TextBackground => _textBackground;

            [SerializeField, Range(0.01f, 10.0f)] private float _lineThickness = 2.5f;
            public float LineThickness => _lineThickness;
            
            [SerializeField, Range(0.01f, 10.0f)] private float _buttonSize = 0.2f;
            public float ButtonSize => _buttonSize;
            
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

        
        
        [Header("EXTRUDING")] 
        [SerializeField, Range(0f, 5.0f)] private float _cornerExtrudeDistance = 1.0f; 
        [SerializeField] private Vector3 _extrudePositiveDirection = Vector3.up;
        public Vector3 ExtrudePositiveOffset => _extrudePositiveDirection * _cornerExtrudeDistance;
        public Vector3 ExtrudeNegativeOffset => -ExtrudePositiveOffset;

        

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