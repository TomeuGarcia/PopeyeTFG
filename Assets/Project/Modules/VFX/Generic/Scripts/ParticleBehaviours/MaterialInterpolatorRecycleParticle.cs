using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Pool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.VFX.Generic.ParticleBehaviours
{
    public class MaterialInterpolatorRecycleParticle : RecyclableObject
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        private Material _material;

        [System.Serializable]
        private class FloatSetupData
        {
            public string id;
            public float initialValue;
        }
        
        [System.Serializable]
        private class FloatInterpolationData
        {
            public float delay;
            
            public string id;
            public float duration;
            public float endValue = 1.0f;
            public Ease ease;
            
            public bool waitForCompletion;
        }

        [SerializeField] private List<FloatSetupData> _floatSetupDatas = new();
        [SerializeField] private List<FloatInterpolationData> _floatInterpolationDatas = new();

        private void Awake()
        {
            _material = _meshRenderer.material;
        }

        internal override void Init()
        {
            Setup();
            FloatInterpolations();
        }

        private void Setup()
        {
            foreach (var data in _floatSetupDatas)
            {
                _material.SetFloat(data.id, data.initialValue);
            }
        }
        
        private async UniTaskVoid FloatInterpolations()
        {
            int i = 0;
            foreach (var data in _floatInterpolationDatas)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(data.delay));
                _material.DOFloat(data.endValue, data.id, data.duration).SetEase(data.ease);
                i++;
                
                if (data.waitForCompletion || i == (_floatInterpolationDatas.Count))
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(data.duration));
                }
            }
            
            Recycle();
        }

        internal override void Release() { }
    }
}