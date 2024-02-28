using Popeye.Modules.PlayerAnchor.Anchor;

namespace Popeye.Modules.PlayerAnchor.Player.EnemyInteractions
{
    public class PlayerEnemySpawnerInteractions : IPlayerEnemySpawnersInteractions
    {
        private readonly IPlayerMediator _playerMediator;
        private readonly IAnchorMediator _anchorMediator;

        public PlayerEnemySpawnerInteractions(IPlayerMediator playerMediator, IAnchorMediator anchorMediator)
        {
            _playerMediator = playerMediator;
            _anchorMediator = anchorMediator;
        }
        
        
        public void OnSpawnerTrapActivated()
        {
            if (!_anchorMediator.IsBeingCarried() && !_anchorMediator.IsBeingPulled())
            {
                _playerMediator.QueuePullAnchor().Forget();
            }

        }
        
        
    }
}