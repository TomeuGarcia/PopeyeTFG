using System;
using Popeye.Modules.PlayerController.Inputs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    [ExecuteInEditMode]
    public class AutoAimWorld_Test : MonoBehaviour
    {
        [Header("TARGETER")]
        [SerializeField] private Transform _targeter;
        [SerializeField] private TrailRenderer _aimTrail;
        [SerializeField] private float _targeterMoveSpeed = 50f;
        [SerializeField] private float _targeterRotationSpeed = 2000f;
        
        public Transform Targeter => _targeter;
        public float TargeterLookAngle { get; private set; }
    
        
        [Header("TARGETS")]
        [SerializeField] private Transform _aimTargetsParent;
        public Transform AimTargetsParent => _aimTargetsParent;

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
            if (AimTargetsData == null || AimTargetsData.Length != _aimTargetsParent.childCount)
            {
                AimTargetsData = new AutoAimTargetData_Test[_aimTargetsParent.childCount];
                for (int i = 0; i < _aimTargetsParent.childCount; ++i)
                {
                    AimTargetsData[i] = _aimTargetsParent.GetChild(i).GetComponent<AutoAimTargetData_Test>();
                }
            }

            if (_movementInput != null)
            {
                Vector3 movementInput = _movementInput.GetMovementInput();
                _targeter.position += movementInput * (Time.deltaTime * _targeterMoveSpeed);

                Vector3 lookInput = _movementInput.GetLookInput();
                bool isLooking = lookInput.sqrMagnitude > 0.9f;

                if (isLooking)
                {
                    TargeterLookAngle = GetAngleFromDirection(isLooking ? lookInput : movementInput);
                }
                else if (movementInput.sqrMagnitude > 0.1f)
                {
                    SetTargeterLookToDirection(movementInput.normalized);
                }

                _aimTrail.emitting = isLooking;
                

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
        private void SetTargeterLookToDirection(Vector3 direction)
        {
            _targeter.rotation =
                Quaternion.RotateTowards(_targeter.rotation, Quaternion.LookRotation(direction, Vector3.up),
                    Time.deltaTime * _targeterRotationSpeed);
        }


        public void RandomizeTargetPositions()
        {
            foreach (var autoAimTargetDataTest in AimTargetsData)
            {
                int x = Random.Range(-4, 5);
                int y = 0;
                int z = Random.Range(-4, 5);

                autoAimTargetDataTest.transform.localPosition =
                    new Vector3(x, y, z);
            }
            

        }
    }
}