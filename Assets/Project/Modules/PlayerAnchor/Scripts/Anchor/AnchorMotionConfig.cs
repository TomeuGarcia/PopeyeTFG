using Project.Scripts.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
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