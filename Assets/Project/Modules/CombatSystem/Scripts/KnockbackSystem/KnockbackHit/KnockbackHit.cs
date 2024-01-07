using UnityEngine;

namespace Project.Modules.CombatSystem.KnockbackSystem
{
    public class KnockbackHit
    {
        public float _pushDistance;
        
        public KnockbackHitType KnockbackType { get; private set; }
        public float Duration { get; private set; }
        
        
        
        public Vector3 PushDisplacement { get; private set; }
        public Vector3 EndPosition { get; private set; }


        public KnockbackHit(KnockbackHitConfig config)
        {
            _pushDistance = config.PushDistance;
            KnockbackType = config.KnockbackType;
            Duration = config.Duration;
        }
        
        
        public void UpdatePushDirection(Vector3 pushDirection)
        {
            PushDisplacement = pushDirection * _pushDistance;
        }
        
        public void UpdateEndPosition(Vector3 endPosition)
        {
            EndPosition = endPosition;
        }
        
    }
}