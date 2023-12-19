using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;

namespace Popeye.Modules.Camera
{
    public interface ICameraFunctionalities
    {
        public ICameraShaker CameraShaker { get;}
        public ICameraZoomer CameraZoomer { get;}
    }
}