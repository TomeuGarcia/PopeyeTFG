using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.ValueStatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    public class PowerBoostDropperDeathwish : AHealthUserDeathwish
    {
        [SerializeField] private PowerBoostDropConfig _powerBoostDrop;
        
        protected override void DoDeathwish()
        {
            IPowerBoostDropFactory powerBoostDropFactory = ServiceLocator.Instance.GetService<IPowerBoostDropFactory>();
            powerBoostDropFactory.Create(transform.position, Quaternion.identity, _powerBoostDrop);
        }
    }
}