using Popeye.Core.Services.InformationDisplay;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint.View
{
    [CreateAssetMenu(fileName = "CheckpointSetDisplayerConfig", 
        menuName = ScriptableObjectsHelper.PLAYERCHECKPOINTS_ASSETS_PATH + "CheckpointSetDisplayerConfig")]
    public class CheckpointSetDisplayerConfig : ScriptableObject
    {
        [SerializeField] private TextDisplayConfig _checkpointSetTextDisplay;
        [SerializeField, Range(0.0f, 20.0f)] private float _displayDelay = 1.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _displayDuration = 8.0f;
        
        public TextDisplayConfig CheckpointSetTextDisplay => _checkpointSetTextDisplay;
        public float DisplayDelay => _displayDelay;
        public float DisplayDuration => _displayDuration;
    }
}