using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.InverseKinematics.Bones
{

    public class BoneChain : MonoBehaviour
    {
        [SerializeField] private Bone _bonePrefab;
        [SerializeField] private Bone _boneEndEffectorPrefab;
        [SerializeField, Range(1, 50)] private int _numberOfBones = 10;
        private int _oldNumberOfBones;


        [SerializeField] private Transform _firstBoneParent;
        [SerializeField] public bool autoUpdateBoneGeneration = true;
        public Bone[] Bones { get; private set; }
        public int NumberOfBones => Bones.Length;

        public delegate void BoneArmEvent();

        public BoneArmEvent OnGenerationUpdate;


        public void AwakeConfigure(int numberOfBones)
        {
            _numberOfBones = numberOfBones;
        }
        

        public void StartInit()
        {
            _oldNumberOfBones = -1;
            Bones = new Bone[0];

            UpdateBoneGeneration();
        }

        private void Update()
        {
            if (autoUpdateBoneGeneration)
            {
                UpdateBoneGeneration();
            }
        }


        public void UpdateBoneGeneration()
        {
            if (_bonePrefab == null || _numberOfBones < 1 || _firstBoneParent == null) return;

            if (_numberOfBones == _oldNumberOfBones) return;

            DestroyBones();
            SpawnBones();

            _oldNumberOfBones = _numberOfBones;

            OnGenerationUpdate?.Invoke();
        }


        private void SpawnBones()
        {
            Bones = new Bone[_numberOfBones];

            Bones[0] = Instantiate(_bonePrefab, _firstBoneParent);

            for (int i = 1; i < _numberOfBones - 1; ++i)
            {
                Bones[i] = Instantiate(_bonePrefab, Bones[i - 1].BoneEnd);
            }

            if (_numberOfBones > 1)
            {
                Bones[_numberOfBones - 1] = Instantiate(_boneEndEffectorPrefab, Bones[_numberOfBones - 2].BoneEnd);
            }
        }

        private void DestroyBones()
        {
            if (Bones.Length < 1) return;

            Destroy(Bones[0].gameObject);
        }


        public void FromPositions(Vector3[] positions)
        {
            int numberOfBonesMinusOne = _numberOfBones - 1;
            for (int i = 0; i < numberOfBonesMinusOne; ++i)
            {
                Vector3 oldDirection = (Bones[i + 1].Position - Bones[i].Position).normalized;
                Vector3 newDirection = (positions[i + 1] - positions[i]).normalized;
                
                Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
                float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;
                
                if (angle > 1.0f)
                {
                    Bones[i].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * Bones[i].Rotation);
                }
            }
        }
    }
}