using Popeye.Scripts.EventChannels;
using Popeye.Timers;

namespace Popeye.Modules.WorldElements.WorldInteractors
{

    public class PerformedAmountDisplayCondition : ITutorialDisplayCondition
    {
        private IEmptyEventChannelListenEntry _performedActionChannel;
        private Counter _performedAmountCounter;
        private IEmptyEventChannelListenEntry.ChannelEvent _conditionMetCallback;
        private bool _isSubscribed;

        public PerformedAmountDisplayCondition(IEmptyEventChannelListenEntry performedActionChannel, int performedTimes)
        {
            _performedActionChannel = performedActionChannel;
            _performedAmountCounter = new Counter(performedTimes);
            _isSubscribed = false;
        }
        public void StartChecking(IEmptyEventChannelListenEntry.ChannelEvent conditionMetCallback)
        {
            _conditionMetCallback = conditionMetCallback;
            SubscribeToChannel();
        }

        public void FinishChecking()
        {
            UnsubscribeToChannel();
        }

        private void SubscribeToChannel()
        {
            if (_isSubscribed) return;
            _isSubscribed = true;
            
            _performedActionChannel.Subscribe(CheckCondition);
        }
        
        private void UnsubscribeToChannel()
        {
            if (!_isSubscribed) return;
            _isSubscribed = false;
            
            _performedActionChannel.Unsubscribe(CheckCondition);
        }
        
        private void CheckCondition()
        {
            _performedAmountCounter.Add(1);
            if (_performedAmountCounter.HasReachedTotal())
            {
                _conditionMetCallback();
                UnsubscribeToChannel();
            }
        }
    }

    
}