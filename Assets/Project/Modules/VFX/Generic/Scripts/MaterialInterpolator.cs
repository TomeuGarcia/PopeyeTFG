using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.VFX.Generic.MaterialInterpolationConfiguration;
using Unity.VisualScripting;
using UnityEngine;

namespace Popeye.Modules.VFX.Generic
{
    public static class MaterialInterpolator
    {
        public static void Setup(Material material, MaterialFloatSetupConfig[] setupDatas)
        {
            foreach (var data in setupDatas)
            {
                material.SetFloat(data.ID, data.InitialValue);
            }
        }
        
        public static async UniTask ApplyInterpolations(Material material, MaterialFloatInterpolationConfig[] interpolationDatas)
        {
            int i = 0;
            foreach (var data in interpolationDatas)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(data.Delay));
                material.DOFloat(data.EndValue, data.ID, data.Duration).SetEase(data.Ease);
                i++;
                
                if (data.WaitForCompletion || i == (interpolationDatas.Length))
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(data.Duration));
                }
            }
        }
    }
}