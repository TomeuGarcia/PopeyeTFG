using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IHazardDispenserAudio
    {
        void Configure();
        void PlayPrepareSound(GameObject source);
        void PlayDispenseSound(GameObject source);
    }
}