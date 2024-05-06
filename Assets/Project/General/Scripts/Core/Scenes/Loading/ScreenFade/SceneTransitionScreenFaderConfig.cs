using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    [CreateAssetMenu(fileName = "SceneTransitionScreenFaderConfig", 
        menuName = ScriptableObjectsHelper.SCENES_ASSETS_PATH + "TransitionScreenFaderConfig")]
    public class SceneTransitionScreenFaderConfig : ScriptableObject
    {
        [Header("FADES")]
        [SerializeField] private TweenFadeConfig _fadeIn = TweenFadeConfig.FadeIn();
        [SerializeField] private TweenFadeConfig _fadeOut = TweenFadeConfig.FadeOut();
        
        [Header("TEXT FADES")]
        [SerializeField] private TweenFadeConfig _loadingTextFadeIn = TweenFadeConfig.FadeIn();
        [SerializeField] private TweenFadeConfig _loadingTextFadeOut = TweenFadeConfig.FadeOut();
        
        [Header("DURATIONS")]
        [SerializeField, Range(0.0f, 10.0f)] private float _minimumTimeFadedIn = 1.0f;
        [SerializeField, Range(0.0f, 10.0f)] private float _extraTimeFadedIn = 0.5f;
        
        public TweenFadeConfig FadeIn => _fadeIn;
        public TweenFadeConfig FadeOut => _fadeOut;
        public TweenFadeConfig LoadingTextFadeIn => _loadingTextFadeIn;
        public TweenFadeConfig LoadingTextFadeOut => _loadingTextFadeOut;
        public float MinimumTimeFadedIn => _minimumTimeFadedIn;
        public float ExtraTimeFadedIn => _extraTimeFadedIn;
    }
}