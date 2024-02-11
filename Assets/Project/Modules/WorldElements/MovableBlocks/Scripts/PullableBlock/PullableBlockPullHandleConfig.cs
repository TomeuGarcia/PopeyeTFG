using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    [CreateAssetMenu(fileName = "NAME_PullableBlockPullHandleConfig", 
        menuName = ScriptableObjectsHelper.GRIDMOVEMENT_ASSETS_PATH + "PullableBlockPullHandleConfig")]
    public class PullableBlockPullHandleConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 20.0f)] private float _requiredDistanceToPull = 8.0f;
        [SerializeField, Range(0.0f, 5.0f)] private float _requiredTimePulling = 0.4f;


        public float RequiredDistanceToPull => _requiredDistanceToPull;
        public float RequiredTimePulling => _requiredTimePulling;
    }
}