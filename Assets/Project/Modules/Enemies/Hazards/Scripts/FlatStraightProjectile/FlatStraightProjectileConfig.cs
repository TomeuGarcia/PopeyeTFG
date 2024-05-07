using Popeye.Modules.CombatSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    [CreateAssetMenu(fileName = "FlatStraightProjectileConfig", 
        menuName = ScriptableObjectsHelper.HAZARDS_ASSET_PATH + "FlatStraightProjectileConfig")]
    public class FlatStraightProjectileConfig : ScriptableObject
    {
        [Header("DAMAGE")]
        [SerializeField] private DamageHitConfig _damageHitConfig;

        [Header("LOGIC")]
        [SerializeField, Range(0.0f, 10.0f)] private float _maximumLifetime = 8.0f; 
        [SerializeField, Range(0.0f, 10.0f)] private float _disappearDuration = 0.3f; 
        [SerializeField, Range(0.0f, 50.0f)] private float _movementSpeed = 8.0f;

        [Header("VIEW")] 
        [SerializeField] private FlatStraightProjectileViewConfig _viewConfig;
        
        [Header("SOUND")] 
        [SerializeField] private FMODFlatStraightProjectileAudio _audio;
        
        public DamageHitConfig DamageHitConfig => _damageHitConfig;
        public float MaximumLifetime => _maximumLifetime;
        public float DisappearDuration => _disappearDuration;
        public float MovementSpeed => _movementSpeed;
        public FlatStraightProjectileViewConfig ViewConfig => _viewConfig;
        public IFlatStraightProjectileAudio Audio => _audio;
    }
}