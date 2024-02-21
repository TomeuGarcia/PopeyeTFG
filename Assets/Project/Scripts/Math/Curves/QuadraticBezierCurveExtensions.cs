using UnityEngine;

namespace Project.Scripts.Math.Curves
{
    public static class QuadraticBezierCurveExtensions
    {
        public static void FillPointsFromCurve(this QuadraticBezierCurve curve, Vector3[] trajectoryPoints,
            out float trajectoryDistance)
        {
            trajectoryPoints[0] = curve.GetPoint(0);
            
            trajectoryDistance = 0f;

            float tStep = 1.0f / (trajectoryPoints.Length - 1);
            
            for (int i = 1; i < trajectoryPoints.Length; ++i)
            {
                trajectoryPoints[i] = curve.GetPoint(tStep * i);

                trajectoryDistance += Vector3.Distance(trajectoryPoints[i - 1], trajectoryPoints[i]);
            }
        }
    }
}