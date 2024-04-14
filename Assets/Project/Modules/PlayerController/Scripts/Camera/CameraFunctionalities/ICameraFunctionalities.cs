using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using Popeye.Modules.Camera.TargetSwapper;

namespace Popeye.Modules.Camera
{
    public interface ICameraFunctionalities
    {
        public ICameraShaker CameraShaker { get;}
        public ICameraZoomer CameraZoomer { get;}
        public ICameraTargetSwapper CameraTargetSwapper { get;}
    }
}