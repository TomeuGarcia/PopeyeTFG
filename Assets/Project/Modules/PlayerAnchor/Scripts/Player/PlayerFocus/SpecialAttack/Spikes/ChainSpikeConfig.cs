using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spikes
{
    [CreateAssetMenu(fileName = "ChainSpikeConfig", 
        menuName = ScriptableObjectsHelper.PLAYERSPECIALATTACKS_ASSETS_PATH + "ChainSpikeConfig")]
    public class ChainSpikeConfig : ScriptableObject
    {
        [SerializeField] private ChainSpikeViewConfig _viewConfig;
        public ChainSpikeViewConfig ViewConfig => _viewConfig;

    }
}