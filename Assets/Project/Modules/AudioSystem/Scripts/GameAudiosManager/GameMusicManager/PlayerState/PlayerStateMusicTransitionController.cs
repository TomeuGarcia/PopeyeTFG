namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    public class PlayerStateMusicTransitionController : IPlayerStateMusicTransitionController
    {
        private readonly PlayerStateMusicConfig _playerStateMusicConfig;
        

        public PlayerStateMusicTransitionController(PlayerStateMusicConfig playerStateMusicConfig)
        {
            _playerStateMusicConfig = playerStateMusicConfig;
            TransitionToDefault();
        }


        public void TransitionToDefault()
        {
            _playerStateMusicConfig.TransitionToExploration();
        }

        public void TransitionToDeath()
        {
            _playerStateMusicConfig.TransitionToDeath();
        }

        public void TransitionToBattle()
        {
            _playerStateMusicConfig.TransitionToBattle();
        }

        public void TransitionOutOfBattle()
        {
            _playerStateMusicConfig.TransitionToExploration();
        }
    }
}