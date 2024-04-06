using System;
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
        [SerializeField, Range(0.0f, 360.0f)] private float _requiredAngleForPulling = 120.0f;


        public float RequiredDistanceToPull => _requiredDistanceToPull;
        public float RequiredTimePulling => _requiredTimePulling;
        public float RequiredDotForPulling { get; private set; }

        private void OnValidate()
        {
            RequiredDotForPulling = Mathf.Cos((_requiredAngleForPulling / 2) * Mathf.Rad2Deg);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}