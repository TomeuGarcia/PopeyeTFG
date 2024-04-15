using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IHazardDispenserView
    {
        void Configure(HazardDispenserViewConfig config);
        void PlayPrepareDispensingAnimation(float duration);
        void PlayDispenseAnimation();
        UniTask PlayReadyToDispenseAnimation();
    }
}