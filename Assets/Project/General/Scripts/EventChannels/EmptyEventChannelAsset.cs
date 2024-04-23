using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Scripts.EventChannels
{
    [CreateAssetMenu(fileName = "EmptyEventChannel_NAME", 
        menuName = ScriptableObjectsHelper.EVENTCHANNELS_ASSETS_PATH + "EmptyEventChannel")]
    public class EmptyEventChannelAsset : ScriptableObject, IEmptyEventChannelListenEntry, IEmptyEventChannelDispatcher
    {
        private IEmptyEventChannelListenEntry.ChannelEvent OnRequest;
        
        [SerializeField] private bool _logWarining = true;
        
        public void Subscribe(IEmptyEventChannelListenEntry.ChannelEvent callback)
        {
            OnRequest += callback;
        }

        public void Unsubscribe(IEmptyEventChannelListenEntry.ChannelEvent callback)
        {
            OnRequest -= callback;
        }
        
        
        public void RaiseEvent()
        {
            if (OnRequest != null)
            {
                OnRequest.Invoke();
            }
            else if (_logWarining)
            {
                Debug.LogWarning( $"An Event was requested but nobody picked it up [{name}]");
            }
        }
        
        
    }
}