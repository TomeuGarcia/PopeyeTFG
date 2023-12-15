using Cysharp.Threading.Tasks;

namespace Popeye.Modules.Camera.CameraShake
{
    public interface ICameraShaker
    {
        UniTaskVoid PlayShake(CameraShakeConfig shakeConfig);
    }
}