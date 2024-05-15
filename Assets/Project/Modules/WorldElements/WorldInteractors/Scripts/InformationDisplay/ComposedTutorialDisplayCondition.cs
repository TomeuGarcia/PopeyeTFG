using System;
using Popeye.Scripts.EventChannels;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
    public class ComposedTutorialDisplayCondition : ITutorialDisplayCondition
    {
        private readonly ITutorialDisplayCondition[] _subConditions;

        public ComposedTutorialDisplayCondition(ITutorialDisplayCondition[] subConditions)
        {
            _subConditions = subConditions;
        }
        
        public void StartChecking(IEmptyEventChannelListenEntry.ChannelEvent onConditionMetCallback)
        {
            foreach (ITutorialDisplayCondition subCondition in _subConditions)
            {
                subCondition.StartChecking(onConditionMetCallback);
            }
        }

        public void FinishChecking()
        {
            foreach (ITutorialDisplayCondition subCondition in _subConditions)
            {
                subCondition.FinishChecking();
            }
        }
    }
}