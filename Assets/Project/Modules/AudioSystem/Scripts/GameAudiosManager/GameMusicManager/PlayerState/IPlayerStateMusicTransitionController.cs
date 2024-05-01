namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    public interface IPlayerStateMusicTransitionController
    {
        void TransitionToDefault();
        void TransitionToDeath();
        void TransitionToBattle();
        void TransitionOutOfBattle();
        void TransitionToTakingDamage();
    }
}