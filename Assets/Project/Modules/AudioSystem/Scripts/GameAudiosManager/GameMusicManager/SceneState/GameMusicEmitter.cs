using System;
using FMODUnity;
using UnityEngine;

namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    public class GameMusicEmitter : MonoBehaviour
    {
        private StudioEventEmitter[] _eventEmitters;
        
        public void Init(LastingFMODSound[] lastingSounds)
        {
            _eventEmitters = new StudioEventEmitter[lastingSounds.Length];
        
            for (int i = 0; i < lastingSounds.Length; ++i)
            {
                StudioEventEmitter eventEmitter = gameObject.AddComponent<StudioEventEmitter>();
                eventEmitter.EventReference = lastingSounds[i].EventReference;
                eventEmitter.Play();

                _eventEmitters[i] = eventEmitter;
            }
        }

        private void OnDestroy()
        {
            foreach (StudioEventEmitter eventEmitter in _eventEmitters)
            {
                eventEmitter.Stop();
            }
        }
        
    }
}