using System;
using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "HookAnimatorViewConfig_NAME", 
        menuName = ScriptableObjectsHelper.SNAPTARGETS_ASSETS_PATH + "HookAnimatorViewConfig")]
    public class HookAnimatorViewConfig : ScriptableObject
    {
        [Header("ANIMATOR")]
        [SerializeField] private string _aimedParameter = "b_IsAimed";
        [SerializeField] private string _grabbedParameter = "b_IsGrabbed";
        [SerializeField] private string _pulledParameter = "t_Pulled";
        
        public int AimedParameter { get; private set; }
        public int GrabbedParameter { get; private set; }
        public int PulledParameter { get; private set; }


        [Header("TWEENS")] 
        [SerializeField] private TweenPunchConfig _startGrabbingScalePunch;
        [SerializeField] private TweenPunchConfig _stopGrabbingScalePunch;
        public TweenPunchConfig StartGrabbingScalePunch => _startGrabbingScalePunch;
        public TweenPunchConfig StopGrabbingScalePunch => _stopGrabbingScalePunch;
        

        private void OnValidate()
        {
            AimedParameter = Animator.StringToHash(_aimedParameter);
            GrabbedParameter = Animator.StringToHash(_grabbedParameter);
            PulledParameter = Animator.StringToHash(_pulledParameter);
        }

        private void OnEnable()
        {
            OnValidate();
        }
        
    }
}