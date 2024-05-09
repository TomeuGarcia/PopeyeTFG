using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Project.Modules.GameMenus.GameCredits
{
    [CreateAssetMenu(fileName = "CreditsDisplayConfig", 
        menuName = ScriptableObjectsHelper.GAMEMENU_ASSETS_PATH + "CreditsDisplayConfig")]
    public class CreditsDisplayConfig : ScriptableObject
    {
        [Header("REFERENCES")]
        [SerializeField] private CreditsContent _content;
        [SerializeField] private CreditsSettings _settings;
        [SerializeField] private CreditsText _textPrefab;
        [SerializeField] private CreditsImage _imagePrefab;

        [Header("SCROLLING")] 
        [SerializeField] private float _startScrollOffset = 500.0f;
        [SerializeField] private float _normalScrollSpeed = 50.0f;
        [SerializeField] private float _fastScrollSpeed = 200.0f;
        [SerializeField] private float _delayBeforeShowingInput = 1.0f;
        [SerializeField] private TweenFadeConfig _showInputFade = TweenFadeConfig.FadeIn();
        

        [Header("DURATIONS")] 
        [SerializeField] private float _stopScrollingDuration = 0.25f;
        [SerializeField] private AnimationCurve _stopScrollingEase = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _delayBeforeFinishing = 2.0f;
        
        [Header("AUDIO")] 
        [SerializeField] private FMODAudioManagerReference _audioManager;
        [SerializeField] private OneShotFMODSound _finishSound;
        
        
        public CreditsContent Content => _content;        
        public CreditsSettings Settings => _settings;
        public CreditsText TextPrefab => _textPrefab;
        public CreditsImage ImagePrefab => _imagePrefab;
        
        
        public float StartScrollOffset => _startScrollOffset;
        public float NormalScrollSpeed => _normalScrollSpeed;
        public float FastScrollSpeed => _fastScrollSpeed;
        public float DelayBeforeShowingInput => _delayBeforeShowingInput;
        public TweenFadeConfig ShowInputFade => _showInputFade;
        
        public float StopScrollingDuration => _stopScrollingDuration;
        public AnimationCurve StopScrollingEase => _stopScrollingEase;
        
        public float DelayBeforeFinishing => _delayBeforeFinishing;


        public void PlayFinishSound()
        {
            _audioManager.PlayOneShot(_finishSound);
        }
    }
}