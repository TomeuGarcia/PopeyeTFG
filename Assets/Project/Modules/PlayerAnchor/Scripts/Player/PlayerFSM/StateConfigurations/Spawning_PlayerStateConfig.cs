using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations
{
    
    [CreateAssetMenu(fileName = "Spawning_PlayerStateConfig", 
        menuName = PlayerStateConfigHelper.SO_ASSETS_PATH + "Spawning_PlayerStateConfig")]
    public class Spawning_PlayerStateConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 10.0f)] private float _spawnDuration = 0.5f;
        public float SpawnDuration => _spawnDuration;
    }
}