using System;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerRespawner : MonoBehaviour
    {
        private const string RESPAWN_TRIGGER_TAG = "Respawn";
        
        public Vector3 RespawnPosition { get; private set; }


        private void Awake()
        {
            RespawnPosition = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(RESPAWN_TRIGGER_TAG))
            {
                RespawnPosition = other.gameObject.GetComponent<PlayerRespawnCheckpoint_Trigger>().RespawnPosition;
            }
        }
    }
}