using System;
using FMODUnity;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [CreateAssetMenu(fileName = "LastingSound_NAME", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "LastingSound")]
    public class LastingFMODSound : ScriptableObject
    {
        [SerializeField] private EventReference _eventReference;
        [SerializeField] private SoundParameter[] _parameters;
        
        public EventReference EventReference => _eventReference;
        public SoundParameter[] Parameters => _parameters;
        public Guid Id { get; private set; }


        private void Awake()
        {
            Id = Guid.NewGuid();
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            Awake();
        }
#endif
        
    }
    
    
}