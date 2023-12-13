using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    [System.Serializable]
    public class TrajectoryEndSpotView 
    {
        [SerializeField] private MeshRenderer _mesh;
        [SerializeField] private Color _validColor = Color.green;
        [SerializeField] private Color _notValidColor = Color.red;

        private Material _material;


        public void Configure()
        {
            _material = _mesh.material;
        }

        public void Show()
        {
            _mesh.gameObject.SetActive(true);
        }
        public void Hide()
        {
            _mesh.gameObject.SetActive(false);
        }
        
        public void SetValid(bool isValid)
        {
            _material.SetColor("_WaveColor", isValid ? _validColor : _notValidColor);
        }
    }
}