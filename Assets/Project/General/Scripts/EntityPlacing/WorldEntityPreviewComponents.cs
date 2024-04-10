using NaughtyAttributes;
using UnityEngine;

namespace Popeye.Scripts.EntityPlacing
{
    [System.Serializable]
    public class WorldEntityPreviewComponents
    {
        [Required] [SerializeField] private GameObject _viewGameObject;
        [Required] [SerializeField] private MeshFilter _meshFilter;
        [Required] [SerializeField] private MeshRenderer _meshRenderer;

        public Transform PlaceTransform => _viewGameObject.transform;

        public bool HasAllReferences()
        {
            return _viewGameObject && _meshFilter && _meshRenderer;
        }
        
        public void UpdateView(WorldEntityPlacerViewData viewData)
        {
            _viewGameObject.transform.localScale = Vector3.one * viewData.MeshSize;
            _viewGameObject.name = viewData.ViewName;
            _meshFilter.mesh = viewData.Mesh;
            _meshRenderer.material = viewData.Material;
        }

        public void DestroyView()
        {
            GameObject.Destroy(_viewGameObject);
        }
        
    }
}