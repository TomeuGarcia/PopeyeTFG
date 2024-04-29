using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Video;

namespace Popeye.Core.Services.InformationDisplay
{
    [CreateAssetMenu(fileName = "VideoDisplayConfig_NAME", 
        menuName = ScriptableObjectsHelper.INFORMATIONDISPLAY_ASSETS_PATH + "VideoDisplayConfig")]
    public class VideoDisplayConfig : ScriptableObject
    {
        [SerializeField] private VideoClip _videoClip;
        public VideoClip VideoClip => _videoClip;
        
        
        [Header("VIEW")]
        [Expandable] [SerializeField] private DisplayViewExtras _backgroundViewExtras;

        public DisplayViewExtras BackgroundViewExtras => _backgroundViewExtras;        
    }
}