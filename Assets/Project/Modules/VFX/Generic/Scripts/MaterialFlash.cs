using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.VFX.Generic
{
    [System.Serializable]
    public class MaterialFlash
    {
        public Material _flashMaterial;
        public float _waitTime;
        
        /*
         TODO: Adapt funcition here
        private async UniTaskVoid FlashHitEffect(List<MaterialFlash> materialFlashes, )
        {
            foreach (var flash in materialFlashes)
            {
                foreach (var material in materials)
                {
                    data._mesh.material = flash._flashMaterial;
                }

                await UniTask.Delay(TimeSpan.FromSeconds(flash._waitTime));
            }

            foreach (var data in _originalMeshDatas)
            {
                for (int i = 0; i < data._mesh.materials.Length; i++)
                {
                    data._mesh.material = data._originalMaterial;
                }
            }
        }
        */
    }
}