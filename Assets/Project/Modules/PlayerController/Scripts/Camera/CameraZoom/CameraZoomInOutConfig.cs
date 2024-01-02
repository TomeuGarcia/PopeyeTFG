using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.Camera.CameraZoom
{
    [CreateAssetMenu(fileName = "CameraZoomInOutConfig", 
        menuName = ScriptableObjectsHelper.CAMERA_ASSETS_PATH + "CameraZoomInOutConfig")]
    public class CameraZoomInOutConfig : ScriptableObject
    {
        [SerializeField] private CameraZoomConfig _zoomInConfig;
        [SerializeField] private CameraZoomConfig _zoomOutConfig;
        [SerializeField, Range(0.0f, 10.0f)] private float delayBetweenZooms = 0.2f;
        
        public CameraZoomConfig ZoomInConfig => _zoomInConfig;
        public CameraZoomConfig ZoomOutConfig => _zoomOutConfig;
        public float DelayBetweenZooms => delayBetweenZooms;
    }
}