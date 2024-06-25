using System;
using AYellowpaper;
using Popeye.Core.Services.InformationDisplay;
using Popeye.Modules.GameMenus.Generic;
using Popeye.Scripts.TextUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Popeye.Modules.GameMenus.OptionsMenu
{
    public class ControlsMenuController : AMenuController
    {
        [Header("VIDEO DISPLAYER")] 
        [SerializeField] private TextInitializer _descriptionTextInitializer;
        [SerializeField] private InterfaceReference<IVideoDisplayer, MonoBehaviour> _videoDisplayer;
        
        private IVideoDisplayer VideoDisplayer => _videoDisplayer.Value;

        [Header("CONTROLS")] 
        [SerializeField] private ControlsMenuDispalyGroup[] _dispalyGroups;

        protected override void DoInit(InputAction goBackInput)
        {
            foreach (ControlsMenuDispalyGroup controlsMenuDispalyGroup in _dispalyGroups)
            {
                controlsMenuDispalyGroup.Init(_descriptionTextInitializer, VideoDisplayer);
            }
        }
        
    }
}