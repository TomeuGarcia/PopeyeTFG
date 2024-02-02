using Popeye.Modules.PlayerController.AutoAim;
using UnityEngine;

namespace Popeye.Modules.PlayerController.Inputs
{
    public class AutoAimInputCorrector : IInputCorrector
    {
        private readonly AutoAimController _autoAimController;

        
        public AutoAimInputCorrector(AutoAimController autoAimController)
        {
            _autoAimController = autoAimController;
        }
        
        public Vector3 CorrectLookInput(Vector3 lookInput, Vector3 forwardDirection, Vector3 rightDirection)
        {
            float lookX = GetAngleFromDirection(lookInput, forwardDirection, rightDirection);
            float lookY = _autoAimController.CorrectLookAngle(lookX, forwardDirection, rightDirection);
            
            return GetDirectionFromAngle(lookY, forwardDirection, rightDirection);
        }
        
        private float GetAngleFromDirection(Vector3 direction, Vector3 forwardDirection, Vector3 rightDirection)
        {
            float angle = Mathf.Acos(Vector3.Dot(forwardDirection, direction)) * Mathf.Rad2Deg;

            return Vector3.Dot(rightDirection, direction) < 0 ? 
                360 - angle :
                angle;
        }
        
        private Vector3 GetDirectionFromAngle(float angle, Vector3 forwardDirection, Vector3 rightDirection)
        {
            Vector3 axis = Vector3.Cross(forwardDirection, rightDirection).normalized;

            return Quaternion.AngleAxis(angle, axis) * forwardDirection;
        }
        
    }
}