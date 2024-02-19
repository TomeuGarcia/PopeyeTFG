using UnityEngine;

namespace Popeye.Scripts.Collisions
{
    public class CastComputerLocal : ICastComputer
    {
        private readonly Transform _castOriginTransform;
        private readonly Vector3 _originLocalOffset;
        private readonly Vector3 _castLocalDirection;

        
        public CastComputerLocal(Transform castOriginTransform,
            Vector3 originLocalOffset, Vector3 castLocalDirection)
        {
            _castOriginTransform = castOriginTransform;
            _originLocalOffset = originLocalOffset;
            _castLocalDirection = castLocalDirection;
        }

        public Vector3 ComputeCastOrigin()
        {
            Vector3 origin = _castOriginTransform.position;
            origin += _castOriginTransform.rotation * _originLocalOffset;

            return origin;
        }

        public Vector3 ComputeCastDirection()
        {
            Vector3 direction = _castOriginTransform.rotation * _castLocalDirection;

            return direction;
        }
    }
}