using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorKickConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorKickConfig")]
    public class AnchorKickConfig : ScriptableObject
    {
        [Header("DURATIONS")]
        [SerializeField, Range(0.01f, 10.0f)] private float _anchorKickMoveDuration = 0.5f;
        
        public float AnchorKickMoveDuration => _anchorKickMoveDuration;
        
        
        [Header("DISTANCES")]
        [SerializeField, Range(0.0f, 20.0f)] private float _anchorKickMoveDistance = 7.0f;

        public float AnchorKickMoveDistance => _anchorKickMoveDistance;


        [Header("MOVEMENT")] 
        [SerializeField] private AnimationCurve _moveInterpolationCurve;
        public AnimationCurve MoveInterpolationCurve => _moveInterpolationCurve;
        
        
        [Header("TRAJECTORY OFFSET")]
        [SerializeField] private AnimationCurve _heightDisplacementCurve;
        public AnimationCurve HeightDisplacementCurve => _heightDisplacementCurve;
    }
}