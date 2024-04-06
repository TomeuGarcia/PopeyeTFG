namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public interface IPlayerSpecialAttackController
    {
        bool CanDoSpecialAttack();
        bool SpecialAttackIsBeingPerformed();
        void StartSpecialAttack();
    }
}