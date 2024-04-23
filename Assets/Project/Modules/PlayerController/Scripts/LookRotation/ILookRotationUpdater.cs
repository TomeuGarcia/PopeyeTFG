using UnityEngine;

namespace Popeye.Modules.PlayerController.LookRotation
{
    public interface ILookRotationUpdater
    {
        void UpdateLocalRotation(Quaternion goalRotation);
    }
}