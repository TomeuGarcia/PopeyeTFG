using Popeye.Modules.Enemies;
using Popeye.Modules.Utilities;

namespace Popeye.Modules.PlayerController.Scripts.Enemies
{
    public class EnemyStunnedState : IEnemyState
    {
        private Timer _stunnedTimer;
        private Enemy _enemy;

        public EnemyStunnedState(Enemy enemy)
        {
            _enemy = enemy;
            _stunnedTimer = new Timer(0.5f);
        }
        
        protected override void DoEnter()
        {
            _stunnedTimer.SetDuration(_enemy.stunTime);
            _stunnedTimer.Clear();

            _enemy.GetStunned(_enemy.stunTime);
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