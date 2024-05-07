using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    public class AnchorButtonView : MonoBehaviour
    {
        [Header("CONFIG")] 
        [SerializeField] private AnchorButtonViewConfig _config;
    
        [Header("COMPONENTS")]
        [SerializeField] private Transform _buttonTransform;
        [SerializeField] private MeshRenderer[] _buttonMeshes;
        private Material _buttonMaterial;


        private void Awake()
        {
            _buttonMaterial = _buttonMeshes[0].material;
            for (int i = 1; i < _buttonMeshes.Length; ++i)
            {
                _buttonMeshes[i].material = _buttonMaterial;
            }
            
            UpdateIsTriggeredView(false);
        }


        public void PlayTriggeredAnimation()
        {
            UpdateIsTriggeredView(true);
            _buttonTransform.BlendableLocalMoveBy(_config.TriggeredMove);
            _buttonTransform.RotateBy(_config.TriggeredRotate);
            _buttonTransform.PunchScale(_config.TriggeredPunch);
        }
        public void PlayNotTriggeredAnimation()
        {
            UpdateIsTriggeredView(false);
            _buttonTransform.BlendableLocalMoveBy(_config.TriggeredMove.Undo());
            _buttonTransform.RotateBy(_config.TriggeredRotate.Undo());
        }
        
        private void UpdateIsTriggeredView(bool isTriggered)
        {
            _buttonMaterial.SetFloat(_config.IsTriggeredPropertyId, isTriggered ? 1 : 0);
        }
    }
}