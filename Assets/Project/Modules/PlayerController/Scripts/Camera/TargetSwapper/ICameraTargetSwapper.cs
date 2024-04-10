using Cysharp.Threading.Tasks;
using Popeye.Modules.Camera.Target;

namespace Popeye.Modules.Camera.TargetSwapper
{
    public interface ICameraTargetSwapper
    {
        void RestoreOriginalCameraTarget();
        UniTask SwapCameraTarget(CameraTarget cameraTarget);
    }
}