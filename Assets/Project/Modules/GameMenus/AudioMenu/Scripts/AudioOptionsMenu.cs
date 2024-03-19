using System;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.GameMenus.Generic;
using Project.Modules.AudioSystem.Scripts.SoundVolume;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Popeye.Modules.GameMenus.AudioMenu
{
    public class AudioOptionsMenu : AMenuController
    {
        [SerializeField] private SmartSliderAndConfig _masterVolumeSliderAndConfig;
        [SerializeField] private SmartSliderAndConfig _musicVolumeSliderAndConfig;
        [SerializeField] private SmartSliderAndConfig _ambientVolumeSliderAndConfig;
        [SerializeField] private SmartSliderAndConfig _sfxVolumeSliderAndConfig;
        
        
        protected override void DoInit(InputAction goBackButton)
        {
            float startVolumeMaster = 1.0f;
            float startVolumeMusic = 0.8f;
            float startVolumeAmbient = 0.5f;
            float startVolumeSFX = 0.8f;


            SoundVolumeControllersGroup soundVolumeControllersGroup
                = ServiceLocator.Instance.GetService<IFMODAudioManager>().SoundVolumeControllersGroup;
            
            
            
            _masterVolumeSliderAndConfig.SmartSlider.Init(_masterVolumeSliderAndConfig.Config,
                startVolumeMaster, soundVolumeControllersGroup.MasterVolumeController.SetVolume);
            
            _musicVolumeSliderAndConfig.SmartSlider.Init(_musicVolumeSliderAndConfig.Config,
                startVolumeMusic, soundVolumeControllersGroup.MusicVolumeController.SetVolume);
                        
            _ambientVolumeSliderAndConfig.SmartSlider.Init(_ambientVolumeSliderAndConfig.Config,
                startVolumeAmbient, soundVolumeControllersGroup.AmbientVolumeController.SetVolume);
                        
            _sfxVolumeSliderAndConfig.SmartSlider.Init(_sfxVolumeSliderAndConfig.Config,
                startVolumeSFX, soundVolumeControllersGroup.SFXVolumeController.SetVolume);
        }


    }
}