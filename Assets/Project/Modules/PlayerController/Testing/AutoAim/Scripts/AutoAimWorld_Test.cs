using System;
using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    public class AutoAimWorld_Test : MonoBehaviour
    {
        [Header("TARGETER")]
        [SerializeField] private Transform _targeter;

        [SerializeField, Range(0f, 360f)] private float _targeterLookAngle = 0f;
        public float TargeterLookAngle => _targeterLookAngle;
        
        
        [Header("TARGETS")]
        [SerializeField] private Transform _aimTargetsParent;

        private Vector3 startLookDirection = Vector3.forward;
        private Vector3 startRightDirection = Vector3.right;
        
        public AutoAimTargetData_Test[] AimTargetsData { get; private set; }


        private void OnValidate()
        {
            if (_targeterLookAngle > 359.9f)
            {
                _targeterLookAngle = 0f;
            }
        }

        public void DoUpdate()
        {
            AimTargetsData = new AutoAimTargetData_Test[_aimTargetsParent.childCount];
            for (int i = 0; i < _aimTargetsParent.childCount; ++i)
            {
                AutoAimTargetData_Test aimTarget = _aimTargetsParent.GetChild(i).GetComponent<AutoAimTargetData_Test>();
                AimTargetsData[i] = aimTarget;
                AimTargetsData[i].SetAngleAtCenter(GetAngleFromTargeterPosition(aimTarget.Position));
            }
            
            // Sort by angle
            Array.Sort(AimTargetsData, (x, y) => x.AngularPosition < y.AngularPosition ? 0 : 1);
            
        }

        private float GetAngleFromTargeterPosition(Vector3 position)
        {
            Vector3 direction = (position - _targeter.position).normalized;

            float angle = Mathf.Acos(Vector3.Dot(startLookDirection, direction)) * Mathf.Rad2Deg;

            return Vector3.Dot(startRightDirection, direction) < 0 ? 
                360 - angle :
                angle;
        }
    }
}