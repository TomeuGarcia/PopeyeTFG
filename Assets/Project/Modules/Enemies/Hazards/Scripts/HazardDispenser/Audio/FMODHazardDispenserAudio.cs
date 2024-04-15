using NaughtyAttributes;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [System.Serializable]
    public class FMODHazardDispenserAudio : IHazardDispenserAudio
    {
        private IFMODAudioManager _audioManager;
        
        [Expandable] [SerializeField] private OneShotFMODSound _prepare;
        [Expandable] [SerializeField] private OneShotFMODSound _dispense;

        
        public void Configure()
        {
            _audioManager = ServiceLocator.Instance.GetService<IFMODAudioManager>();
        }
        

        public void PlayPrepareSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_prepare, source);
        }

        public void PlayDispenseSound(GameObject source)
        {
            _audioManager.PlayOneShotAttached(_dispense, source);
        }
    }
}