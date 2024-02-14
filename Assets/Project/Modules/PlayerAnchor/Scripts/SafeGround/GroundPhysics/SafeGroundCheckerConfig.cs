using Popeye.ProjectHelpers;
using Popeye.Scripts.Collisions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGround
{
    [CreateAssetMenu(fileName = "SafeGroundCheckerConfig", 
        menuName = ScriptableObjectsHelper.PLAYERSAFEGROUND_ASSETS_PATH + "SafeGroundCheckerConfig")]
    public class SafeGroundCheckerConfig : ScriptableObject
    {
        [SerializeField] private CollisionProbingConfig _groundCollisionProbingConfig;
        
        [SerializeField] private Vector3 _probeOriginLocalOffset;
        [SerializeField] private float _probeSize;

        public LayerMask GroundCollisionLayerMask => _groundCollisionProbingConfig.CollisionLayerMask;
        public QueryTriggerInteraction GroundQueryTriggerInteraction => _groundCollisionProbingConfig.QueryTriggerInteraction;
        public float GroundProbeDistance => _groundCollisionProbingConfig.ProbeDistance;
        public Vector3 ProbeOriginLocalOffset => _probeOriginLocalOffset;
        public float ProbeSize => _probeSize;
    }
}