using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System.Threading;

namespace Popeye.Modules.Enemies.StateMachine
{
    public class SpiderSpiderEnemyDashingState : ISpiderEnemyState
    {
        private SpiderEnemy _spiderEnemy;
        private float _dashPrepareDuration;
        private float _dashExecutionDuration;
        private float _dashRecoverDuration;
        private float _dashDistance;

        private float _defaultMaxMoveSpeed;

        Vector3 _dashStartPosition;
        Vector3 _dashEndPosition;

        bool _finishedDashing;

        private CancellationToken _dashCancellationToken;

        public SpiderSpiderEnemyDashingState(SpiderEnemy spiderEnemy, float dashPrepareDuration, float dashExecutionDuration,
            float dashRecoverDuration,
            float dashDistance, float defaultMaxMoveSpeed)
        {
            _spiderEnemy = spiderEnemy;
            _dashPrepareDuration = dashPrepareDuration;
            _dashExecutionDuration = dashExecutionDuration;
            _dashRecoverDuration = dashRecoverDuration;
            _dashDistance = dashDistance;
            _defaultMaxMoveSpeed = defaultMaxMoveSpeed;
        }


        protected override void DoEnter()
        {
            _finishedDashing = false;
            _spiderEnemy.SetMaxMoveSpeed(0.0f);

            StartDashSequence();

            _dashCancellationToken = new CancellationToken();
        }

        public override void Exit()
        {
            _spiderEnemy.SetMaxMoveSpeed(_defaultMaxMoveSpeed);
            _spiderEnemy.SetCanRotate(true);

            _spiderEnemy.DisableDealingContactDamage();
        }

        public override void Interrupt()
        {
            if (!_finishedDashing)
            {
                _spiderEnemy.transform.DOKill();
                _dashCancellationToken.ThrowIfCancellationRequested();
                _finishedDashing = true;
            }
        }

        public override bool Update(float deltaTime)
        {
            if (_finishedDashing)
            {
                _nextState = States.Chasing;
                return true;
            }

            return false;
        }


        private void ComputeDashStartPosition()
        {
            _dashStartPosition = _spiderEnemy.Position - (_spiderEnemy.LookDirection * _dashDistance / 4);
            _dashStartPosition =
                PositioningHelper.Instance.GetGoalPositionCheckingObstacles(_spiderEnemy.Position, _dashStartPosition,
                    out float distanceRatio);
        }

        private void ComputeDashEndPosition()
        {
            _dashEndPosition = _spiderEnemy.Position + (_spiderEnemy.LookDirection * _dashDistance);
            _dashEndPosition =
                PositioningHelper.Instance.GetGoalPositionCheckingObstacles(_spiderEnemy.Position, _dashEndPosition,
                    out float distanceRatio);
        }

        private async void StartDashSequence()
        {
            ComputeDashStartPosition();
            ComputeDashEndPosition();

            _spiderEnemy.SetCanRotate(false);
            _spiderEnemy.transform.DOMove(_dashStartPosition, _dashPrepareDuration)
                .SetEase(Ease.InOutQuart);
            await Task.Delay((int)(_dashPrepareDuration * 1000), _dashCancellationToken);

            if (_finishedDashing) return;
            _spiderEnemy.EnableDealingContactDamage();

            _spiderEnemy.transform.DOMove(_dashEndPosition, _dashExecutionDuration)
                .SetEase(Ease.OutQuart);
            await Task.Delay((int)(_dashExecutionDuration * 1000), _dashCancellationToken);

            if (_finishedDashing) return;

            await Task.Delay((int)(_dashRecoverDuration * 1000), _dashCancellationToken);


            _finishedDashing = true;
        }

    }
}
