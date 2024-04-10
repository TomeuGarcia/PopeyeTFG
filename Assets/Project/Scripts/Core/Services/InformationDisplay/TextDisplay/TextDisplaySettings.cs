using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Core.Services.InformationDisplay
{
    [CreateAssetMenu(fileName = "TextDisplayType_NAME", 
        menuName = ScriptableObjectsHelper.INFORMATIONDISPLAY_ASSETS_PATH + "TextDisplayType")]
    public class TextDisplaySettings : ScriptableObject
    {
        [Header("COLORS")]
        [SerializeField] private Color _headerColor = Color.cyan;
        [SerializeField] private Color _descriptionColor = Color.white;

        [Header("SOUND")]
        [SerializeField] private OneShotFMODSound _sound;
        
        
        public Color HeaderColor => _headerColor;
        public Color DescriptionColor => _descriptionColor;
        public OneShotFMODSound Sound => _sound;
        
        
    }
}