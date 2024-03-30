using Cysharp.Threading.Tasks;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public interface IPlayerAbilityUnlockerView
    {
        UniTaskVoid PlayIdleAnimation();
        UniTaskVoid PlayUnlockAbilityAnimation();
    }
}