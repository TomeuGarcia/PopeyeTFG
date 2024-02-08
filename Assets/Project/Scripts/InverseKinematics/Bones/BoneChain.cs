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
        [SerializeField] private bool _startVisible = true;
        public float BoneLength { get; private set; } = -1f;


        [SerializeField] private Transform _firstBoneParent;
        [SerializeField] public bool autoUpdateBoneGeneration = true;
        public Bone[] Bones { get; private set; }
        public int NumberOfBones => Bones.Length;

        public delegate void BoneArmEvent();

        public BoneArmEvent OnGenerationUpdate;


        public void AwakeConfigure(int numberOfBones, bool startVisible, float boneLength)
        {
            _numberOfBones = numberOfBones;
            _startVisible = startVisible;

            BoneLength = boneLength;
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

        public void Show()
        {
            foreach (Bone bone in Bones)
            {
                bone.Show();
            }
        }
        public void Hide()
        {
            foreach (Bone bone in Bones)
            {
                bone.Hide();
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

            if (BoneLength > 0)
            {
                for (int i = 0; i < _numberOfBones - 1; ++i)
                {
                    Bones[i].SetBoneLength(BoneLength);
                }
            }

            if (_startVisible)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void DestroyBones()
        {
            if (Bones.Length < 1) return;

            Destroy(Bones[0].gameObject);
        }


        public void FromPositions(Vector3[] positions)
        {
            int numberOfBonesMinusOne = NumberOfBones - 1;
            for (int i = 0; i < numberOfBonesMinusOne; i++)
            {
                Vector3 oldDirection = (Bones[i + 1].Position - Bones[i].Position).normalized;
                Vector3 newDirection = (positions[i + 1] - positions[i]).normalized;

                UpdateBonePosition(i, oldDirection, newDirection);
            }
        }
        
        private void UpdateBonePosition(int boneIndex, Vector3 oldDirection, Vector3 newDirection)
        {
            Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
            float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;

            if (angle > 1.0f)
            {
                Bones[boneIndex].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * Bones[boneIndex].Rotation);
            }
        }
        
        
        public void FromPositions2(Vector3[] positions)
        {
            int i = 0;
            for (; i < 1; ++i)
            {
                Vector3 oldDirection = (Bones[i + 1].Position - Bones[i].Position).normalized;
                Vector3 newDirection = positions[i + 1] - positions[i];

                UpdateBonePosition(i, oldDirection, newDirection);
            }


            int numberOfBonesMinusTwo = _numberOfBones - 2;
            for (; i < numberOfBonesMinusTwo; ++i)
            {
                Vector3 oldDirection = (Bones[i + 1].Position - Bones[i].Position).normalized;
                
                Vector3 newToNext = positions[i + 1] - positions[i];

                bool nextIsInsideCurrent = newToNext.sqrMagnitude < 0.1f;
                if (nextIsInsideCurrent)
                {
                    UpdateBonePosition(i, oldDirection, Vector3.down);
                    UpdateBonePosition(++i, oldDirection, Vector3.up);
                    continue;
                }
                
                Vector3 newDirection = newToNext.normalized;
                UpdateBonePosition(i, oldDirection, newDirection);
            }
            
            
            for (; i < _numberOfBones - 1; ++i)
            {
                Vector3 oldDirection = (Bones[i + 1].Position - Bones[i].Position).normalized;
                Vector3 newDirection = positions[i + 1] - positions[i];

                UpdateBonePosition(i, oldDirection, newDirection);
            }
        }



        
    }
}