using FMODUnity;
using Popeye.Core.Pool;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    public class LastingFMODSoundEmitter : RecyclableObject
    {
        [SerializeField] private FMODUnity.StudioEventEmitter _eventEmitter;

        private ILastingFMODSound _currentSound;
        
        
        internal override void Init()
        {
            
        }

        internal override void Release()
        {
            
        }

        
        public void Play(ILastingFMODSound lastingSound)
        {
            _currentSound = lastingSound;

            SubscribeToParameters();

            _eventEmitter.EventReference = _currentSound.EventReference;
            _eventEmitter.Play();
        }

        public void Stop()
        {
            UnsubscribeToParameters();
            
            _eventEmitter.Stop();
            
            Recycle();
        }

        public bool IsPlaying()
        {
            return _eventEmitter.IsPlaying();
        }


        private void SubscribeToParameters()
        {
            foreach (SoundParameter parameter in _currentSound.Parameters)
            {
                parameter.OnValueChanged += UpdateEmitterParameter;
            }
        }
        private void UnsubscribeToParameters()
        {
            foreach (SoundParameter parameter in _currentSound.Parameters)
            {
                parameter.OnValueChanged -= UpdateEmitterParameter;
            }
        }
        
        private void UpdateEmitterParameter(SoundParameter parameter)
        {
            _eventEmitter.SetParameter(parameter.Name, parameter.Value);
        }
        

    }
}