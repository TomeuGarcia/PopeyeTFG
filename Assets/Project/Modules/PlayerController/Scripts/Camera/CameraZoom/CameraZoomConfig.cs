using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Camera.CameraZoom
{
    [CreateAssetMenu(fileName = "CameraZoomConfig", 
        menuName = ScriptableObjectsHelper.CAMERA_ASSETS_PATH + "CameraZoomConfig")]
    public class CameraZoomConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 200.0f)] private float _zoomDistance = 10.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _easeCurve = AnimationCurve.Linear(0,0,1,1);
        
        public float ZoomDistance => _zoomDistance;
        public float Duration => _duration;
        public AnimationCurve EaseCurve => _easeCurve;

        public void SetDuration(float newDuration)
        {
            _duration = newDuration;
        }
    }
}