using Popeye.Scripts.EventChannels;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
    public interface ITutorialDisplayCondition
    {
        public enum Type
        {
            TimesPerformed,
            Duration
        }
        
        void StartChecking(IEmptyEventChannelListenEntry.ChannelEvent onConditionMetCallback);
        void FinishChecking();
    }
}