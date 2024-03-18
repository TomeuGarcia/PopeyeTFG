using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using Popeye.Modules.Camera.CameraZoom;

namespace Popeye.Modules.Camera.CameraZoom
{
    public class CameraZoomer : ICameraZoomer
    {
        private readonly ICameraController _orbitingCamera;
        private DG.Tweening.Core.TweenerCore<float, float, FloatOptions> _currentZoom;

        public CameraZoomer(ICameraController orbitingCamera)
        {
            _orbitingCamera = orbitingCamera;
        }
        
        public void ZoomIn(CameraZoomConfig zoomInConfig)
        {
            float endDistance = _orbitingCamera.Distance - zoomInConfig.ZoomDistance;
            DoZoom(zoomInConfig, endDistance);
        }
        public void ZoomOut(CameraZoomConfig zoomOutConfig)
        {
            float endDistance = _orbitingCamera.Distance + zoomOutConfig.ZoomDistance;
            DoZoom(zoomOutConfig, endDistance);
        }
        public void ZoomToDefault(CameraZoomConfig zoomConfig)
        {
            DoZoom(zoomConfig, _orbitingCamera.DefaultDistance);
        }
        
        
        public async UniTaskVoid ZoomInOut(CameraZoomInOutConfig zoomInOutConfig)
        {
            ZoomIn(zoomInOutConfig.ZoomInConfig);
            var expectedZoom = _currentZoom;

            await UniTask.Delay(
                TimeSpan.FromSeconds(zoomInOutConfig.ZoomInConfig.Duration + zoomInOutConfig.DelayBetweenZooms));
            
            if (expectedZoom != _currentZoom)
            {
                return;
            }
            ZoomOut(zoomInOutConfig.ZoomOutConfig);
        }

        public async UniTaskVoid ZoomOutIn(CameraZoomInOutConfig zoomInOutConfig)
        {
            ZoomOut(zoomInOutConfig.ZoomOutConfig);
            var expectedZoom = _currentZoom;

            await UniTask.Delay(
                TimeSpan.FromSeconds(zoomInOutConfig.ZoomOutConfig.Duration + zoomInOutConfig.DelayBetweenZooms));
            
            if (expectedZoom != _currentZoom)
            {
                return;
            }
            ZoomIn(zoomInOutConfig.ZoomInConfig);
        }



        public async UniTaskVoid ZoomInOutToDefault(CameraZoomInOutConfig zoomInOutConfig)
        {
            ZoomIn(zoomInOutConfig.ZoomInConfig);
            var expectedZoom = _currentZoom;
            
            await UniTask.Delay(
                TimeSpan.FromSeconds(zoomInOutConfig.ZoomInConfig.Duration + zoomInOutConfig.DelayBetweenZooms));
            
            if (expectedZoom != _currentZoom)
            {
                return;
            }
            ZoomToDefault(zoomInOutConfig.ZoomOutConfig);
        }

        public async UniTaskVoid ZoomOutInToDefault(CameraZoomInOutConfig zoomInOutConfig)
        {
            ZoomOut(zoomInOutConfig.ZoomOutConfig);
            var expectedZoom = _currentZoom;

            await UniTask.Delay(
                TimeSpan.FromSeconds(zoomInOutConfig.ZoomOutConfig.Duration + zoomInOutConfig.DelayBetweenZooms));
            
            if (expectedZoom != _currentZoom)
            {
                return;
            }
            ZoomToDefault(zoomInOutConfig.ZoomInConfig);
        }

        
        private void DoZoom(CameraZoomConfig zoomConfig, float endDistance)
        {
            _currentZoom = DOTween.To(
                    () => _orbitingCamera.Distance,
                    (newDistance) => _orbitingCamera.SetDistance(newDistance),
                    endDistance,
                    zoomConfig.Duration
                )
                .SetEase(zoomConfig.EaseCurve);
        }

        public void KillCurrentZoom()
        {
            if (_currentZoom.IsPlaying())
            {
                _currentZoom.Kill();
            }
        }
    }
}