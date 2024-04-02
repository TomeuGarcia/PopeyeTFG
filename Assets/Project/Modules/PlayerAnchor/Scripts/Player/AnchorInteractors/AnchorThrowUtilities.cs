using Popeye.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public static class AnchorThrowUtilities
    {
        public static void CorrectEndRotationForVisibility(AnchorThrowResult throwResult, AnchorThrowConfig throwConfig)
        {
            Vector3 dir = Vector3.ProjectOnPlane(throwResult.EndLookRotation * Vector3.down, Vector3.up).normalized;
            Vector3 toCamera = (Vector3.left + Vector3.back).normalized;

            float dot = Vector3.Dot(dir, toCamera);
            int sign = dot > 0 ? -1 : 1;
            dot = Mathf.Abs(dot);

                
            Vector3 forward = (Vector3.down + toCamera * 0.3f).normalized;
            Vector3 up = toCamera * sign;
            Quaternion lookToCamera = Quaternion.LookRotation(forward, up);

            float t = throwConfig.EndRotationWeightCurve.Evaluate(dot);
            
            throwResult.EndLookRotation =
                Quaternion.LerpUnclamped(throwResult.EndLookRotation, lookToCamera, throwConfig.CorrectionAmount * t);
        }
        
    }
}