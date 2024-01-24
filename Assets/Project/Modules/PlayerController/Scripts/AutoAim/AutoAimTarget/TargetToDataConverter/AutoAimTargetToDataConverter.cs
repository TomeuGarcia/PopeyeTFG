using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargetToDataConverter : IAutoAimTargetToDataConverter
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
        
        public AutoAimTargetData[] Convert(IAutoAimTarget[] autoAimTargets)
        {
            AutoAimTargetData[] autoAimTargetDatas = new AutoAimTargetData[autoAimTargets.Length];

            for (int i = 0; i < autoAimTargetDatas.Length; ++i)
            {
                IAutoAimTarget autoAimTarget = autoAimTargets[i];
                
                AutoAimTargetData autoAimTargetData  = new AutoAimTargetData();
                autoAimTargetData.Configure(
                    autoAimTarget.DataConfig,
                    autoAimTarget.Position,
                    ComputeAngularPositionFromPosition(autoAimTarget.Position)
                    );

                autoAimTargetDatas[i] = autoAimTargetData;
            }

            return autoAimTargetDatas;
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