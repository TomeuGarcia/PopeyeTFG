using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Popeye.Core.Services.InformationDisplay
{
    public class CanvasVideoInstantDisplayer : MonoBehaviour, IVideoDisplayer, IDisplayQueueDelegate
    {
        [Header("COMPONENTS")] 
        [SerializeField] private CanvasGroup _backgroundFadeGroup;

        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private RawImage _videoImage;
        [SerializeField] private RenderTexture _videoRenderTexture;
        [SerializeField, Range(0.0f, 3.0f)] private float _videoScale = 1.0f;
        [SerializeField, Range(0.0f, 5.0f)] private float _videoDelay = 0.5f;

        private DisplayQueue<VideoDisplayConfig> _displayQueue;


        private void Awake()
        {
            _backgroundFadeGroup.alpha = 0;

            _videoImage.texture = _videoRenderTexture;
            _videoImage.rectTransform.sizeDelta = 
                new Vector2(_videoRenderTexture.width, _videoRenderTexture.height) * _videoScale;

            _videoPlayer.targetTexture = _videoRenderTexture;
            _videoPlayer.isLooping = true;
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
            _videoPlayer.Play();
            
            await UniTask.Delay(TimeSpan.FromSeconds(_videoDelay), ignoreTimeScale: true);   

            _backgroundFadeGroup.alpha = 1f;
        }

        public async UniTask DoStopShowing()
        {
            _backgroundFadeGroup.alpha = 0f;
        }


        private void SetVideoContents(VideoDisplayConfig displayConfig)
        {
            _videoPlayer.clip = displayConfig.VideoClip;
            _videoPlayer.time = 0;
        }
        
    }
}