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
        private readonly Dictionary<Guid, LastingFMODSoundEmitter> _activeLastingSoundEmitters;

        
        public LastingSoundsController(Transform lastingSoundEmittersParent, LastingSoundsControllerConfig config)
        {
            _lastingSoundEmittersParent = lastingSoundEmittersParent;
            _lastingSoundEmittersPool = new ObjectPool(config.LastingSoundEmitterPrefab, _lastingSoundEmittersParent);
            _lastingSoundEmittersPool.Init(config.StartNumberOfLastingSounds);

            _activeLastingSoundEmitters = new Dictionary<Guid, LastingFMODSoundEmitter>(10);
        }
            
        public void Play(LastingFMODSound lastingSound, Transform attachedGameObject)
        {
            if (_activeLastingSoundEmitters.ContainsKey(lastingSound.Id))
            {
                throw new Exception($"Lasting sound {lastingSound.name} is already playing");
            }

            LastingFMODSoundEmitter soundEmitter =
                _lastingSoundEmittersPool.Spawn<LastingFMODSoundEmitter>(attachedGameObject.position, Quaternion.identity);
            soundEmitter.transform.parent = attachedGameObject;
            
            soundEmitter.Play(lastingSound);
            _activeLastingSoundEmitters.Add(lastingSound.Id, soundEmitter);
        }
        
        public void Stop(LastingFMODSound lastingSound)
        {
            if (_activeLastingSoundEmitters.Remove(lastingSound.Id, out LastingFMODSoundEmitter soundEmitter))
            {
                soundEmitter.Stop();
                ResetSoundEmitter(soundEmitter);
            }
            else
            {
                throw new Exception($"Trying to stop lasting sound {lastingSound.name}, but it is already not playing");
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