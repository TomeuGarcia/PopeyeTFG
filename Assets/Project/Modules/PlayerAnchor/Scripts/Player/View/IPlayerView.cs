using Cysharp.Threading.Tasks;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerView
    {

        void StartTired();
        void EndTired();
        void PlayTakeDamageAnimation();
        void PlayRespawnAnimation();
        void PlayDeathAnimation();
        void PlayHealAnimation();
        void PlayDashAnimation(float duration);
        void PlayKickAnimation();
        void PlayThrowAnimation();
        UniTaskVoid PlayPullAnimation(float delay);
        
        void PlayAnchorObstructedAnimation();
        
    }
}