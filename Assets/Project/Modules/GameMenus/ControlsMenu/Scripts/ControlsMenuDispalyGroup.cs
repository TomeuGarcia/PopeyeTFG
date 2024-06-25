using Popeye.Core.Services.InformationDisplay;
using Popeye.Modules.GameMenus.Generic;
using Popeye.Scripts.TextUtilities;
using TMPro;
using UnityEngine;

namespace Popeye.Modules.GameMenus.OptionsMenu
{
    public class ControlsMenuDispalyGroup : MonoBehaviour
    {
        [Header("BUTTON")]
        [SerializeField] private SmartButtonAndConfig _showButton;

        [Header("DISPLAY")]
        [SerializeField] private TextContent _descriptionText;
        [SerializeField] private VideoDisplayConfig _video;

        private TextInitializer _textInitializer;
        private IVideoDisplayer _videoDisplayer;
        

        public void Init(TextInitializer textInitializer, IVideoDisplayer videoDisplayer)
        {
            _textInitializer = textInitializer;
            _videoDisplayer = videoDisplayer;
        
            _showButton.SmartButton.Init(_showButton.Config, ShowControls);
        }

        public void ShowControls()
        {
            _textInitializer.SetTextContent(_descriptionText);

            if (_video != null)
            {
                _videoDisplayer.StartShowing(_video);
            }            
        }
    }
}