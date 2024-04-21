using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IHazardDispenserAudio
    {
        void PlayPrepareSound(GameObject source);
        void PlayDispenseSound(GameObject source);
    }
}