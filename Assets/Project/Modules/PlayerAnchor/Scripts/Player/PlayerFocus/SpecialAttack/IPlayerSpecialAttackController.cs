namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public interface IPlayerSpecialAttackController
    {
        float PreparationDuration { get; }
        
        bool CanDoSpecialAttack();
        void StartSpecialAttack();
        bool SpecialAttackHasFinished();

        void ForceStopSpecialAttack();
    }
}