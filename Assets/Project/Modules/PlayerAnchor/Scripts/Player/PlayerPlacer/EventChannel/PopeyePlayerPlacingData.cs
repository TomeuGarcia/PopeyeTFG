using Popeye.Modules.VFX.Generic;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPlacer
{
    public struct PopeyePlayerPlacingData
    {
        public Vector3 playerPosition;
        public Quaternion playerRotation;
        public Vector3 anchorPosition;
        public Quaternion anchorRotation;

        public bool isNewPlayerRespawn;
        
        public bool startCarryingAnchor;
        
        public bool debugUnlockAllAbilities;

        public EnvironmentFollowData environmentFollowData;
    }

    [System.Serializable]
    public struct EnvironmentFollowData
    {
        public EnvironmentFollower.EnvironmentElementConfig waterConfig;
        public EnvironmentFollower.EnvironmentElementConfig rainConfig;
    }
}