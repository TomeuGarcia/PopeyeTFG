
using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Core.DataStructures;
using Project.Scripts.Time.TimeScale;
using UnityEngine;

namespace Project.Scripts.Time.TimeHitStop
{
    public class HitStopManager : IHitStopManager
    {
        private readonly HitStopManagerConfig _config;
        private readonly ITimeScaleManager _timeScaleManager;
        private readonly CircularBuffer<HitStop> _pendingHitStops;
        private bool _playingHitStops;


        public HitStopManager(HitStopManagerConfig config, ITimeScaleManager timeScaleManager)
        {
            _config = config;
            _timeScaleManager = timeScaleManager;
            _pendingHitStops = new CircularBuffer<HitStop>(_config.MaxSimultaneousHitStops);
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
                await PlayHitStop(_pendingHitStops.GetNext());
                await UniTask.Delay(TimeSpan.FromSeconds(_config.DelayBetweenHitStops));
            }

            _playingHitStops = false;
        }
        
        private async UniTask PlayHitStop(HitStop hitStop)
        {
            _timeScaleManager.SetTimeScale(hitStop.TimeScale);
            await UniTask.Delay(TimeSpan.FromSeconds(hitStop.RealtimeDuration), ignoreTimeScale: true);
            _timeScaleManager.SetTimeScale(_config.DefaultTimeScale);
        }
    }
}