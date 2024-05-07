using Cysharp.Threading.Tasks;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
    public class NullBarrierView : IBarrierView
    {
        public float ActivateDuration => 0;
        
        public async UniTask PlayActivateAnimation()
        {
            
        }

        public async UniTask PlayDeactivateAnimation()
        {
            
        }
    }
}