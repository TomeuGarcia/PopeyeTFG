using System.Collections;
using System.Collections.Generic;
using Popeye.Modules.PlayerController.Scripts.Enemies;
using UnityEngine;

namespace Popeye.Modules.Enemies.StateMachine
{
    public class SpiderSpiderEnemyStateMachine : ISpiderEnemyStateMachine
    {
        private SpiderEnemy _spiderEnemy;

        [Header("DISTANCES")] [SerializeField, Range(0.0f, 20.0f)]
        private float _chaseStartDistance = 8.0f;

        [SerializeField, Range(0.0f, 20.0f)] private float _loseInterestDistance = 7.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _attackStartDistance = 2.0f;
        [SerializeField, Range(0.0f, 20.0f)] private float _dashDistance = 4.0f;

        [Header("DURATIONS")] [SerializeField, Range(0.0f, 10.0f)]
        private float _minIdleDuration = 0.5f;

        [SerializeField, Range(0.0f, 10.0f)] private float _dashPrepareDuration = 1.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _dashExecutionDuration = 0.5f;
        [SerializeField, Range(0.0f, 10.0f)] private float _dashRecoverDuration = 2.0f;

        private Dictionary<ISpiderEnemyState.States, ISpiderEnemyState> _states;
        private ISpiderEnemyState _currentState;



        public override void AwakeInit(SpiderEnemy spiderEnemy)
        {
            _spiderEnemy = spiderEnemy;
            Init();
        }

        private void OnDestroy()
        {
            _currentState.Exit();
        }

        private void Init()
        {
            SpiderSpiderEnemyIdleState idleState = new SpiderSpiderEnemyIdleState(_spiderEnemy, _chaseStartDistance, _minIdleDuration);
            SpiderSpiderEnemyChasingState chasingState = new SpiderSpiderEnemyChasingState(_spiderEnemy, _loseInterestDistance, _attackStartDistance);
            SpiderSpiderEnemyDashingState dashingState = new SpiderSpiderEnemyDashingState(_spiderEnemy, _dashPrepareDuration, _dashExecutionDuration,
                _dashRecoverDuration, _dashDistance,
                _spiderEnemy.MaxMoveSpeed);
            SpiderSpiderEnemyDeadState deadState = new SpiderSpiderEnemyDeadState(_spiderEnemy);
            SpiderSpiderEnemyStunnedState stunnedState = new SpiderSpiderEnemyStunnedState(_spiderEnemy);


            _states = new Dictionary<ISpiderEnemyState.States, ISpiderEnemyState>()
            {
                { ISpiderEnemyState.States.Idle, idleState },
                { ISpiderEnemyState.States.Dead, deadState },
                { ISpiderEnemyState.States.Chasing, chasingState },
                { ISpiderEnemyState.States.Dashing, dashingState },
                { ISpiderEnemyState.States.Stunned, stunnedState }
            };

            _currentState = _states[ISpiderEnemyState.States.Idle];
            _currentState.Enter();
        }


        private void Update()
        {
            if (_currentState.Update(Time.deltaTime))
            {
                _currentState.Exit();
                _currentState = _states[_currentState.NextState];
                _currentState.Enter();
            }
        }


        public override void ResetStateMachine()
        {
            OverwriteCurrentState(ISpiderEnemyState.States.Idle);
        }

        public override void OverwriteCurrentState(ISpiderEnemyState.States newState)
        {
            _currentState.Interrupt();
            _currentState.Exit();
            _currentState = _states[newState];
            _currentState.Enter();
        }

    }
}
