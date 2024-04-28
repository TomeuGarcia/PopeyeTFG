using System;
using Cysharp.Threading.Tasks;
using Popeye.Scripts.EventChannels;
using Popeye.Timers;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
    public class DurationDisplayCondition : ITutorialDisplayCondition
    {
        private readonly float _waitDuration;
        private IEmptyEventChannelListenEntry.ChannelEvent _conditionMetCallback;


        public DurationDisplayCondition(float waitDuration)
        {
            _waitDuration = waitDuration;
        }
        
        public void StartChecking(IEmptyEventChannelListenEntry.ChannelEvent onConditionMetCallback)
        {
            _conditionMetCallback = onConditionMetCallback;
            RiseAfterWaitDuration().Forget();
        }

        public void FinishChecking()
        {
            
        }

        private async UniTaskVoid RiseAfterWaitDuration()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_waitDuration));
            _conditionMetCallback();
        }
        
    }
}