namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public interface IPlayerSpecialAttackController
    {
        float PreparationDuration { get; }
        string Name { get; }
        
        void OnPreparationStart(float durationToComplete);
        void OnPreparationInterrupted();
        bool CanDoSpecialAttack();
        void StartSpecialAttack();
        bool SpecialAttackHasFinished();

        void ForceStopSpecialAttack();
    }
}