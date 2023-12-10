using Project.Modules.CombatSystem;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorDamageConfig", 
        menuName = AnchorConfigHelper.SO_ASSETS_PATH + "AnchorDamageConfig")]
    public class AnchorDamageConfig : ScriptableObject
    {
        [Header("THROW")]
        [SerializeField] private DamageHitConfig _anchorThrowDamageHit;
        [SerializeField, Range(0f, 5.0f)] private float _throwDamageExtraDuration = 0.2f;
        
        public DamageHitConfig AnchorThrowDamageHit => _anchorThrowDamageHit;
        public float ThrowDamageExtraDuration => _throwDamageExtraDuration;

        
        [Header("PULL")]
        [SerializeField] private DamageHitConfig _anchorPullDamageHit;
        [SerializeField, Range(0f, 5.0f)] private float _pullDamageExtraDuration = 0.2f;
        
        public DamageHitConfig AnchorPullDamageHit => _anchorPullDamageHit;
        public float PullDamageExtraDuration => _pullDamageExtraDuration;



    }
}