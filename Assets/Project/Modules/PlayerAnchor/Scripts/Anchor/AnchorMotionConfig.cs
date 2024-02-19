using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorMotionConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorMotionConfig")]
    public class AnchorMotionConfig : ScriptableObject
    {
        [Header("ROTATIONS")] 
        [SerializeField] private Vector3 _carriedAnchorRotation;
        [SerializeField] private Vector3 _grabbedToThrowAnchorRotation;
        
        public Quaternion CarriedAnchorRotation { get; private set; }
        public Quaternion GrabbedToThrowAnchorRotation { get; private set; }

        
        [SerializeField, Range(0.01f, 2.0f)] private float _maxCarriedDuration = 0.15f;

        public float MaxCarriedDuration => _maxCarriedDuration;
        
        private void OnValidate()
        {
            CarriedAnchorRotation = Quaternion.Euler(_carriedAnchorRotation);
            GrabbedToThrowAnchorRotation = Quaternion.Euler(_grabbedToThrowAnchorRotation);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}