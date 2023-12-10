using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

namespace Popeye.Modules.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class OrbitingCamera : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private Transform _focus;
        private Vector3 _focusPoint;
        [HideInInspector] public Vector3 focusPointOffset = Vector3.zero;

        [SerializeField, Range(1.0f, 300.0f)] public float _distance;
        [SerializeField, Range(0.0f, 10.0f)] private float _noFocusRadius;

        [SerializeField, Range(0.0f, 1.0f)] private float _stillFocusRecentering = 0.5f;

        [SerializeField] private LayerMask _obstructionLayers;


        private Quaternion _lookRotation;
        private Vector3 _lookDirection;
        private Vector3 _lookPosition;

        public Transform CameraTransform => transform;

        //Testing
        private bool followPlayer = true;


        private Vector3 CameraHalfExtends
        {
            get
            {
                Vector3 halfExtends;
                halfExtends.y = _camera.nearClipPlane * Mathf.Tan(0.5f * Mathf.Deg2Rad * _camera.fieldOfView);
                halfExtends.x = halfExtends.y * _camera.aspect;
                halfExtends.z = 0.0f;

                return halfExtends;
            }
        }



        private void Awake()
        {
            _focusPoint = _focus.position;
        }

        private void OnValidate()
        {
            if (_camera != null && _focus != null)
            {
                _focusPoint = _focus.position;
                UpdateFocusPoint();
                UpdateState();
                UpdateTransform();
            }
        }


        private void LateUpdate()
        {
            UpdateFocusPoint();
            UpdateState();

            CheckCameraObstruction();

            //Testing
            if (Input.GetKeyDown(KeyCode.C))
            {
                followPlayer = !followPlayer;
            }

            if (followPlayer)
            {
                //Original
                UpdateTransform();
            }
        }


        private void UpdateFocusPoint()
        {
            Vector3 focusPosition = _focus.position + focusPointOffset;
            if (_noFocusRadius > 0.0f)
            {
                float distanceBetweenFocusPoint = Vector3.Distance(focusPosition, _focusPoint + focusPointOffset);
                float t = 1.0f;

                if (distanceBetweenFocusPoint > 0.01f && _stillFocusRecentering > 0.0f)
                {
                    t = Mathf.Pow(1.0f - _stillFocusRecentering, Time.unscaledDeltaTime);
                }

                if (distanceBetweenFocusPoint > _noFocusRadius)
                {
                    t = Mathf.Min(t, _noFocusRadius / distanceBetweenFocusPoint);
                }

                _focusPoint = Vector3.Lerp(focusPosition, _focusPoint + focusPointOffset, t);
            }
            else
            {
                _focusPoint = focusPosition;
            }

        }

        private void UpdateState()
        {
            _lookRotation = CameraTransform.localRotation;

            _lookDirection = CameraTransform.forward;
            //_lookDirection = lookRotation * CameraTransform.forward;
            _lookPosition = _focusPoint - _lookDirection * _distance;
        }

        private void UpdateTransform()
        {
            CameraTransform.SetPositionAndRotation(_lookPosition, _lookRotation);
        }

        private void CheckCameraObstruction()
        {
            Vector3 rectOffset = _lookDirection * _camera.nearClipPlane;
            Vector3 rectPosition = _lookPosition + rectOffset;
            Vector3 castFrom = _focus.position;
            Vector3 castLine = rectPosition - castFrom;
            float castDistance = castLine.magnitude;
            Vector3 castDirection = castLine / castDistance;

            if (Physics.BoxCast(castFrom, CameraHalfExtends, castDirection, out RaycastHit hit,
                    _lookRotation, castDistance, _obstructionLayers))
            {
                rectPosition = castFrom + castDirection * hit.distance;
                _lookPosition = rectPosition - rectOffset;
            }
        }

    }
}


