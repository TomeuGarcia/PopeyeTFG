using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    public static class DoTweenExtensions
    {
        // SCALE
        public static Tweener Scale(this Transform target, TweenConfig config, bool completeBeforeApplying = false)
        {
            if (completeBeforeApplying)
            {
                target.DOComplete();
            }
            
            return target.DOScale(config.Value, config.Duration)
                .SetEase(config.Ease);
        }
        
        // ROTATE
        public static Tweener Rotate(this Transform target, TweenConfig config, bool completeBeforeApplying = false)
        {
            if (completeBeforeApplying)
            {
                target.DOComplete();
            }
            
            return target.DORotate(config.Value, config.Duration)
                .SetEase(config.Ease);
        }
        public static Tweener LocalRotate(this Transform target, TweenConfig config, bool completeBeforeApplying = false)
        {
            if (completeBeforeApplying)
            {
                target.DOComplete();
            }
            
            return target.DOLocalRotate(config.Value, config.Duration)
                .SetEase(config.Ease);
        }
        
        // MOVE
        public static Tweener Move(this Transform target, TweenConfig config, bool completeBeforeApplying = false)
        {
            if (completeBeforeApplying)
            {
                target.DOComplete();
            }
            
            return target.DOMove(config.Value, config.Duration)
                .SetEase(config.Ease);
        }
        public static Tweener BlendableLocalMoveBy(this Transform target, TweenConfig config, bool completeBeforeApplying = false)
        {
            if (completeBeforeApplying)
            {
                target.DOComplete();
            }
            
            return target.DOBlendableLocalMoveBy(config.Value, config.Duration)
                .SetEase(config.Ease);
            
        }
        public static Tweener FuckMe(this Transform target, TweenConfig config, bool completeBeforeApplying = false)
        {
            if (completeBeforeApplying)
            {
                target.DOComplete();
            }
            
            return target.DOBlendableLocalMoveBy(config.Value, config.Duration)
                .SetEase(config.Ease);
            
        }
        
        
    }
}