using System;

namespace Popeye.Core.Services.EventSystem
{
    public interface IEventSystemService
    {
        void Subscribe<TEvent>(Action<TEvent> callback);
        void Unsubscribe<TEvent>(Action<TEvent> callback);
        void Dispatch<TEvent>(TEvent eventData);
    }
}