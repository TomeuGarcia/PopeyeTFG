using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.ValueStatSystem
{
    [RequireComponent(typeof(ValueStatBar))]
    public class ImageFillBarParticles : MonoBehaviour
    {
        [Header("IMAGE FILL BAR")]
        [Required()] [SerializeField] private ImageFillBar _imageFillBar;
        
        [Header("PARTICLES")]
        [SerializeField] private ParticleSystem _maxFillBurstParticles;
        
        [SerializeField] private bool _maxFillConstantParticles = true;
        [ShowIf("_maxFillConstantParticles")] [SerializeField] private ParticleSystem _maxFillParticles;
    
        private void OnEnable()
        {
            _imageFillBar.OnFilledToMax += OnFilledToMax;
            _imageFillBar.OnStopBeingFilledToMax += OnStopBeingFilledToMax;
        }
        private void OnDisable()
        {
            _imageFillBar.OnFilledToMax -= OnFilledToMax;
            _imageFillBar.OnStopBeingFilledToMax -= OnStopBeingFilledToMax;
        }

        private void OnFilledToMax()
        {
            _maxFillBurstParticles.Play();

            if (_maxFillConstantParticles)
            {
                _maxFillParticles.Play();
            }
        }
        private void OnStopBeingFilledToMax()
        {
            if (_maxFillConstantParticles)
            {
                _maxFillParticles.Stop();
            }
        }
        
        
    }
}