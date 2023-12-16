using Project.Scripts.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    
    [CreateAssetMenu(fileName = "AnchorSpinConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorSpinConfig")]
    public class AnchorSpinConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 20.0f)] private float _spinRadius = 5.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _spinMaxSpeed = 1.0f;
        
        [SerializeField, Range(0.01f, 10.0f)] private float _spinReadyDuration = 0.3f;

        public float SpinRadius => _spinRadius;
        public float SpinMaxSpeed => _spinMaxSpeed;
        public float SpinReadyDuration => _spinReadyDuration;
    }
}