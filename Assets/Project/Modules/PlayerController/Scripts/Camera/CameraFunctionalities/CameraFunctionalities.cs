using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using Popeye.Modules.Camera.TargetSwapper;

namespace Popeye.Modules.Camera
{
    public class CameraFunctionalities : ICameraFunctionalities
    {
        public ICameraZoomer CameraZoomer { get; private set; }
        public ICameraShaker CameraShaker { get; private set; }
        public ICameraTargetSwapper CameraTargetSwapper { get; private set; }


        public CameraFunctionalities(ICameraZoomer cameraZoomer, ICameraShaker cameraShaker, ICameraTargetSwapper cameraTargetSwapper)
        {
            CameraZoomer = cameraZoomer;
            CameraShaker = cameraShaker;
            CameraTargetSwapper = cameraTargetSwapper;
        }
    }
}