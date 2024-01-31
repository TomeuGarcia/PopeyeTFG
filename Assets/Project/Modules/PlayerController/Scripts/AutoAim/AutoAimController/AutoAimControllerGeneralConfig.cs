using System;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerController.AutoAim
{
    
    [CreateAssetMenu(fileName = "AutoAimControllerGeneralConfig", 
        menuName = ScriptableObjectsHelper.AUTOAIM_ASSETS_PATH + "AutoAimControllerGeneralConfig")]
    public class AutoAimControllerGeneralConfig : ScriptableObject
    {
        [SerializeField] private AutoAimFunctionConfig _functionConfig;
        
        [Space(30)]
        [SerializeField] private AutoAimTargetFilterConfig _targetFilterConfig;
        
        [Space(30)]
        [SerializeField] private AutoAimTargetFinderConfig _targetFinderConfig;
        
        [Space(30)]
        [SerializeField] private AutoAimTargetResultFiltererConfig _targetResultFiltererConfig;
        
        [Space(30)]
        [SerializeField] private CollisionProbingConfig _collisionProbingConfig;

        
        
        public AutoAimFunctionConfig FunctionConfig => _functionConfig;
        public AutoAimTargetFilterConfig TargetFilterConfig => _targetFilterConfig;
        public AutoAimTargetFinderConfig TargetFinderConfig => _targetFinderConfig;
        public AutoAimTargetResultFiltererConfig TargetResultFiltererConfig => _targetResultFiltererConfig;
        public CollisionProbingConfig CollisionProbingConfig => _collisionProbingConfig;
        
        
        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            _functionConfig?.OnValidate();
            _targetFilterConfig?.OnValidate();
            _targetFinderConfig?.OnValidate();
            _targetResultFiltererConfig?.OnValidate();
        }
    }
}