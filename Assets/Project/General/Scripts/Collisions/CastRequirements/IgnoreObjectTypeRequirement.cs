using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Scripts.Collisions
{
    public class IgnoreObjectTypeRequirement : IPhysicsCastRequirement
    {
        private readonly ObjectTypeAsset _safeGroundIgnoreType;

        public IgnoreObjectTypeRequirement(ObjectTypeAsset safeGroundIgnoreType)
        {
            _safeGroundIgnoreType = safeGroundIgnoreType;
        }

        public bool HitMeetsRequirement(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out IObjectType objectType))
            {
                return !objectType.IsOfType(_safeGroundIgnoreType);
            }

            return true;
        }
    }
}