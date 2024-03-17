using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [CreateAssetMenu(fileName = "Tween_TYPE__NAME", 
        menuName = ScriptableObjectsHelper.TWEENEXTENSIONS_ASSETS_PATH + "TweenConfig")]
    public class TweenConfigAsset : ScriptableObject
    {
        [SerializeField] private TweenConfig _config;
        
        public static implicit operator TweenConfig(TweenConfigAsset asset)
        {
            return asset._config;
        }
    }
}