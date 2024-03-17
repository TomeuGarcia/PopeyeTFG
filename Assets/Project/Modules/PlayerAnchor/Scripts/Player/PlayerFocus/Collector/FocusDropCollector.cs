using Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public class FocusDropCollector : MonoBehaviour
    {
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
            }
            
        }
    }
}