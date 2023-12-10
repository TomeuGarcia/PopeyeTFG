using Cysharp.Threading.Tasks;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerView
    {

        void StartTired();
        void EndTired();
        void PlayTakeDamageAnimation();
        UniTask PlayDeathAnimation();
        UniTask PlayHealAnimation();

    }
}