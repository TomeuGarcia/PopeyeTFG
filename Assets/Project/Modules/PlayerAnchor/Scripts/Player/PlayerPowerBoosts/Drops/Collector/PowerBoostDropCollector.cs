using System;
using AYellowpaper;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    public class PowerBoostDropCollector : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IPlayerPowerBoostController, ScriptableObject> _powerBoostController;


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPowerBoostDrop powerBoostDrop))
            {
                _powerBoostController.Value.AddExperience(powerBoostDrop.Experience);
            }
            
            Debug.Log(other.name);
        }
        
        
    }
}