using NaughtyAttributes;
using UnityEngine;

namespace Project.Scripts.TweenExtensions
{
    [System.Serializable]
    public class TweenEaseReference
    {
        [SerializeField] private bool _useAsset = false;

        [HideIf("_useAsset")] [AllowNesting]
        [SerializeField] private TweenEaseConfig _config;
        
        [ShowIf("_useAsset")] [AllowNesting]
        [Expandable] [SerializeField] private TweenEaseConfigAsset _configAsset;

        public TweenEaseConfig Value => _useAsset ? _configAsset.Config : _config;

        
        public static implicit operator TweenEaseConfig(TweenEaseReference reference)
        {
            return reference.Value;
        }
        
    }
}