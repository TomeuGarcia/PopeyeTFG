using DG.Tweening;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [CreateAssetMenu(fileName = "TweenColor__NAME", 
        menuName = ScriptableObjectsHelper.TWEENEXTENSIONS_ASSETS_PATH + "TweenColorConfig")]
    public class TweenColorConfigAsset : ScriptableObject
    {
        [SerializeField] private TweenColorConfig _config;
        
        public static implicit operator TweenColorConfig(TweenColorConfigAsset asset)
        {
            return asset._config;
        }
    }
}