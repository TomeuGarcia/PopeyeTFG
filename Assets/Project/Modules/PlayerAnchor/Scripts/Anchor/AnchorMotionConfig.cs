using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "AnchorMotionConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorMotionConfig")]
    public class AnchorMotionConfig : ScriptableObject
    {
        [SerializeField, Range(0.01f, 2.0f)] private float _maxCarriedDuration = 0.15f;

        public float MaxCarriedDuration => _maxCarriedDuration;
    }
}