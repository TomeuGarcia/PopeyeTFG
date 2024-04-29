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
        [SerializeField, Range(0, 10)] private int _minimumNumberOfPlays = 1;
        public VideoClip VideoClip => _videoClip;
        public int MinimumNumberOfPlays => _minimumNumberOfPlays;
        
        
        [Header("VIEW")]
        [Expandable] [SerializeField] private DisplayViewExtras _backgroundViewExtras;

        public DisplayViewExtras BackgroundViewExtras => _backgroundViewExtras;        
    }
}