using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "ClawAnchorSnapTargetViewConfig", 
        menuName = ScriptableObjectsHelper.SNAPTARGETS_ASSETS_PATH + "ClawAnchorSnapTargetViewConfig")]
    public class ClawAnchorSnapTargetViewConfig : ScriptableObject
    {
        [Header("OPEN")]
        [SerializeField] private TweenConfig _rotateOpenClaws;
        
        public TweenConfig RotateOpenClaws => _rotateOpenClaws;
        
        
        
        [Header("CLOSE")]
        [SerializeField] private TweenConfig _rotateCloseClaws;
        
        public TweenConfig RotateCloseClaws => _rotateCloseClaws;
        
        
        
        [Header("SNAP")]
        [SerializeField] private TweenPunchConfig _scalePunchSnapClawParent;
        
        public TweenPunchConfig ScalePunchSnapClawParent => _scalePunchSnapClawParent;
        
        
        
        [Header("HINTING")] 
        [SerializeField, Range(0.01f, 10.0f)] private float _hintingDelay = 1.5f;
        [SerializeField, Range(0.01f, 10.0f)] private float _hintingDuration = 2.5f;
        [SerializeField] private TweenPunchConfig _movePunchHintClawParent;
        [SerializeField] private TweenPunchConfig _rotatePunchHintClawParent;
        [SerializeField] private TweenPunchConfig _rotatePunchHintClaws;
        
        
        public float HintingDelay => _hintingDelay;
        public float HintingDuration => _hintingDuration;
        public TweenPunchConfig MovePunchHintClawParent => _movePunchHintClawParent;
        public TweenPunchConfig RotatePunchHintClawParent => _rotatePunchHintClawParent;
        public TweenPunchConfig RotatePunchHintClaws => _rotatePunchHintClaws;
        
        
        [Header("USED FOR DASH")]
        [SerializeField, Range(0.01f, 10.0f)] private float _usedForDashDelay = 0.5f;
        [SerializeField] private TweenPunchConfig _movePunchUsedClawParent;
        [SerializeField] private TweenPunchConfig _rotatePunchUsedClawParent;
        [SerializeField] private TweenPunchConfig _scalePunchUsedClawParent;
        [SerializeField] private TweenPunchConfig _rotatePunchUsedClaws;
        
        public float UsedForDashDelay => _usedForDashDelay;
        public TweenPunchConfig MovePunchUsedClawParent => _movePunchUsedClawParent;
        public TweenPunchConfig RotatePunchUsedClawParent => _rotatePunchUsedClawParent;
        public TweenPunchConfig ScalePunchUsedClawParent => _scalePunchUsedClawParent;
        public TweenPunchConfig RotatePunchUsedClaws => _rotatePunchUsedClaws;
    }
}