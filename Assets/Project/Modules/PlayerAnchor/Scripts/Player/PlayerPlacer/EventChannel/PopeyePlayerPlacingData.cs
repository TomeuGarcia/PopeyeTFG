using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    public struct PopeyePlayerPlacingData
    {
        public Vector3 playerPosition;
        public Quaternion playerRotation;
        public Vector3 anchorPosition;
        public Quaternion anchorRotation;

        public bool startCarryingAnchor;
    }
}