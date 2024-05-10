using System;
using AYellowpaper;
using Popeye.Core.Services.InformationDisplay;
using Popeye.Modules.GameMenus.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Popeye.Modules.GameMenus.OptionsMenu
{
    public class ControlsMenuController : AMenuController
    {
        [Header("VIDEO DISPLAYER")] 
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private InterfaceReference<IVideoDisplayer, MonoBehaviour> _videoDisplayer;
        
        private IVideoDisplayer VideoDisplayer => _videoDisplayer.Value;

        [Header("CONTROLS")] 
        [SerializeField] private ControlsMenuDispalyGroup[] _dispalyGroups;

        protected override void DoInit(InputAction goBackInput)
        {
            foreach (ControlsMenuDispalyGroup controlsMenuDispalyGroup in _dispalyGroups)
            {
                controlsMenuDispalyGroup.Init(_descriptionText, VideoDisplayer);
            }
        }
        
    }
}