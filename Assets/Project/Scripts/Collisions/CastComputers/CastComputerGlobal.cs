using UnityEngine;

namespace Popeye.Scripts.Collisions
{
    public class CastComputerGlobal : ICastComputer
    {
        private readonly Transform _castOriginTransform;
        private readonly Vector3 _originOffset;
        private readonly Vector3 _castDirection;

        
        public CastComputerGlobal(Transform castOriginTransform,
            Vector3 originOffset, Vector3 castDirection)
        {
            _castOriginTransform = castOriginTransform;
            _originOffset = originOffset;
            _castDirection = castDirection;
        }

        public Vector3 ComputeCastOrigin()
        {
            Vector3 origin = _castOriginTransform.position;
            origin += _originOffset;

            return origin;
        }

        public Vector3 ComputeCastDirection()
        {
            return _castDirection;
        }
    }
}