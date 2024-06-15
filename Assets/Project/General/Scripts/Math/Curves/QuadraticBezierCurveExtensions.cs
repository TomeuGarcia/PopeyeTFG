using UnityEngine;

namespace Project.Scripts.Math.Curves
{
    public static class QuadraticBezierCurveExtensions
    {
        public static void FillPointsFromCurve(this QuadraticBezierCurve curve, Vector3[] pointsToFill,
            out float distanceBetweenPoints)
        {
            pointsToFill[0] = curve.GetPoint(0);
            
            distanceBetweenPoints = 0f;

            float tStep = 1.0f / (pointsToFill.Length - 1);
            
            for (int i = 1; i < pointsToFill.Length; ++i)
            {
                pointsToFill[i] = curve.GetPoint(tStep * i);

                distanceBetweenPoints += Vector3.Distance(pointsToFill[i - 1], pointsToFill[i]);
            }
        }
    }
}