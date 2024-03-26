using Popeye.Modules.AudioSystem.SoundVolume;

namespace Project.Modules.AudioSystem.Scripts.SoundVolume
{
    public class SoundVolumeControllersGroup
    {
        public ISoundVolumeController MasterVolumeController { get; private set; }
        public ISoundVolumeController MusicVolumeController { get; private set; }
        public ISoundVolumeController AmbientVolumeController { get; private set; }
        public ISoundVolumeController SFXVolumeController { get; private set; }

        
        public SoundVolumeControllersGroup(
            ISoundVolumeController masterVolumeController,
            ISoundVolumeController musicVolumeController,
            ISoundVolumeController ambientVolumeController,
            ISoundVolumeController sfxVolumeController)
        {
            MasterVolumeController = masterVolumeController;
            MusicVolumeController = musicVolumeController;
            AmbientVolumeController = ambientVolumeController;
            SFXVolumeController = sfxVolumeController;
        }
        
        
    }
}