using System;
using Popeye.ProjectHelpers;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Popeye.Scripts.EditorUtilities
{
    [CreateAssetMenu(fileName = "DistanceFromCameraTransparencyConfig", 
        menuName = ScriptableObjectsHelper.EDITORUTILITIES_ASSETS_PATH + "DistanceFromCameraTransparencyConfig")]
    public class DistanceFromCameraTransparencyConfig : ScriptableObject
    {
        
#if UNITY_EDITOR
        [SerializeField, Range(0f, 200.0f)] private float _startInvisibleDistance = 30.0f;
        [SerializeField, Range(0f, 200.0f)] private float _completelyInvisibleDistance = 50.0f;
        private float _fadingToInvisibleDistance;
        
        [SerializeField] private AnimationCurve _transparencyCurve = AnimationCurve.Linear(0,0,1,1);
        
        private Vector3 CameraPosition => SceneView.currentDrawingSceneView.camera.transform.position;

        private void OnValidate()
        {
            _fadingToInvisibleDistance = _completelyInvisibleDistance - _startInvisibleDistance;
        }

        private void OnEnable()
        {
            OnValidate();
        }

        public float GetTransparencyFromPosition(Vector3 position)
        {
            float distanceFromCamera = Vector3.Distance(position, CameraPosition);

            if (distanceFromCamera < _startInvisibleDistance)
            {
                return 1f;
            }

            if (distanceFromCamera > _completelyInvisibleDistance)
            {
                return 0f;
            }

            float transparency = 1f - ((distanceFromCamera - _startInvisibleDistance) / _fadingToInvisibleDistance);

            transparency = _transparencyCurve.Evaluate(transparency);
            
            return transparency;
        }

        public Color ApplyTransparencyToColor(Color color, Vector3 position)
        {
            Color colorWithTransparency = color;
            colorWithTransparency.a = GetTransparencyFromPosition(position);
            return colorWithTransparency;
        }
        
#endif
    }
}