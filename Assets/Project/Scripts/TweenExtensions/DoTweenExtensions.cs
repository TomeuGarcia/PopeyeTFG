using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.TweenExtensions
{
    public static class DoTweenExtensions
    {
        // SCALE
        public static Tweener PunchScale(this Transform target, TweenPunchConfig punchConfig, bool completeBeforeApplying = false)
        {
            if (completeBeforeApplying)
            {
                target.DOComplete();
            }
            
            return target.DOPunchScale(punchConfig.Value, punchConfig.Duration, punchConfig.Vibrato, punchConfig.Elasticity)
                .SetEase(punchConfig.Ease);
        }

        
        // COLOR
        public static Tweener PunchColor(this Graphic target, TweenColorConfig punchConfig)
        {
            Color originalColor = target.color;
            return target.DOColor(punchConfig.Value, punchConfig.Duration).SetEase(punchConfig.Ease)
                .OnComplete(() => target.DOColor(originalColor, punchConfig.Duration).SetEase(punchConfig.Ease));
        }
        
        
        
    }
}