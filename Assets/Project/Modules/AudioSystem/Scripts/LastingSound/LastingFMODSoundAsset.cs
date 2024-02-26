using System;
using FMODUnity;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [CreateAssetMenu(fileName = "LastingFMODSoundAsset_NAME", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "LastingFMODSoundAsset")]
    public class LastingFMODSoundAsset : ScriptableObject, ILastingFMODSound
    {
        [SerializeField] private EventReference _eventReference;
        [SerializeField] private SoundParameter[] _parameters;
        
        public EventReference EventReference => _eventReference;
        public SoundParameter[] Parameters => _parameters;
        public Guid Id { get; private set; }


        private void Awake()
        {
            Id = new Guid();
        }
    }
}