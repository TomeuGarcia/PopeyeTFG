using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [System.Serializable]
    public class TweenFadeConfig
    {
        [SerializeField, Range(0f, 1f)] private float _value = 0f;
        [SerializeField, Range(0.01f, 5.0f)] private float _duration = 0.3f;
        [SerializeField] private Ease _ease = Ease.InOutSine;
        
        public float Value => _value;
        public float Duration => _duration;
        public Ease Ease => _ease;

    }
}