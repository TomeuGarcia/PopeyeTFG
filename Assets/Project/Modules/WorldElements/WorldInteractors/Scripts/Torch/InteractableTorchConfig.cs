using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
    [CreateAssetMenu(fileName = "InteractableTorchConfig", 
        menuName = ScriptableObjectsHelper.WORLDELEMENTS_ASSETS_PATH + "InteractableTorchConfig")]
    public class InteractableTorchConfig : ScriptableObject
    {
        [SerializeField] private TweenEaseConfig _lightOnEase;
        [SerializeField] private TweenEaseConfig _lightOffEase;
        
        public TweenEaseConfig LightOnEase => _lightOnEase;
        public TweenEaseConfig LightOffEase => _lightOffEase;
    }
}