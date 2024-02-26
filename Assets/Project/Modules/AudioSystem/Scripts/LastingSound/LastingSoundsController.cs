using System;
using System.Collections.Generic;
using Popeye.Core.Pool;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class LastingSoundsController
    {
        private readonly Transform _lastingSoundEmittersParent;
        private readonly ObjectPool _lastingSoundEmittersPool;
        private Dictionary<Guid, LastingFMODSoundEmitter> _activeLastingSoundEmitters;

        
        public LastingSoundsController(Transform lastingSoundEmittersParent, LastingSoundsControllerConfig config)
        {
            _lastingSoundEmittersParent = lastingSoundEmittersParent;
            _lastingSoundEmittersPool = new ObjectPool(config.LastingSoundEmitterPrefab, _lastingSoundEmittersParent);
            _lastingSoundEmittersPool.Init(config.StartNumberOfLastingSounds);
        }
            
        public void Play(ILastingFMODSound lastingSound, Transform attachedGameObject)
        {
            LastingFMODSoundEmitter soundEmitter =
                _lastingSoundEmittersPool.Spawn<LastingFMODSoundEmitter>(attachedGameObject.position, Quaternion.identity);
            soundEmitter.transform.parent = attachedGameObject;
            
            soundEmitter.Play(lastingSound);
            _activeLastingSoundEmitters.Add(lastingSound.Id, soundEmitter);
        }
        
        public void Stop(ILastingFMODSound lastingSound)
        {
            if (_activeLastingSoundEmitters.Remove(lastingSound.Id, out LastingFMODSoundEmitter soundEmitter))
            {
                soundEmitter.Stop();
                ResetSoundEmitter(soundEmitter);
            }
        }

        public void StopAll()
        {
            foreach (KeyValuePair<Guid,LastingFMODSoundEmitter> idToSoundEmitter in _activeLastingSoundEmitters)
            {
                idToSoundEmitter.Value.Stop();
                ResetSoundEmitter(idToSoundEmitter.Value);
            }
            
            _activeLastingSoundEmitters.Clear();
        }

        private void ResetSoundEmitter(LastingFMODSoundEmitter soundEmitter)
        {
            soundEmitter.transform.parent = _lastingSoundEmittersParent;
            soundEmitter.transform.localPosition = Vector3.zero;
        }
    }
}