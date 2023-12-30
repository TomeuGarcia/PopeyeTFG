using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Pool;
using Popeye.Modules.VFX.Generic.MaterialInterpolationConfiguration;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.VFX.Generic.ParticleBehaviours
{
    public class MaterialInterpolatorRecycleParticle : RecyclableObject
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        private Material _material;

        [SerializeField] private MaterialFloatSetupConfig[] _floatSetupDatas;
        [SerializeField] private MaterialFloatInterpolationConfig[] _floatInterpolationDatas;

        private void Awake()
        {
            _material = _meshRenderer.material;
        }

        internal override void Init()
        {
            ApplyInterpolations().Forget();
        }

        private async UniTaskVoid ApplyInterpolations()
        {
            MaterialInterpolator.Setup(_material, _floatSetupDatas);
            await MaterialInterpolator.ApplyInterpolations(_material, _floatInterpolationDatas);
            Recycle();
        }

        internal override void Release() { }
    }
}