using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using Project.Scripts.Time.TimeHitStop;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    [System.Serializable]
    public class PlayerGameFeelEffectsViewConfig
    {
        [Header("TAKE DAMAGE")]
        [SerializeField] private HitStopConfig _takeDamageHitStop;
        [SerializeField] private CameraShakeConfig _takeDamageCameraShake;
        
        [Header("HEALING")]
        [SerializeField] private CameraZoomConfig _healingZoomIn;
        
        [Header("SPECIAL ATTACK")]
        [SerializeField] private CameraZoomInOutConfig _specialAttackZoomInOut;
        
        [Header("INTERRUPT")]
        [SerializeField] private CameraZoomConfig _interruptedZoom;
        
        public HitStopConfig TakeDamageHitStop => _takeDamageHitStop;
        public CameraShakeConfig TakeDamageCameraShake => _takeDamageCameraShake;
        public CameraZoomConfig HealingZoomIn => _healingZoomIn;
        public CameraZoomInOutConfig SpecialAttackZoomInOut => _specialAttackZoomInOut;
        public CameraZoomConfig InterruptedToZoomOut => _interruptedZoom;

    }
}