using UnityEngine;

namespace Popeye.Scripts.EntityPlacing
{
    [System.Serializable]
    public class WorldEntityPlacerViewData
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;
        [SerializeField] private float _meshSize = 1f;
        [SerializeField] private string _viewName = "Preview_ENTITY-NAME";

        public Mesh Mesh => _mesh;
        public Material Material => _material;
        public float MeshSize => _meshSize;
        public string ViewName => _viewName;
    }
}