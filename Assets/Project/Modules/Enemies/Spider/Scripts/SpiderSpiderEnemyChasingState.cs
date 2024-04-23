using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.Enemies.StateMachine
{
    public class SpiderSpiderEnemyChasingState : ISpiderEnemyState
    {
        private SpiderEnemy _spiderEnemy;
        private float _loseInterestDistance;
        private float _attackStartDistance;

        private float _timeLastDash;
        private float _dashCooldownTime;


        public SpiderSpiderEnemyChasingState(SpiderEnemy spiderEnemy, float loseInterestDistance, float attackStartDistance)
        {
            _spiderEnemy = spiderEnemy;
            _loseInterestDistance = loseInterestDistance;
            _attackStartDistance = attackStartDistance;
            _timeLastDash = 0.0f;
            _dashCooldownTime = 2.0f;
        }


        protected override void DoEnter()
        {

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
            float distanceFromTarget = Vector3.Distance(_spiderEnemy.TargetPosition, _spiderEnemy.Position);

            if (distanceFromTarget < _attackStartDistance && _spiderEnemy.IsTargetOnReachableHeight())
            {
                if (DashCooldownFinished())
                {
                    _timeLastDash = Time.time;
                    _nextState = States.Dashing;
                    return true;
                }
                else
                {
                    _spiderEnemy.SetMaxMoveSpeed(1.0f);
                }
            }

            if (distanceFromTarget > _loseInterestDistance)
            {
                _nextState = States.Idle;
                return true;
            }


            return false;
        }

        private bool DashCooldownFinished()
        {
            return Time.time > _timeLastDash + _dashCooldownTime;
        }
    }
}
