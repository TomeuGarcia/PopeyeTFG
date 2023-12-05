using Project.Modules.CombatSystem;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorDamageConfig", 
        menuName = AnchorConfigHelper.SO_ASSETS_PATH + "AnchorDamageConfig")]
    public class AnchorDamageConfig : ScriptableObject
    {
        [SerializeField] private DamageHitConfig _anchorThrowDamageHit;
        [SerializeField] private DamageHitConfig _anchorPullDamageHit;
        
        
        public DamageHitConfig AnchorThrowDamageHit => _anchorThrowDamageHit;
        public DamageHitConfig AnchorPullDamageHit => _anchorPullDamageHit;
        
    }
}