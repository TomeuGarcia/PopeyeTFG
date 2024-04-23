using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorTrajectoryConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorTrajectoryConfig")]
    public class AnchorTrajectoryConfig : ScriptableObject
    {
        [SerializeField, Range(10, 50)] private int _numberOfPoints = 20;
        [SerializeField] private AnchorTrajectoryViewConfig _viewConfig;

        public int NumberOfPoints => _numberOfPoints;
        public AnchorTrajectoryViewConfig ViewConfig => _viewConfig;
    }
}