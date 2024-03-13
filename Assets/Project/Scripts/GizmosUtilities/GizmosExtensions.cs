#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Popeye.Scripts.GizmosUtilities
{
    public static class GizmosExtensions
    {
        public static void GizmosDrawArrow(Vector3 start, Vector3 end, Color color, 
            float angle = 30f, float capDistance = 0.5f)
        {
            ComputeArrowPoints(start, end, angle, capDistance, out Vector3 arrowStartA, out Vector3 arrowStartB);

            Gizmos.color = color;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawLine(arrowStartA, end);
            Gizmos.DrawLine(arrowStartB, end);
        }
        
        
#if UNITY_EDITOR
        public static void HandlesDrawArrow(Vector3 start, Vector3 end, Color color, 
            float thickness = 1.0f, float angle = 30f, float capDistance = 0.5f)
        {
            ComputeArrowPoints(start, end, angle, capDistance, out Vector3 arrowStartA, out Vector3 arrowStartB);

            Handles.color = color;
            Handles.DrawLine(start, end, thickness);
            Handles.DrawLine(arrowStartA, end, thickness);
            Handles.DrawLine(arrowStartB, end, thickness);
        }
        
#endif

        private static void ComputeArrowPoints(Vector3 start, Vector3 end, float angle, float capDistance,
            out Vector3 arrowStartA, out Vector3 arrowStartB)
        {
            Vector3 endToStartDirection = (start - end).normalized;


            Vector3 sideAxis = Vector3.right;
            float dot = Vector3.Dot(sideAxis, endToStartDirection);
            if (Mathf.Abs(dot) > 0.99f)
            {
                sideAxis = Vector3.forward;
            }
            
            Vector3 forwardAxis = Vector3.Cross(endToStartDirection, sideAxis).normalized;
            Vector3 rotationAxis = Vector3.Cross(endToStartDirection, forwardAxis).normalized;

            Quaternion rotation = Quaternion.AngleAxis(angle, rotationAxis);
            arrowStartA = end + (rotation * (endToStartDirection * capDistance));
            
            rotation = Quaternion.AngleAxis(-angle, rotationAxis);
            arrowStartB = end + (rotation * (endToStartDirection * capDistance));
        }
        
        
    }
}