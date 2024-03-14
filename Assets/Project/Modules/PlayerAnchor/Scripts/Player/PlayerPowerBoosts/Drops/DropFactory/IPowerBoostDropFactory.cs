using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    public interface IPowerBoostDropFactory
    {
        void Create(Vector3 position, Quaternion rotation, PowerBoostDropConfig dropConfig);
    }
}