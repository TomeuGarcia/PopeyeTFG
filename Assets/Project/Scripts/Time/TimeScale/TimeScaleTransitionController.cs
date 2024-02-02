using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.Core.Transitions;

namespace Project.Scripts.Time.TimeScale
{
    public class TimeScaleTransitionController
    {
        
        public async UniTask ApplySmoothTimeScale(ITimeScaleManager timeScaleManager, 
            float midpointTimeScale, TransitionConfig transitionConfig)
        {
            float originalTimeScale = timeScaleManager.CurrentTimeScale;
            float currentTimeScale = originalTimeScale;
            
            await DOTween.To(
                () => currentTimeScale,
                (value) => { currentTimeScale = value; timeScaleManager.SetTimeScale(currentTimeScale);},
                midpointTimeScale,
                transitionConfig.TransitionInDuration
            )
                .SetUpdate(true)
                .SetEase(transitionConfig.TransitionInEase)
                .AsyncWaitForCompletion();


            await UniTask.Delay(TimeSpan.FromSeconds(transitionConfig.MidpointDuration), ignoreTimeScale: true);
            
            
            await DOTween.To(
                () => currentTimeScale,
                (value) => { currentTimeScale = value; timeScaleManager.SetTimeScale(currentTimeScale);},
                originalTimeScale,
                transitionConfig.TransitionOutDuration
            )
                .SetUpdate(true)
                .SetEase(transitionConfig.TransitionOutEase)
                .AsyncWaitForCompletion();
        }
    }
}