
namespace Popeye.Scripts.EventChannels
{
    public interface IEmptyEventChannelListenEntry
    {
        delegate void ChannelEvent();
        void Subscribe(ChannelEvent callback);
        void Unsubscribe(ChannelEvent callback);
    }
}