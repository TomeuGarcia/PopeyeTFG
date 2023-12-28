using System;
using Popeye.Modules.WorldElements;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerRespawner : MonoBehaviour
    {
        private const string RESPAWN_TRIGGER_TAG = "Respawn";
        
        public Vector3 RespawnPosition { get; private set; }


        private void Awake()
        {
            SetRespawnPosition(transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(RESPAWN_TRIGGER_TAG))
            {
                SetRespawnPosition(
                    other.gameObject.GetComponent<PlayerRespawnCheckpoint_Trigger>().RespawnPosition);
            }
        }

        private void SetRespawnPosition(Vector3 position)
        {
            RespawnPosition = position + Vector3.up;
        }
    }
}