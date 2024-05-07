using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
    [CreateAssetMenu(fileName = "BarrierViewConfig", 
        menuName = ScriptableObjectsHelper.WORLDELEMENTS_ASSETS_PATH + "BarrierViewConfig")]
    public class StoneBarrierViewConfig : ScriptableObject
    {
        [SerializeField] private bool _startsOn = false;
        [SerializeField] private TweenEaseConfig _activationEase;
        [SerializeField] private string _activationAnimationProperty = "_OpenAnimationT";
        
        public bool StartsOn => _startsOn;
        public TweenEaseConfig ActivationEase => _activationEase;
        public string ActivationAnimationProperty => _activationAnimationProperty;
    }
}