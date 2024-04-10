using NaughtyAttributes;
using Popeye.ProjectHelpers;
using Popeye.Scripts.TextUtilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Core.Services.InformationDisplay
{
    [CreateAssetMenu(fileName = "TextDisplayConfig_NAME", 
        menuName = ScriptableObjectsHelper.INFORMATIONDISPLAY_ASSETS_PATH + "TextDisplayConfig")]
    public class TextDisplayConfig : ScriptableObject
    {
        [Header("SETTINGS")] 
        [Expandable] [SerializeField] private TextDisplaySettings _settings;
        
        [Header("TEXTS")]
        [Expandable] [SerializeField] private TextContent _header;
        [Expandable] [SerializeField] private TextContent _description;

        [Header("VIEW")]
        [Expandable] [SerializeField] private DisplayViewExtras _backgroundViewExtras;
        [Expandable] [SerializeField] private DisplayViewExtras _contentViewExtras;
        
        
        public TextDisplaySettings TextDisplaySettings => _settings;
        public TextContent Header => _header;
        public TextContent Description => _description;
        public DisplayViewExtras BackgroundViewExtras => _backgroundViewExtras;        
        public DisplayViewExtras ContentViewExtras => _contentViewExtras;        
    }
}