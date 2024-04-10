using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [System.Serializable]
    public class TweenFadeConfig
    {
        [SerializeField, Range(0f, 1f)] private float _value = 0f;
        [SerializeField, Range(0.01f, 5.0f)] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutSine;
        
        public float Value => _value;
        public float Duration => _duration;
        public Ease Ease => _ease;


        public TweenFadeConfig(float value, float duration = 0.5f, Ease ease = Ease.InOutSine)
        {
            _value = value;
            _duration = duration;
            _ease = ease;
        }
        public static TweenFadeConfig FadeIn()
        {
            return new TweenFadeConfig(1);
        }
        public static TweenFadeConfig FadeOut()
        {
            return new TweenFadeConfig(0);
        }
    }
}