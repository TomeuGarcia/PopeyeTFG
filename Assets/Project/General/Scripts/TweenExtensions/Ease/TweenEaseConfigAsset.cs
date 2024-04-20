using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [CreateAssetMenu(fileName = "TweenEase__NAME", 
        menuName = ScriptableObjectsHelper.TWEENEXTENSIONS_ASSETS_PATH + "TweenEaseConfig")]
    public class TweenEaseConfigAsset : ScriptableObject
    {
        [SerializeField] private TweenEaseConfig _config;
        
        public TweenEaseConfig Config => _config;
    }
}