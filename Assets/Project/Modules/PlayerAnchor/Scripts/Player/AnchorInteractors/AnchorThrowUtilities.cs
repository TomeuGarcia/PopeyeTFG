using Popeye.Modules.PlayerAnchor.Anchor;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public static class AnchorThrowUtilities
    {
        public static void CorrectEndRotationForVisibility(AnchorThrowResult throwResult, AnchorThrowConfig.RotationCorrection config)
        {
            throwResult.EndLookRotation = CorrectRotationForVisibility(throwResult.EndLookRotation, config);
        }
        
        public static Quaternion CorrectRotationForVisibility(Quaternion rotation, AnchorThrowConfig.RotationCorrection config)
        {
            Vector3 dir = Vector3.ProjectOnPlane(rotation * Vector3.down, Vector3.up).normalized;
            Vector3 toCamera = (Vector3.left + Vector3.back).normalized;

            float dot = Vector3.Dot(dir, toCamera);
            bool isGoingToCamera = dot > 0;
            int sign = isGoingToCamera ? -1 : 1;
            dot = Mathf.Abs(dot);


            float toCameraWeight = isGoingToCamera ? config.FrontToCameraWeight : config.BackToCameraWeight;
            
            Vector3 forward = (Vector3.down + toCamera * toCameraWeight).normalized;
            Vector3 up = toCamera * sign;
            Quaternion lookToCamera = Quaternion.LookRotation(forward, up);

            float t = config.WeightCurve.Evaluate(dot);
            
            return Quaternion.LerpUnclamped(rotation, lookToCamera, config.CorrectionAmount * t);
        }
        
        
        
    }
}