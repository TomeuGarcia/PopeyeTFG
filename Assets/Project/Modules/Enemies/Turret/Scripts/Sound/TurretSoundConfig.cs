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
        [Header("AUDIO MANAGER")]
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        
        [Header("SOUNDS")]
        [Expandable] [SerializeField] private OneShotFMODSound _turretAppear;
        [Expandable] [SerializeField] private OneShotFMODSound _turretDeath;
        [Expandable] [SerializeField] private OneShotFMODSound _turretDigUp;
        [Expandable] [SerializeField] private OneShotFMODSound _turretDigDown;
        [Expandable] [SerializeField] private OneShotFMODSound _turretDamageArea;
        [Expandable] [SerializeField] private OneShotFMODSound _turretShot;

        public void PlayTurretAppear(GameObject attachedGameObject)
        {
            PlayOneShotSound(attachedGameObject, _turretAppear);
        }

        public void PlayTurretShot(GameObject attachedGameObject)
        {
            PlayOneShotSound(attachedGameObject, _turretShot);
        }
        public void PlayTurretDeath(GameObject attachedGameObject)
        {
            PlayOneShotSound(attachedGameObject, _turretDeath);
        }
        public void PlayTurretDigUp(GameObject attachedGameObject)
        {
            PlayOneShotSound(attachedGameObject, _turretDigUp);
        }
        
        public void PlayTurretDigDown(GameObject attachedGameObject)
        {
            PlayOneShotSound(attachedGameObject, _turretDigDown);
        }
        
        public void PlayAreaDamage(GameObject attachedGameObject)
        {
            PlayOneShotSound(attachedGameObject, _turretDamageArea);
        }

        private void PlayOneShotSound(GameObject attachedGameObject,
            OneShotFMODSound sound)
        {
            _audioManager.PlayOneShotAttached(sound, attachedGameObject);
        }
    }
}

