using System;
using Project.Modules.AudioSystem.Scripts.SoundVolume;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class FMODAudioManager : IFMODAudioManager
    {
        private readonly OneShotSoundsController _oneShotSoundsController;
        private readonly LastingSoundsController _lastingSoundsController;
        public GlobalParametersController GlobalParametersController { get; private set; }
        public SoundVolumeControllersGroup SoundVolumeControllersGroup { get; private set; }

        public FMODAudioManager(OneShotSoundsController oneShotSoundsController, 
            LastingSoundsController lastingSoundsController,
            GlobalParametersController globalParametersController,
            SoundVolumeControllersGroup soundVolumeControllersGroup)
        {
            _oneShotSoundsController = oneShotSoundsController;
            _lastingSoundsController = lastingSoundsController;
            GlobalParametersController = globalParametersController;
            SoundVolumeControllersGroup = soundVolumeControllersGroup;
        }


        public void PlayOneShot(OneShotFMODSound oneShotSound)
        {
            _oneShotSoundsController.Play(oneShotSound);
        }

        public void PlayOneShotAttached(OneShotFMODSound oneShotSound, GameObject attachedGameObject)
        {
            _oneShotSoundsController.Play(oneShotSound, attachedGameObject);
        }

        public void PlayOneShots(OneShotFMODSound[] oneShotSounds)
        {
            foreach (OneShotFMODSound oneShotSound in oneShotSounds)
            {
                PlayOneShot(oneShotSound);
            }
        }

        public void PlayOneShotsAttached(OneShotFMODSound[] oneShotSounds, GameObject attachedGameObject)
        {
            foreach (OneShotFMODSound oneShotSound in oneShotSounds)
            {
                PlayOneShotAttached(oneShotSound, attachedGameObject);
            }
        }


        public LastingFMODSound.SoundId PlayLastingSound(LastingFMODSound lastingSound, GameObject attachedGameObject)
        {
            return _lastingSoundsController.Play(lastingSound, attachedGameObject.transform);            
        }

        public void StopLastingSound(LastingFMODSound.SoundId lastingSoundId)
        {
            _lastingSoundsController.Stop(lastingSoundId);
        }

        public LastingFMODSound.SoundId[] PlayLastingSounds(LastingFMODSound[] lastingSounds, GameObject attachedGameObject)
        {
            LastingFMODSound.SoundId[] soundIds = new LastingFMODSound.SoundId[lastingSounds.Length];
            
            for (int i = 0; i < lastingSounds.Length; ++i)
            {
                soundIds[i] = PlayLastingSound(lastingSounds[i], attachedGameObject);
            }

            return soundIds;
        }

        public void StopLastingSounds(LastingFMODSound.SoundId[] lastingSoundIds)
        {
            foreach (LastingFMODSound.SoundId lastingSoundId in lastingSoundIds)
            {
                StopLastingSound(lastingSoundId);
            }
        }


        public void StopAllSounds()
        {
            _lastingSoundsController.StopAll();
        }
    }
}