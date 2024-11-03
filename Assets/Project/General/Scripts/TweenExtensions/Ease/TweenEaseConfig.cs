using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NaughtyAttributes;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [System.Serializable]
    public class TweenEaseConfig
    {
        [Header("DURATION")]
        [SerializeField, Range(-1.0f, 5.0f)] private float _duration = 0.5f;

        [Header("EASE")] 
        [SerializeField] private bool _useCurve = false;
        public bool UseCurve => _useCurve;
        
        [Expandable] [HideIf("_useCurve")]
        [SerializeField] private Ease _ease = Ease.InOutSine;
        
        [Expandable] [ShowIf("_useCurve")]
        [SerializeField] private AnimationCurve _easeCurve = AnimationCurve.EaseInOut(0,0,1,1);
        
        public float Duration => _duration;
        public Ease Ease => _ease;
        public AnimationCurve EaseCurve => _easeCurve;

        
    }

    
    public static class TweenEaseConfigExtensions
    {
        public static TweenerCore<Vector3,Vector3,VectorOptions> SetEase(
            this TweenerCore<Vector3,Vector3,VectorOptions> target, TweenEaseConfig tweenEaseConfig)
        {
            if (tweenEaseConfig.UseCurve)
            {
                target.SetEase(tweenEaseConfig.EaseCurve);
            }
            else
            {
                target.SetEase(tweenEaseConfig.Ease);
            }
            return target;
        }
        
        public static TweenerCore<Quaternion,Quaternion,NoOptions> SetEase(
            this TweenerCore<Quaternion,Quaternion,NoOptions> target, TweenEaseConfig tweenEaseConfig)
        {
            if (tweenEaseConfig.UseCurve)
            {
                target.SetEase(tweenEaseConfig.EaseCurve);
            }
            else
            {
                target.SetEase(tweenEaseConfig.Ease);
            }
            return target;
        }
    }
    
    
}