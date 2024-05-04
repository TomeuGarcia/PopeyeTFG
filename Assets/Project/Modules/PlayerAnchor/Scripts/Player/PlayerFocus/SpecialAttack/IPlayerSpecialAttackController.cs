namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public interface IPlayerSpecialAttackController
    {
        float PreparationDuration { get; }
        string Name { get; }
        
        bool CanDoSpecialAttack();
        void StartSpecialAttack();
        bool SpecialAttackHasFinished();

        void ForceStopSpecialAttack();
    }
}