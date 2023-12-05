namespace Popeye.Modules.PlayerAnchor.Player
{
    public interface IPlayerView
    {

        void StartTired();
        void EndTired();
        void PlayTakeDamageAnimation();
        void PlayDeathAnimation();
        void PlayHealAnimation();

    }
}