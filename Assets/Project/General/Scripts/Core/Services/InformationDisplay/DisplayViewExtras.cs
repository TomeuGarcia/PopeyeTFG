using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Core.Services.InformationDisplay
{
    [CreateAssetMenu(fileName = "DisplayViewExtras_NAME", 
        menuName = ScriptableObjectsHelper.INFORMATIONDISPLAY_ASSETS_PATH + "DisplayViewExtras")]
    public class DisplayViewExtras : ScriptableObject
    {        
        [SerializeField] private TweenFadeConfig _showFade;
        [SerializeField] private TweenFadeConfig _hideFade;
        
        public TweenFadeConfig ShowFade => _showFade;
        public TweenFadeConfig HideFade => _hideFade;
    }
    
}