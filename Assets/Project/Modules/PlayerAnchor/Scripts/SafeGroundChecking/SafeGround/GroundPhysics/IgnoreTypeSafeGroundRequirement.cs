using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking
{
    public class IgnoreTypeSafeGroundRequirement : ISafeGroundPhysicsRequirement
    {
        private readonly ObjectTypeAsset _safeGroundIgnoreType;

        public IgnoreTypeSafeGroundRequirement(ObjectTypeAsset safeGroundIgnoreType)
        {
            _safeGroundIgnoreType = safeGroundIgnoreType;
        }

        public bool MeetsRequirement(RaycastHit groundHit)
        {
            if (groundHit.collider.TryGetComponent(out IObjectType objectType))
            {
                return !objectType.IsOfType(_safeGroundIgnoreType);
            }

            return true;
        }
    }
}