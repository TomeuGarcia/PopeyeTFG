using Popeye.Modules.CombatSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [CreateAssetMenu(fileName = "FlatStraightProjectileConfig", 
        menuName = ScriptableObjectsHelper.HAZARDS_ASSET_PATH + "FlatStraightProjectileConfig")]
    public class FlatStraightProjectileConfig : ScriptableObject
    {
        [SerializeField] private DamageHitConfig _damageHitConfig;

        [SerializeField, Range(0.0f, 10.0f)] private float _maximumLifetime = 8.0f; 
        [SerializeField, Range(0.0f, 10.0f)] private float _disappearDuration = 0.3f; 
        [SerializeField, Range(0.0f, 10.0f)] private float _movementSpeed = 8.0f; 
        
        public DamageHitConfig DamageHitConfig => _damageHitConfig;
        public float MaximumLifetime => _maximumLifetime;
        public float DisappearDuration => _disappearDuration;
        public float MovementSpeed => _movementSpeed;
    }
}