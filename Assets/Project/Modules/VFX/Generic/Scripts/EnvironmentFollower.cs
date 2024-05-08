using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Popeye.Modules.PlayerAnchor.Player.PlayerPlacer;
using UnityEngine;

namespace Popeye.Modules.VFX.Generic
{
    public class EnvironmentFollower : MonoBehaviour
    {
        [System.Serializable]
        public struct EnvironmentElementConfig
        {
            public float desiredWorldHeight;
        }
        
        [System.Serializable]
        public struct EnvironmentElement
        {
            public Transform transform;
            [HideInInspector] public EnvironmentElementConfig config;
        }

        [Header("CONFIGURATION")] 
        [SerializeField] private EnvironmentFollowData _defaultEnvironmentFollowData;

        [Header("FOLLOW TARGET")]
        [SerializeField] private Transform _followTarget;
        
        [Header("ELEMENTS")]
        [SerializeField] private EnvironmentElement _water;
        [SerializeField] private EnvironmentElement _rain;
        [SerializeField] private EnvironmentElement _wind;
        
        private EnvironmentElement[] _environmentElements;


        private void Awake()
        {
            Configure(_defaultEnvironmentFollowData);
        }

        public void Configure(EnvironmentFollowData environmentFollowData)
        {
            _water.config = environmentFollowData.waterConfig;
            _rain.config = environmentFollowData.rainConfig;
            _wind.config = environmentFollowData.windConfig;
            
            _environmentElements = new[] { _water, _rain, _wind };
        }
        
        
        void Update()
        {
            Vector3 environmentElementPosition = _followTarget.position;
            
            foreach (var element in _environmentElements)
            {
                environmentElementPosition.y = element.config.desiredWorldHeight;
                element.transform.position = environmentElementPosition;
            }
        }
        
    }
}