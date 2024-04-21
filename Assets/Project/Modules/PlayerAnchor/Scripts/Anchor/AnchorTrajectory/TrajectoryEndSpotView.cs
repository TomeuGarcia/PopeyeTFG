using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [System.Serializable]
    public class TrajectoryEndSpotView 
    {
        [Header("GENERIC")]
        [SerializeField] private Color _validColor = Color.green;
        [SerializeField] private Color _notValidColor = Color.red;

        [Header("SPOT")]
        [SerializeField] private MeshRenderer _mesh;
        private Material _spotMaterial;
        private int _waveColorParameterId;

        [Header("TRAJECTORY")]
        [SerializeField] private Material _trajectoryMaterial;
        private int _tipPositionParameterId;
        private int _tipIsVisibleParameterId;
        

        public void Configure()
        {
            _spotMaterial = _mesh.material;

            _waveColorParameterId = Shader.PropertyToID("_WaveColor");
            _tipPositionParameterId = Shader.PropertyToID("_TipPosition");
            _tipIsVisibleParameterId = Shader.PropertyToID("_TipIsVisible");
            
            _trajectoryMaterial.SetColor(_waveColorParameterId, _validColor);
            _trajectoryMaterial.SetColor(Shader.PropertyToID("_TipColor"), _notValidColor);
        }
        public void Finish()
        {
            _trajectoryMaterial.SetVector(_tipPositionParameterId, Vector3.zero);
            _trajectoryMaterial.SetFloat(_tipIsVisibleParameterId, 0);
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
            _spotMaterial.SetColor(_waveColorParameterId, isValid ? _validColor : _notValidColor);
            _trajectoryMaterial.SetFloat(_tipIsVisibleParameterId, isValid ? 0 : 1);
        }

        public void SetTrajectoryContactPosition(Vector3 contactPosition)
        {
            _trajectoryMaterial.SetVector(_tipPositionParameterId, contactPosition);

        }
        
        
    }
}