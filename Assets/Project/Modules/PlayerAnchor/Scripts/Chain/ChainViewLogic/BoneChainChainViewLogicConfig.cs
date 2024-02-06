using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    [CreateAssetMenu(fileName = "BoneChainChainViewLogicConfig", 
        menuName = ScriptableObjectsHelper.ANCHORCHAIN_ASSETS_PATH + "BoneChainChainViewLogicConfig")]
    public class BoneChainChainViewLogicConfig : ScriptableObject
    {
        [SerializeField] private CollisionProbingConfig _floorCollisionProbingConfig;

        [SerializeField] private ChainConfig _chainConfig;





        public CollisionProbingConfig FloorCollisionProbingConfig => _floorCollisionProbingConfig;
        
        public float MaxChainLength => _chainConfig.MaxChainLength;

    }
}