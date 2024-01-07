using NaughtyAttributes;
using UnityEngine;

namespace Project.Modules.CombatSystem.KnockbackSystem
{
    [System.Serializable]
    public class KnockbackHitConfig
    {
        [SerializeField] private KnockbackHitType _knockbackType = KnockbackHitType.Push;
        [SerializeField, Min(0.01f)] private float _duration = 0.2f;
        
        [AllowNesting]
        [ShowIf("_knockbackType", KnockbackHitType.Push)]
        [SerializeField, Range(-20.0f, 20.0f)] private float _pushDistance = 3.0f;


        public KnockbackHitType KnockbackType => _knockbackType;
        public float Duration => _duration;
        public float PushDistance => _pushDistance;
    }
}