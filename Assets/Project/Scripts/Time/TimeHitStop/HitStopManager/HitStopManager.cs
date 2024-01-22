
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Core.DataStructures;
using Project.Scripts.Time.TimeScale;
using UnityEngine;

namespace Project.Scripts.Time.TimeHitStop
{
    public class HitStopManager : IHitStopManager
    {
        private delegate UniTask HitStopPlayFunctionType(HitStop hitStop);
        
        
        private readonly HitStopManagerConfig _config;
        private readonly ITimeScaleManager _timeScaleManager;
        private readonly TimeScaleTransitionController _timeScaleTransitionController;

        private bool _playingHitStops;
        private readonly CircularBuffer<HitStop> _pendingHitStops;
        private readonly Dictionary<HitStopTransitionType, HitStopPlayFunctionType> _hitStopsPlayFunctionMap;

        public HitStopManager(HitStopManagerConfig config, ITimeScaleManager timeScaleManager)
        {
            _config = config;
            _timeScaleManager = timeScaleManager;
            _timeScaleTransitionController = new TimeScaleTransitionController();

            _playingHitStops = false;
            _pendingHitStops = new CircularBuffer<HitStop>(_config.MaxSimultaneousHitStops);
            _hitStopsPlayFunctionMap = new Dictionary<HitStopTransitionType, HitStopPlayFunctionType>
                {
                    { HitStopTransitionType.Instant, PlayInstantHitStop},
                    { HitStopTransitionType.Transition, PlayTransitionHitStop}
                };
        }
        
        
        public void QueueHitStop(HitStopConfig hitStopConfig)
        {
            if (_pendingHitStops.IsFull())
            {
                return;
            }
            
            _pendingHitStops.AddNext(new HitStop(hitStopConfig));

            if (!_playingHitStops)
            {
                PlayPendingHitStops().Forget();
            }
        }

        private async UniTaskVoid PlayPendingHitStops()
        {
            _playingHitStops = true;
            
            while (_pendingHitStops.HasEnqueuedElements())
            {
                HitStop hitStop = _pendingHitStops.GetNext();
                await _hitStopsPlayFunctionMap[hitStop.TransitionType](hitStop);
                await UniTask.Delay(TimeSpan.FromSeconds(_config.DelayBetweenHitStops));
            }

            _playingHitStops = false;
        }
        
        private async UniTask PlayInstantHitStop(HitStop hitStop)
        {
            float originalTimeScale = _timeScaleManager.CurrentTimeScale;
            _timeScaleManager.SetTimeScale(hitStop.TimeScale);
            await UniTask.Delay(TimeSpan.FromSeconds(hitStop.RealtimeDuration), ignoreTimeScale: true);
            _timeScaleManager.SetTimeScale(originalTimeScale);
        }
        
        private async UniTask PlayTransitionHitStop(HitStop hitStop)
        {
            await _timeScaleTransitionController.ApplySmoothTimeScale(_timeScaleManager, 
                hitStop.TimeScale, hitStop.TransitionConfig);
        }
    }
}