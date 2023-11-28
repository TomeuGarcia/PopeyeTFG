using Popeye.Modules.PlayerAnchor.Player.PlayerStates;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerMediator
    {

        public void SetMaxMovementSpeed(float maxMovementSpeed);
        public void SetCanRotate(bool canRotate);

        public void ThrowAnchor();

    }
}