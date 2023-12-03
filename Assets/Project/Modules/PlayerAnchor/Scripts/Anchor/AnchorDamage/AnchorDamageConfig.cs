using Project.Modules.CombatSystem;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorDamageConfig", 
        menuName = AnchorConfigHelper.SO_ASSETS_PATH + "AnchorDamageConfig")]
    public class AnchorDamageConfig : ScriptableObject
    {
        [SerializeField] private DamageHitConfig _anchorThrowDamageHit;
        
        
        public DamageHitConfig AnchorThrowDamageHit => _anchorThrowDamageHit;
        
    }
}