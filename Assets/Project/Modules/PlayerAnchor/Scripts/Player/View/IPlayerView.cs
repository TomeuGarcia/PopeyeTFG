using Cysharp.Threading.Tasks;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerView
    {

        void StartTired();
        void EndTired();
        void PlayTakeDamageAnimation();
        void PlayRespawnAnimation();
        UniTask PlayDeathAnimation();
        UniTask PlayHealAnimation();
        void PlayDashAnimation(float duration);
        void PlayKickAnimation();
        void PlayThrowAnimation();
        UniTaskVoid PlayPullAnimation(float duration);
        
        void PlayAnchorObstructedAnimation();

    }
}