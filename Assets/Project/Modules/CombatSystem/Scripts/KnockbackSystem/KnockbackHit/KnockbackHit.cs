using UnityEngine;

namespace Project.Modules.CombatSystem.KnockbackSystem
{
    public class KnockbackHit
    {
        private readonly KnockbackHitConfig _config;
        private float PushDistance => _config.PushDistance;
        public float Duration => _config.Duration;
        public KnockbackHitType KnockbackType => _config.KnockbackType;
        
        
        
        public Vector3 PushDisplacement { get; private set; }
        public Vector3 EndPosition { get; private set; }


        public KnockbackHit(KnockbackHitConfig config)
        {
            _config = config;
        }
        
        
        public void UpdatePushDirection(Vector3 pushDirection)
        {
            PushDisplacement = pushDirection * PushDistance;
        }
        
        public void UpdateEndPosition(Vector3 endPosition)
        {
            EndPosition = endPosition;
        }
        
    }
}