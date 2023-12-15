using System;
using Project.Modules.PlayerAnchor.Anchor;
using Project.Scripts.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.PlayerAnchor.Chain
{
    [CreateAssetMenu(fileName = "AnchorChainConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorChainConfig")]
    public class ChainConfig : ScriptableObject
    {
        [Header("CHAIN LENGTH")]
        [SerializeField, Range(0.0f, 20.0f)] private float _maxChainLength = 8.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _failedThrowExtraLength = 3.0f;
        
        public float MaxChainLength => _maxChainLength;
        public float FailedThrowExtraLength => _failedThrowExtraLength;
        
        
        [Header("SPRING")]
        [SerializeField, Range(0.0f, 500.0f)] private float _chainSpringForce = 300.0f;

        public float ChainSpringForce => _chainSpringForce;
        

        
        public Action OnValuesUpdated;

        private void OnValidate()
        {
            OnValuesUpdated?.Invoke();
        }
    }
}