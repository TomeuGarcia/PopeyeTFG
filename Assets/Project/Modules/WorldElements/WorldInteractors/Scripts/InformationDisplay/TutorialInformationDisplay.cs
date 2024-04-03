using AYellowpaper;
using Popeye.Core.Services.InformationDisplay;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Scripts.EventChannels;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
    public class TutorialInformationDisplay : AWorldInteractor
    {
        private class DisplayConditionData
        {
            private IEmptyEventChannelListenEntry _performedActionChannel;
            private Counter _performedAmountCounter;
            private IEmptyEventChannelListenEntry.ChannelEvent _conditionMetCallback;
            private bool _isSubscribed;

            public DisplayConditionData(IEmptyEventChannelListenEntry performedActionChannel, int performedTimes)
            {
                _performedActionChannel = performedActionChannel;
                _performedAmountCounter = new Counter(performedTimes);
                _isSubscribed = false;
            }
            public void Start(IEmptyEventChannelListenEntry.ChannelEvent conditionMetCallback)
            {
                _conditionMetCallback = conditionMetCallback;
                SubscribeToChannel();
            }

            public void Finish()
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

        
        
        private DisplayConditionData _stopDisplayingCondition;
        private TextDisplayConfig _informationToDisplay;
        
        private IInformationDisplayService _informationDisplayService;
        
        
        protected override void AwakeInit() { }

        public void Configure(TextDisplayConfig informationToDisplay, 
            IEmptyEventChannelListenEntry actionToStopShowing, int performedTimesToStopShowing)
        {
            _informationToDisplay = informationToDisplay;
            _stopDisplayingCondition = new DisplayConditionData(actionToStopShowing, performedTimesToStopShowing);
            
            _informationDisplayService = ServiceLocator.Instance.GetService<IInformationDisplayService>();            
        }

        private void OnDestroy()
        {
            _stopDisplayingCondition.Finish();
        }

        protected override void EnterActivatedState()
        {
            StartShowing();
        }

        protected override void EnterDeactivatedState()
        {
            _stopDisplayingCondition.Finish();
        }

        private void StartShowing()
        {
            _informationDisplayService.TextDisplayer.StartShowing(_informationToDisplay);            
            _stopDisplayingCondition.Start(StopShowing);
        }
        private void StopShowing()
        {
            _informationDisplayService.TextDisplayer.StopShowing(_informationToDisplay);
        }

    }
}