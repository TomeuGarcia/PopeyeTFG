using System;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.AudioSystem.SoundVolume;
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
            float startVolumeMusic = 0.5f;
            float startVolumeAmbient = 0.5f;
            float startVolumeSFX = 0.8f;


            SoundVolumeControllersGroup soundVolumeControllersGroup
                = ServiceLocator.Instance.GetService<IFMODAudioManager>().SoundVolumeControllersGroup;


            InitVolumeSlider(_masterVolumeSliderAndConfig, soundVolumeControllersGroup.MasterVolumeController, 
                startVolumeMaster);
            
            InitVolumeSlider(_musicVolumeSliderAndConfig, soundVolumeControllersGroup.MusicVolumeController, 
                startVolumeMusic);
            
            InitVolumeSlider(_ambientVolumeSliderAndConfig, soundVolumeControllersGroup.AmbientVolumeController, 
                startVolumeAmbient);
            
            InitVolumeSlider(_sfxVolumeSliderAndConfig, soundVolumeControllersGroup.SFXVolumeController, 
                startVolumeSFX);
        }

        private void InitVolumeSlider(SmartSliderAndConfig sliderAndConfig,
            ISoundVolumeController soundVolumeController, float startVolume)
        {
            sliderAndConfig.SmartSlider.Init(sliderAndConfig.Config, startVolume, soundVolumeController.SetVolume);
            soundVolumeController.SetVolume(startVolume);
        }


    }
}