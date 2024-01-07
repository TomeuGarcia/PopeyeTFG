using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations
{
    [CreateAssetMenu(fileName = "CollisionProbingConfig", 
        menuName = ScriptableObjectsHelper.COLLISIONS_PATH + "CollisionProbingConfig")]
    public class CollisionProbingConfig : ScriptableObject
    {
        [Header("COLLISION DETECTION")] 
        [SerializeField] private LayerMask _collisionLayerMask;
        [SerializeField] private QueryTriggerInteraction _queryTriggerInteraction = QueryTriggerInteraction.Ignore;
        [SerializeField, Min(0.1f)] private float _probeDistance = 100f;

        public LayerMask CollisionLayerMask => _collisionLayerMask;
        public QueryTriggerInteraction QueryTriggerInteraction => _queryTriggerInteraction;
        public float ProbeDistance => _probeDistance;
    }
}