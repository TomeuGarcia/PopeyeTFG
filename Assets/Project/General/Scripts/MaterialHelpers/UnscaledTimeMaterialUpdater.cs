using UnityEngine;

namespace Popeye.Scripts.MaterialHelpers
{
    public class UnscaledTimeMaterialUpdater : MonoBehaviour
    {
        [SerializeField] private Material[] _sharedMaterials;
        
        private int _unscaledTimePropertyId;
        
        private void Awake()
        {
            _unscaledTimePropertyId = Shader.PropertyToID("_UnscaledTime");
        }

        private void Update()
        {
            foreach (Material sharedMaterial in _sharedMaterials)
            {
                sharedMaterial.SetFloat(_unscaledTimePropertyId, Time.unscaledTime);
            }
        }
        
    }
}