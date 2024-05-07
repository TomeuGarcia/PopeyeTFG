using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spikes
{
    [System.Serializable]
    public class ChainSpikeViewConfig
    {
        [System.Serializable]
        public class SpikeSpawnAnimation
        {
            [SerializeField] private TweenConfig _preScaleUp;
            [SerializeField] private TweenConfig _scaleUp;
            [SerializeField] private TweenConfig _scaledUpRotation;
            [SerializeField] private TweenConfig _scaleDown;
            [SerializeField] private TweenConfig _postScaleDown;

            public TweenConfig PreScaleUp => _preScaleUp;
            public TweenConfig ScaleUp => _scaleUp;
            public TweenConfig ScaledUpRotation => _scaledUpRotation;
            public TweenConfig ScaleDown => _scaleDown;
            public TweenConfig PostScaleDown => _postScaleDown;
        }

        [Header("SPAWN ANIMATION")] 
        [SerializeField] private SpikeSpawnAnimation _spawnAnimation;
        public SpikeSpawnAnimation SpawnAnimation => _spawnAnimation;
    }
}