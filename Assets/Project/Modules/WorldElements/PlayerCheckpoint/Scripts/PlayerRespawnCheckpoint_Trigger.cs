using Popeye.Modules.PlayerAnchor.SafeGround;
using UnityEngine;


namespace Popeye.Modules.WorldElements
{
    public class PlayerRespawnCheckpoint_Trigger : MonoBehaviour, ICheckpointTrigger
    {
        [SerializeField] private Transform _respawnPoint;
        public Vector3 RespawnPosition => _respawnPoint.position;

    }
}


