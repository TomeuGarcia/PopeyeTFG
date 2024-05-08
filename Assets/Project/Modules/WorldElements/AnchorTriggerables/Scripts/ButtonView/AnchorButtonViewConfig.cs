using System;
using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.WorldElements.AnchorTriggerables
{
    [CreateAssetMenu(fileName = "AnchorButtonViewConfig", 
        menuName = ScriptableObjectsHelper.WORLDELEMENTS_ASSETS_PATH + "AnchorButtonViewConfig")]
    public class AnchorButtonViewConfig : ScriptableObject
    {
        [SerializeField] private string _isTriggeredProperty = "_IsTriggered";
        [SerializeField] private TweenConfig _triggeredMove;
        [SerializeField] private TweenConfig _triggeredRotate;
        [SerializeField] private TweenPunchConfig _triggeredPunch;
        
        public int IsTriggeredPropertyId { get; private set; }
        public TweenConfig TriggeredMove => _triggeredMove;
        public TweenConfig TriggeredRotate => _triggeredRotate;
        public TweenPunchConfig TriggeredPunch => _triggeredPunch;


        private void OnValidate()
        {
            IsTriggeredPropertyId = Shader.PropertyToID(_isTriggeredProperty);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}