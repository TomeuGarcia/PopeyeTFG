using Cysharp.Threading.Tasks;

namespace Popeye.Modules.Camera.CameraZoom
{
    public interface ICameraZoomer
    {
        void ZoomIn(CameraZoomConfig zoomInConfig);
        void ZoomOut(CameraZoomConfig zoomOutConfig);
        void ZoomToDefault(CameraZoomConfig zoomConfig);
        
        UniTaskVoid ZoomInOut(CameraZoomInOutConfig zoomInOutConfig);
        UniTaskVoid ZoomOutIn(CameraZoomInOutConfig zoomInOutConfig);
        
        UniTaskVoid ZoomInOutToDefault(CameraZoomInOutConfig zoomInOutConfig);
        UniTaskVoid ZoomOutInToDefault(CameraZoomInOutConfig zoomInOutConfig);
    }
}