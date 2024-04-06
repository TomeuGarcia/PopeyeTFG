using DG.Tweening;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [CreateAssetMenu(fileName = "TweenPunch_TYPE__NAME", 
        menuName = ScriptableObjectsHelper.TWEENEXTENSIONS_ASSETS_PATH + "TweenPunchConfig")]
    public class TweenPunchConfigAsset : ScriptableObject
    {
        [SerializeField] private TweenPunchConfig _config;
        
        
        public static implicit operator TweenPunchConfig(TweenPunchConfigAsset asset)
        {
            return asset._config;
        }

    }
}