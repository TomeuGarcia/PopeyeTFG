using System;
using AYellowpaper;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    public class PowerBoostDropCollector : MonoBehaviour
    {
        private IPlayerPowerBoostController _powerBoostController;

        public void Init(IPlayerPowerBoostController powerBoostController)
        {
            _powerBoostController = powerBoostController;
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPowerBoostDrop powerBoostDrop) &&  powerBoostDrop.CanBeUsed())
            {
                _powerBoostController.AddExperience(powerBoostDrop.GetExperienceAndSetUsed());
            }
            
        }
        
        
    }
}