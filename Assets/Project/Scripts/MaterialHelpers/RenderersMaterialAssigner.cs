using UnityEngine;

namespace Popeye.Scripts.MaterialHelpers
{
    public class RenderersMaterialAssigner : MonoBehaviour
    {
        [Header("MATERIAL")]
        [SerializeField] private Material _materialToAssign;

        [Header("RENDERERS")] 
        [SerializeField] private Renderer[] _renderers;
        
        

        public Material AssignToRenderersAndGetMaterial()
        {
            Material material = new Material(_materialToAssign);

            foreach (Renderer renderer in _renderers)
            {
                renderer.material = material;
            }
            
            return material;
        }
        
    }
}