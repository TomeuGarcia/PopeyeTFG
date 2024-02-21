using System;
using DG.Tweening;
using UnityEngine;

namespace Popeye.InverseKinematics.Bones
{

    public class Bone : MonoBehaviour
    {
        [SerializeField] private Transform _boneRoot;
        [SerializeField] private Transform _boneEnd;
        [SerializeField] private GameObject _meshHolder;
        [SerializeField] private MeshRenderer[] _meshes;
        [SerializeField, Range(0f, 2.0f)] private float _boneLength = 0.5f;
        [SerializeField] private bool _isEndEffector = false;
        
        public Transform BoneRoot => _boneRoot;
        public Transform BoneEnd => _boneEnd;
        public Vector3 Up => BoneRoot.up;
        public Vector3 Forward => BoneRoot.forward;
        public Vector3 Position => BoneRoot.position;
        public Quaternion Rotation => BoneRoot.rotation;

        public void SetBoneLength(float boneLength)
        {
            _boneLength = boneLength;
            OnValidate();
        }
        public void SetMaterial(Material material)
        {
            foreach (MeshRenderer mesh in _meshes)
            {
                mesh.sharedMaterial = material;
            }
        }

        private void Awake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if (_boneEnd && _boneEnd && _meshHolder)
            {
                if (_isEndEffector)
                {
                    _boneEnd.localPosition = Vector3.zero;
                    _meshHolder.transform.localScale = Vector3.one;
                }
                else
                {
                    _boneEnd.position = _boneRoot.position + (Forward * _boneLength);
                    _meshHolder.transform.localScale = Vector3.one * _boneLength;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 boneStart = _boneRoot.position;
            Vector3 boneEnd = _boneEnd.position;
            Vector3 endToStartScaledDirection = (boneStart - boneEnd).normalized * 0.2f;  

            Gizmos.color = Color.red;
            Gizmos.DrawLine(boneStart, boneEnd);
            Gizmos.DrawLine(boneEnd, boneEnd + Quaternion.AngleAxis(30.0f, Up) * endToStartScaledDirection);
            Gizmos.DrawLine(boneEnd, boneEnd + Quaternion.AngleAxis(-30.0f, Up) * endToStartScaledDirection);
        }


        public void Show()
        {
            _meshHolder.SetActive(true);
        }
        public void Hide()
        {
            _meshHolder.SetActive(false);
        }


        public void SetWorldRotation(Quaternion worldRotation)
        {
            _boneRoot.rotation = worldRotation;
        }
        public void SetLocalRotation(Quaternion localRotation)
        {
            _boneRoot.localRotation = localRotation;
        }


        public void SetWorldPosition(Vector3 worldPosition)
        {
            _boneRoot.position = worldPosition;
        }
        public void SetLocalPosition(Vector3 localPosition)
        {
            _boneRoot.localPosition = localPosition;
        }

        public void SetLocalRotation(Quaternion localRotation, float duration, Ease ease = Ease.InOutSine)
        {
            _boneRoot.DOLocalRotateQuaternion(localRotation, duration).SetEase(ease);
        }

        public void SetWorldRotation(Quaternion worldRotation, float duration, Ease ease = Ease.InOutSine)
        {
            _boneRoot.DORotateQuaternion(worldRotation, duration).SetEase(ease);
        }

        public void SetLocalPosition(Vector3 localPosition, float duration, Ease ease = Ease.InOutSine)
        {
            _boneRoot.DOLocalMove(localPosition, duration).SetEase(ease);
        }

        public void LookAt(Transform target)
        {
            Quaternion rotation = GetRotationToTarget(target.position);

            SetWorldRotation(rotation);
        }

        public void LookAt(Transform target, float duration, Ease ease = Ease.InOutSine)
        {
            Quaternion rotation = GetRotationToTarget(target.position);

            SetWorldRotation(rotation, duration, ease);
        }

        private Quaternion GetRotationToTarget(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - _boneRoot.position).normalized;

            float dot = Vector3.Dot(direction, _boneRoot.forward);

            if (dot > 0.99f)
            {
                return _boneRoot.rotation;
            }

            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            Vector3 axis = Vector3.Cross(_boneRoot.forward, direction).normalized;

            Quaternion offsetRotation = Quaternion.AngleAxis(angle, axis);

            return offsetRotation * _boneRoot.rotation;
        }
        
    }
    
}