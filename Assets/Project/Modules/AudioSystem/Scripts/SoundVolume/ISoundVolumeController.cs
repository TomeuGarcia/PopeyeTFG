namespace Popeye.Modules.AudioSystem.SoundVolume
{
    public interface ISoundVolumeController
    {
        float CurrentVolume { get; }
        void SetVolume(float volumeValue01);
    }
}