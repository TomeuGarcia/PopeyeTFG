using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargetToResultConverter : IAutoAimTargetToResultConverter
    {
        private Transform _targeter;

        private Vector3 TargeterPosition => _targeter.position;


        public void Configure(Transform targeter)
        {
            _targeter = targeter;
        }
        
        public AutoAimTargetResult[] Convert(IAutoAimTarget[] autoAimTargets, Vector3 forwardDirection, Vector3 rightDirection)
        {
            AutoAimTargetResult[] autoAimTargetDatas = new AutoAimTargetResult[autoAimTargets.Length];

            for (int i = 0; i < autoAimTargetDatas.Length; ++i)
            {
                autoAimTargetDatas[i] = Convert(autoAimTargets[i], forwardDirection, rightDirection);
            }

            return autoAimTargetDatas;
        }

        public AutoAimTargetResult Convert(IAutoAimTarget autoAimTarget, Vector3 forwardDirection, Vector3 rightDirection)
        {
            AutoAimTargetResult autoAimTargetResult  = new AutoAimTargetResult();
            autoAimTargetResult.Configure(
                autoAimTarget,
                ComputeAngularPositionFromPosition(autoAimTarget.Position, forwardDirection, rightDirection)
            );

            return autoAimTargetResult;
        }
        
        
        private float ComputeAngularPositionFromPosition(Vector3 position, Vector3 forwardDirection, Vector3 rightDirection)
        {
            Vector3 direction = (position - TargeterPosition).normalized;

            return ComputeAngularPositionFromDirection(direction, forwardDirection, rightDirection);
        }

        private float ComputeAngularPositionFromDirection(Vector3 direction, Vector3 forwardDirection, Vector3 rightDirection)
        {
            float angle = Mathf.Acos(Vector3.Dot(forwardDirection, direction)) * Mathf.Rad2Deg;

            return Vector3.Dot(rightDirection, direction) < 0 ? 
                360 - angle :
                angle;
        }
    }
}