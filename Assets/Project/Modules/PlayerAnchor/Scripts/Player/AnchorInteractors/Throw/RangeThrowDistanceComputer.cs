using Popeye.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class RangeThrowDistanceComputer : IThrowDistanceComputer
    {
        private readonly AnchorThrowConfig _throwConfig;

        public RangeThrowDistanceComputer(AnchorThrowConfig throwConfig)
        {
            _throwConfig = throwConfig;
        }
        
        public float ComputeThrowDistance(float throwForce01)
        {
            return Mathf.Lerp(_throwConfig.MinThrowDistance, _throwConfig.MaxThrowDistance, 
                throwForce01);
        }

        public void ClearState()
        {
            
        }
    }
}