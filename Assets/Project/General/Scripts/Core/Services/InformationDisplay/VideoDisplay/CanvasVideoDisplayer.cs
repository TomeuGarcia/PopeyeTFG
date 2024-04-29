using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.TweenExtensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Popeye.Core.Services.InformationDisplay
{
    public class CanvasVideoDisplayer : MonoBehaviour, IVideoDisplayer, IDisplayQueueDelegate
    {
        [Header("COMPONENTS")] 
        [SerializeField] private CanvasGroup _backgroundFadeGroup;
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private RawImage _videoImage;
        [SerializeField] private RenderTexture _videoRenderTexture;
        

        private DisplayQueue<VideoDisplayConfig> _displayQueue;
        
        private void Awake()
        {
            _backgroundFadeGroup.alpha = 0;

            _videoImage.texture = _videoRenderTexture;
            _videoImage.rectTransform.sizeDelta = new Vector2(_videoRenderTexture.width, _videoRenderTexture.height);
            
            _videoPlayer.targetTexture = _videoRenderTexture;
            _videoPlayer.Stop();

            _displayQueue = new DisplayQueue<VideoDisplayConfig>(this);
        }


        public void StartShowing(VideoDisplayConfig videoDisplayConfig)
        {
            _displayQueue.StartShowing(videoDisplayConfig);
        }

        public void StopShowing(VideoDisplayConfig videoDisplayConfig)
        {
            _displayQueue.StopShowing(videoDisplayConfig);
        }

        
        public async UniTask DoStartShowing()
        {
            VideoDisplayConfig currentDisplay = _displayQueue.CurrentDisplay;
            SetVideoContents(currentDisplay);
            
            await _backgroundFadeGroup.Fade(currentDisplay.BackgroundViewExtras.ShowFade)
                .AsyncWaitForCompletion();
            
            _videoPlayer.Play();
        }

        public async UniTask DoStopShowing()
        {
            VideoDisplayConfig currentDisplay = _displayQueue.CurrentDisplay;
            
            await _backgroundFadeGroup.Fade(currentDisplay.BackgroundViewExtras.HideFade)
                .AsyncWaitForCompletion();
            
            _videoPlayer.Stop();
        }
        
        
        private void SetVideoContents(VideoDisplayConfig displayConfig)
        {
            _videoPlayer.clip = displayConfig.VideoClip;
            _videoPlayer.time = 0;
        }
        
    }
}