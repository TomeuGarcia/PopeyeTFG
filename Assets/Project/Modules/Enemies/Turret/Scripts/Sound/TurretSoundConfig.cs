using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.Enemies.Turret
{
    [CreateAssetMenu(fileName = "TurretSoundConfig",
        menuName = ScriptableObjectsHelper.ENEMIES_ASSET_PATH + "TurretSoundsConfig")]
    public class TurretSoundConfig : ScriptableObject
    {

        [Expandable] [SerializeField] private OneShotFMODSound _turretAppear;
        [Expandable] [SerializeField] private OneShotFMODSound _turretDeath;
        [Expandable] [SerializeField] private OneShotFMODSound _turretDigUp;
        [Expandable] [SerializeField] private OneShotFMODSound _turretDigDown;
        [Expandable] [SerializeField] private OneShotFMODSound _turretDamageArea;
        [Expandable] [SerializeField] private OneShotFMODSound _turretShot;

        public void PlayTurretAppear(IFMODAudioManager audioManager, GameObject attachedGameObject)
        {
            PlayOneShotSound(audioManager, attachedGameObject, _turretAppear);
        }

        public void PlayTurretShot(IFMODAudioManager audioManager, GameObject attachedGameObject)
        {
            PlayOneShotSound(audioManager, attachedGameObject, _turretShot);
        }
        public void PlayTurretDeath(IFMODAudioManager audioManager, GameObject attachedGameObject)
        {
            PlayOneShotSound(audioManager, attachedGameObject, _turretDeath);
        }
        public void PlayTurretDigUp(IFMODAudioManager audioManager, GameObject attachedGameObject)
        {
            PlayOneShotSound(audioManager, attachedGameObject, _turretDigUp);
        }
        
        public void PlayTurretDigDown(IFMODAudioManager audioManager, GameObject attachedGameObject)
        {
            PlayOneShotSound(audioManager, attachedGameObject, _turretDigDown);
        }
        
        public void PlayAreaDamage(IFMODAudioManager audioManager, GameObject attachedGameObject)
        {
            PlayOneShotSound(audioManager, attachedGameObject, _turretDamageArea);
        }

        private void PlayOneShotSound(IFMODAudioManager audioManager, GameObject attachedGameObject,
            OneShotFMODSound sound)
        {
            audioManager.PlayOneShotAttached(sound, attachedGameObject);
        }
    }
}

