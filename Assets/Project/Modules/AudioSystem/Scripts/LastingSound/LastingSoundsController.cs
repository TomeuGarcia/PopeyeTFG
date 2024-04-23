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
        private readonly Dictionary<LastingFMODSound.SoundId, LastingFMODSoundEmitter> _activeLastingSoundEmitters;

        
        public LastingSoundsController(Transform lastingSoundEmittersParent, LastingSoundsControllerConfig config)
        {
            _lastingSoundEmittersParent = lastingSoundEmittersParent;
            _lastingSoundEmittersPool = new ObjectPool(config.LastingSoundEmitterPrefab, _lastingSoundEmittersParent);
            _lastingSoundEmittersPool.Init(config.StartNumberOfLastingSounds);

            _activeLastingSoundEmitters = new Dictionary<LastingFMODSound.SoundId, LastingFMODSoundEmitter>(10);
        }
            
        public LastingFMODSound.SoundId Play(LastingFMODSound lastingSound, Transform attachedGameObject)
        {
            LastingFMODSound.SoundId soundId = new LastingFMODSound.SoundId();
            
            LastingFMODSoundEmitter soundEmitter =
                _lastingSoundEmittersPool.Spawn<LastingFMODSoundEmitter>(attachedGameObject.position, Quaternion.identity);
            soundEmitter.transform.parent = attachedGameObject;
            
            soundEmitter.Play(lastingSound);
            _activeLastingSoundEmitters.Add(soundId, soundEmitter);

            return soundId;
        }
        
        public void Stop(LastingFMODSound.SoundId lastingSoundId)
        {
            if (_activeLastingSoundEmitters.Remove(lastingSoundId, out LastingFMODSoundEmitter soundEmitter))
            {
                soundEmitter.Stop();
                ResetSoundEmitter(soundEmitter);
            }
        }

        public void StopAll()
        {
            foreach (KeyValuePair<LastingFMODSound.SoundId, LastingFMODSoundEmitter> idToSoundEmitter in _activeLastingSoundEmitters)
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