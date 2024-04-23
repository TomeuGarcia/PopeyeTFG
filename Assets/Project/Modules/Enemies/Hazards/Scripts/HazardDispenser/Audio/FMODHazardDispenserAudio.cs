using NaughtyAttributes;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [System.Serializable]
    public class FMODHazardDispenserAudio : IHazardDispenserAudio
    {
        [Header("AUDIO MANAGER")]
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        
        [Header("SOUNDS")]
        [Expandable] [SerializeField] private OneShotFMODSound _prepareSound;
        [Expandable] [SerializeField] private OneShotFMODSound _dispenseSound;

        
        public void PlayPrepareSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_prepareSound, source);
        }

        public void PlayDispenseSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_dispenseSound, source);
        }
    }
}