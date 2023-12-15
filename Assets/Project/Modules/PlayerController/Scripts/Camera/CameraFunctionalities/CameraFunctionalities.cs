using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;

namespace Popeye.Modules.Camera
{
    public class CameraFunctionalities : ICameraFunctionalities
    {
        public ICameraZoomer CameraZoomer { get; private set; }
        public ICameraShaker CameraShaker { get; private set; }


        public CameraFunctionalities(ICameraZoomer cameraZoomer, ICameraShaker cameraShaker)
        {
            CameraZoomer = cameraZoomer;
            CameraShaker = cameraShaker;
        }
    }
}