using System;
using Popeye.Modules.PlayerController.Inputs;
using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    [ExecuteInEditMode]
    public class AutoAimWorld_Test : MonoBehaviour
    {
        [Header("TARGETER")]
        [SerializeField] private Transform _targeter;
        [SerializeField] private float _targeterMoveSpeed = 50f;
        [SerializeField] private float _targeterRotationSpeed = 2000f;

        public float TargeterLookAngle { get; private set; }
    
        
        [Header("TARGETS")]
        [SerializeField] private Transform _aimTargetsParent;

        private Vector3 startLookDirection = Vector3.forward;
        private Vector3 startRightDirection = Vector3.right;
        
        public AutoAimTargetData_Test[] AimTargetsData { get; private set; }

        private WorldAxisMovementInput _movementInput;

        private void Awake()
        {
            _movementInput = new WorldAxisMovementInput();
        }

        public bool DoUpdate()
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


            if (_movementInput != null)
            {
                Vector3 movementInput = _movementInput.GetMovementInput();
                _targeter.position += movementInput * (Time.deltaTime * _targeterMoveSpeed);

                Vector3 lookInput = _movementInput.GetLookInput();
                bool isLooking = lookInput.sqrMagnitude > 0.1f;
                TargeterLookAngle = GetAngleFromDirection(isLooking ? lookInput : movementInput);

                return isLooking;
            }

            return false;
        }

        private float GetAngleFromTargeterPosition(Vector3 position)
        {
            Vector3 direction = (position - _targeter.position).normalized;

            return GetAngleFromDirection(direction);
        }

        private float GetAngleFromDirection(Vector3 direction)
        {
            float angle = Mathf.Acos(Vector3.Dot(startLookDirection, direction)) * Mathf.Rad2Deg;

            return Vector3.Dot(startRightDirection, direction) < 0 ? 
                360 - angle :
                angle;
        }

        public void SetTargeterLookDirection(float angle)
        {
            _targeter.rotation =
                Quaternion.RotateTowards(_targeter.rotation, Quaternion.AngleAxis(angle, Vector3.up),
                    Time.deltaTime * _targeterRotationSpeed);

        }
    }
}