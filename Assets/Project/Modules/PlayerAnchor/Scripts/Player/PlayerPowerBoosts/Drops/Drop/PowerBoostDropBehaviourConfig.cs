using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts.Drops
{
    [CreateAssetMenu(fileName = "PowerBoostDropBehaviourConfig_NAME", 
        menuName = ScriptableObjectsHelper.PLAYERPOWERBOOSTDROPS_ASSETS_PATH + "PowerBoostDropBehaviourConfig")]
    public class PowerBoostDropBehaviourConfig : ScriptableObject
    {
        [SerializeField, MinMaxSlider(0.0f, 3.0f)] private Vector2 _spawnPositionOffsetRange = new Vector2(0.2f, 0.5f);
        [SerializeField, MinMaxSlider(0.0f, 3.0f)] private Vector2 _delayBeforeStartMovingRange = new Vector2(0.2f, 0.3f);
        [SerializeField, MinMaxSlider(0.0f, 50.0f)] private Vector2 _movementSpeedRange = new Vector2(18.0f, 22.0f);
        [SerializeField, MinMaxSlider(0.0f, 10.0f)] private Vector2 _movementBendRange = new Vector2(1.0f, 3.0f);

        private float RandomSpawnOffset => Random.Range(_spawnPositionOffsetRange.x, _spawnPositionOffsetRange.y) *
                                           (Random.Range(0, 2) == 0 ? 1 : -1);
        public Vector3 RandomSpawnPositionOffset => new Vector3(RandomSpawnOffset, 0, RandomSpawnOffset);
        public float DelayBeforeStartMoving => Random.Range(_delayBeforeStartMovingRange.x, _delayBeforeStartMovingRange.y);
        public float RandomMovementSpeed => Random.Range(_movementSpeedRange.x, _movementSpeedRange.y);

        public Vector3 RandomMovementBendAxis => Random.Range(_movementBendRange.x, _movementBendRange.y) * 
                                           (Random.Range(0, 2) == 0 ? Vector3.up : Vector3.down);

    }
}