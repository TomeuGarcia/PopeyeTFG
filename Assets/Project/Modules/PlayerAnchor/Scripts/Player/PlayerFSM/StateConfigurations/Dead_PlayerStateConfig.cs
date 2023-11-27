using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations
{
    
    [CreateAssetMenu(fileName = "Dead_PlayerStateConfig", 
        menuName = PlayerStateConfigHelper.ASSET_PATH + "Dead_PlayerStateConfig")]
    public class Dead_PlayerStateConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 10.0f)] private float _durationBeforeRespawn = 2.0f;
        public float DurationBeforeRespawn => _durationBeforeRespawn;
    }
}