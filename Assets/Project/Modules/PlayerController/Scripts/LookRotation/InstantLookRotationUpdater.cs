using UnityEngine;

namespace Popeye.Modules.PlayerController.LookRotation
{
    public class InstantLookRotationUpdater : ILookRotationUpdater
    {
        private readonly Transform _lookTransform;

        public InstantLookRotationUpdater(Transform lookTransform)
        {
            _lookTransform = lookTransform;
        }

        public void UpdateLocalRotation(Quaternion goalRotation)
        {
            _lookTransform.localRotation = goalRotation;
        }
    }
}