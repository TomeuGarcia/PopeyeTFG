using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Timers
{
    public class Lifetime
    {
        private readonly Action _onCompleteCallback;
        private readonly CancellationTokenSource _cancellationTokenSource;


        public Lifetime(float duration, Action onCompleteCallback)
        {
            _onCompleteCallback = onCompleteCallback;
            
            _cancellationTokenSource = new CancellationTokenSource();
            StartLifetime(duration).Forget();
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        private async UniTaskVoid StartLifetime(float duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: _cancellationTokenSource.Token);
            
            _onCompleteCallback();
        }
        
    }
}