using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.MovableBlocks.GridMovement
{
    
    [CreateAssetMenu(fileName = "GridMovementAreaViewConfig", 
        menuName = ScriptableObjectsHelper.GRIDMOVEMENT_ASSETS_PATH + "GridMovementAreaViewConfig")]
    public class GridMovementAreaViewConfig : ScriptableObject
    {
        [SerializeField] private GameObject _quadMeshPrefab;
        [SerializeField] private Material _areaMaterial;
        

        public GameObject QuadMeshPrefab => _quadMeshPrefab;
        public Material AreaMaterial => _areaMaterial;
    }
}