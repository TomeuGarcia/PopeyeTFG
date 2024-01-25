using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargetToResultConverter : IAutoAimTargetToResultConverter
    {
        private Transform _targeter;
        private Vector3 _startForwardDirection;
        private Vector3 _startRightDirection;
        private Vector3 TargeterPosition => _targeter.position;


        public void Configure(Transform targeter, Vector3 startForwardDirection, Vector3 startRightDirection)
        {
            _targeter = targeter;
            _startForwardDirection = startForwardDirection;
            _startRightDirection = startRightDirection;
        }
        
        public AutoAimTargetResult[] Convert(IAutoAimTarget[] autoAimTargets)
        {
            AutoAimTargetResult[] autoAimTargetDatas = new AutoAimTargetResult[autoAimTargets.Length];

            for (int i = 0; i < autoAimTargetDatas.Length; ++i)
            {
                autoAimTargetDatas[i] = Convert(autoAimTargets[i]);
            }

            return autoAimTargetDatas;
        }

        public AutoAimTargetResult Convert(IAutoAimTarget autoAimTarget)
        {
            AutoAimTargetResult autoAimTargetResult  = new AutoAimTargetResult();
            autoAimTargetResult.Configure(
                autoAimTarget,
                ComputeAngularPositionFromPosition(autoAimTarget.Position)
            );

            return autoAimTargetResult;
        }
        
        
        private float ComputeAngularPositionFromPosition(Vector3 position)
        {
            Vector3 direction = (position - TargeterPosition).normalized;

            return ComputeAngularPositionFromDirection(direction);
        }

        private float ComputeAngularPositionFromDirection(Vector3 direction)
        {
            float angle = Mathf.Acos(Vector3.Dot(_startForwardDirection, direction)) * Mathf.Rad2Deg;

            return Vector3.Dot(_startRightDirection, direction) < 0 ? 
                360 - angle :
                angle;
        }
    }
}