using Cysharp.Threading.Tasks;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
    public interface IBarrierView
    {
        float ActivateDuration { get; }
        
        UniTask PlayActivateAnimation();
        UniTask PlayDeactivateAnimation();
    }
}