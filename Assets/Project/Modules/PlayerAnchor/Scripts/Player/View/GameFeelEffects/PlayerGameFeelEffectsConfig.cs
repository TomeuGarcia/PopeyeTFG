using Popeye.Modules.Camera.CameraShake;
using Project.Scripts.Time.TimeHitStop;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.GameFeelEffects
{
    [System.Serializable]
    public class PlayerGameFeelEffectsConfig
    {
        [SerializeField] private HitStopConfig _takeDamageHitStop;
        [SerializeField] private CameraShakeConfig _takeDamageCameraShake;
        
        public HitStopConfig TakeDamageHitStop => _takeDamageHitStop;
        public CameraShakeConfig TakeDamageCameraShake => _takeDamageCameraShake;
        
        
    }
}