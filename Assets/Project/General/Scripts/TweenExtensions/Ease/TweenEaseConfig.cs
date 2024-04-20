using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [System.Serializable]
    public class TweenEaseConfig
    {
        [SerializeField, Range(0.01f, 5.0f)] private float _duration = 0.5f;
        [SerializeField] private Ease _ease = Ease.InOutSine;
        
        public float Duration => _duration;
        public Ease Ease => _ease;
    }
}