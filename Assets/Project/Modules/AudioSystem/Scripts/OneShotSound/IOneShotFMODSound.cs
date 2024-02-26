using FMODUnity;

namespace Popeye.Modules.AudioSystem
{
    public interface IOneShotFMODSound
    {
        public EventReference EventReference { get; } 
        public SoundParameter[] Parameters { get; } 
    }
}