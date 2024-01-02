using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.StateMachine
{
    public class SpiderSpiderEnemyIdleState : ISpiderEnemyState
    {
        private SpiderEnemy _spiderEnemy;
        private float _chaseStartDistance;

        private float _minIdleDuration;
        private float _idleTimer;

        public SpiderSpiderEnemyIdleState(SpiderEnemy spiderEnemy, float chaseStartDistance, float minIdleDuration)
        {
            _spiderEnemy = spiderEnemy;
            _chaseStartDistance = chaseStartDistance;
            _minIdleDuration = minIdleDuration;
        }


        protected override void DoEnter()
        {
            _idleTimer = 0.0f;
            _spiderEnemy.SetMaxMoveSpeed(0.0f);
        }

        public override void Exit()
        {
            _spiderEnemy.SetMaxMoveSpeed(_spiderEnemy.MaxMoveSpeed);
        }

        public override void Interrupt()
        {

        }

        public override bool Update(float deltaTime)
        {
            _idleTimer += deltaTime;
            if (_idleTimer < _minIdleDuration)
            {
                return false;
            }

            if (Vector3.Distance(_spiderEnemy.TargetPosition, _spiderEnemy.Position) < _chaseStartDistance)
            {
                _nextState = States.Chasing;
                return true;
            }

            return false;
        }
    }
}
