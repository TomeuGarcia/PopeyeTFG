using System;
using Popeye.Modules.AudioSystem;
using Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public class FocusDropCollector : MonoBehaviour
    {
        [Header("AUDIO")] 
        [SerializeField] private AFMODAudioManagerReference _audioManager;
        [SerializeField] private OneShotFMODSound _collectFocusSound;
    
        private IPlayerFocusGainer _focusGainer;

        public void Init(IPlayerFocusGainer focusGainer)
        {
            _focusGainer = focusGainer;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPowerBoostDrop powerBoostDrop) &&  powerBoostDrop.CanBeUsed())
            {
                _focusGainer.GainFocus(powerBoostDrop.GetExperienceAndSetUsed());
                
                _audioManager.PlayOneShot(_collectFocusSound);
            }            
        }

        public void Update()
        {
            // Debug Only
            if (Input.GetKeyDown(KeyCode.F))
            {
                _focusGainer.GainFocus(50);
            }
        }
    }
}