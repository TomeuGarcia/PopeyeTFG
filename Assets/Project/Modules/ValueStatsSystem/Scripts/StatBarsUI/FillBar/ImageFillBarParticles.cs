using System;
using NaughtyAttributes;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem
{
    [RequireComponent(typeof(ValueStatBar))]
    public class ImageFillBarParticles : MonoBehaviour
    {
        [Required()] [SerializeField] private ImageFillBar _valueStatBar;
    
        private void OnEnable()
        {
            _valueStatBar.OnFilledToMax += OnFilledToMax;
        }
        private void OnDisable()
        {
            _valueStatBar.OnFilledToMax -= OnFilledToMax;
        }

        private void OnFilledToMax()
        {
            Debug.Log("Max");
        }
        
    }
}