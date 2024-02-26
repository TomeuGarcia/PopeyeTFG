using System;
using FMODUnity;

namespace Popeye.Modules.AudioSystem
{
    public interface ILastingFMODSound
    {
        public EventReference EventReference { get; } 
        public SoundParameter[] Parameters { get; } 
        public Guid Id { get; } 
    }
}