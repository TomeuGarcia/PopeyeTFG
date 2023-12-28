using Popeye.Modules.Enemies;
using Popeye.Modules.Utilities;

namespace Popeye.Modules.PlayerController.Scripts.Enemies
{
    public class SpiderSpiderEnemyStunnedState : ISpiderEnemyState
    {
        private Timer _stunnedTimer;
        private SpiderEnemy _spiderEnemy;

        public SpiderSpiderEnemyStunnedState(SpiderEnemy spiderEnemy)
        {
            _spiderEnemy = spiderEnemy;
            _stunnedTimer = new Timer(0.5f);
        }
        
        protected override void DoEnter()
        {
            _stunnedTimer.SetDuration(_spiderEnemy.stunTime);
            _stunnedTimer.Clear();

            _spiderEnemy.GetStunned(_spiderEnemy.stunTime);
        }

        public override void Exit()
        {
            
        }

        public override void Interrupt()
        {
            
        }

        public override bool Update(float deltaTime)
        {
            _stunnedTimer.Update(deltaTime);

            if (_stunnedTimer.HasFinished())
            {
                _nextState = States.Chasing;
                return true;
            }
            
            return false;
        }
        
        

    }
}