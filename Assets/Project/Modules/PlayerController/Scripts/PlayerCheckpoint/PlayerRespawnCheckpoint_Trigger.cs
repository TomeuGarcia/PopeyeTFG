using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnCheckpoint_Trigger : MonoBehaviour
{
    [SerializeField] private Transform _respawnPoint;
    public Vector3 RespawnPosition => _respawnPoint.position;

}
