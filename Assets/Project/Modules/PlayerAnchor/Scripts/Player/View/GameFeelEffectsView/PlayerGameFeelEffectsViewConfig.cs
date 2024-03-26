using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using Project.Scripts.Time.TimeHitStop;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [System.Serializable]
    public class PlayerGameFeelEffectsViewConfig
    {
        [SerializeField] private HitStopConfig _takeDamageHitStop;
        [SerializeField] private CameraShakeConfig _takeDamageCameraShake;
        [SerializeField] private CameraZoomInOutConfig _healingZoomInOut;
        [SerializeField] private CameraZoomConfig _healingInterrupted;
        
        public HitStopConfig TakeDamageHitStop => _takeDamageHitStop;
        public CameraShakeConfig TakeDamageCameraShake => _takeDamageCameraShake;
        public CameraZoomInOutConfig HealingZoomInOut => _healingZoomInOut;
        public CameraZoomConfig HealingInterrupted => _healingInterrupted;
        
        
    }
}