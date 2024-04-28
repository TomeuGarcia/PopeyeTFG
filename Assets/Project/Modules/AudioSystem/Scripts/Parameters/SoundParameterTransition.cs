using Cysharp.Threading.Tasks;
using Popeye.Timers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [System.Serializable]
    public class SoundParameterTransition
    {
        [SerializeField] private float _parameterValue = 0;
        [SerializeField] private TweenEaseConfig _transitionConfig;

        public async UniTaskVoid Transition(SoundParameter soundParameter)
        {
            float startValue = soundParameter.Value;
                
            Timer timer = new Timer(_transitionConfig.Duration);
            while (!timer.HasFinished())
            {
                timer.Update(Time.deltaTime);
                float t = timer.GetCounterRatio01();

                if (_transitionConfig.UseCurve)
                {
                    t = _transitionConfig.EaseCurve.Evaluate(t);
                }

                float value = Mathf.LerpUnclamped(startValue, _parameterValue, t);
                soundParameter.SetValue(value);
                            
                await UniTask.Yield();
            }
        }
    }
}