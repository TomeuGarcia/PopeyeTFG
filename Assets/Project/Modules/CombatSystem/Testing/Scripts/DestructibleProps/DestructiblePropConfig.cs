using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.CombatSystem.Testing.Scripts
{
    [CreateAssetMenu(fileName = "DestructiblePropConfig_NAME", 
        menuName = ScriptableObjectsHelper.COMBATSYSTEM_PATH + "DestructiblePropConfig")]
    public class DestructiblePropConfig : ScriptableObject
    {
        [Header("HEALTH")] 
        [SerializeField, Range(0, 100)] private int _maxHealth = 1;
        [SerializeField, Range(0.0f, 1.0f)] private float _knockbackResistance = 0.0f;


        [Header("VIEW")] 
        [Header("Death")] 
        [SerializeField] private TweenPunchConfig _deathPunchScale;
        [SerializeField] private TweenConfig _deathRotation;
        
        [Header("Take Damage")] 
        [SerializeField] private TweenPunchConfig _takeDamagePunchScale;
        
        
        public int MaxHealth => _maxHealth;
        public float KnockbackResistance => _knockbackResistance;
        public TweenPunchConfig DeathPunchScale => _deathPunchScale;
        public TweenConfig DeathRotation => _deathRotation;
        public TweenPunchConfig TakeDamagePunchScale => _takeDamagePunchScale;
    }
}