using System.Collections.Generic;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Modules.AudioSystem;
using Popeye.Scripts.TextUtilities;
using Project.Scripts.TweenExtensions;
using TMPro;
using UnityEngine;

namespace Popeye.Core.Services.InformationDisplay
{
    public class DisplayQueue<T> where T : class
    {
        private readonly IDisplayQueueDelegate _displayQueueDelegate;
        public T CurrentDisplay { get; private set; }
        
        private readonly Queue<T> _queuedDisplays;
        private bool _processingQueuedDisplays;

        private bool _isShowing;
        private bool _isHiding;
        
        public int CurrentDisplaysInQueue => _queuedDisplays.Count;

        
        public DisplayQueue(IDisplayQueueDelegate displayQueueDelegate)
        {
            _displayQueueDelegate = displayQueueDelegate;

            CurrentDisplay = null;
            
            _queuedDisplays = new Queue<T>(2);
            _processingQueuedDisplays = false;
            
            _isShowing = false;
            _isHiding = false;
        }
        

        public void StartShowing(T newDisplay)
        {
            _queuedDisplays.Enqueue(newDisplay);
            if (_processingQueuedDisplays)
            {
                return;
            }

            TransitionToNext().Forget();
        }

        private async UniTask TransitionToNext()
        {
            _processingQueuedDisplays = true;

            
            if (CurrentDisplay == null)
            {
                CurrentDisplay = _queuedDisplays.Peek();
                await StartShowingCurrent();
                _queuedDisplays.Dequeue();
            }
            
            
            while (CurrentDisplaysInQueue > 0)
            {
                await StopShowingCurrent();
                CurrentDisplay = _queuedDisplays.Peek();
                await StartShowingCurrent();
                _queuedDisplays.Dequeue();
            }
            
            _processingQueuedDisplays = false;
        } 
        
        private async UniTask StartShowingCurrent()
        {
            await UniTask.WaitUntil(() => !_isHiding);
            
            _isShowing = true;

            await _displayQueueDelegate.DoStartShowing();

            _isShowing = false;
        }

        public void StopShowing(T oldDisplay)
        {
            if (CurrentDisplay != oldDisplay) return;

            StopShowingCurrent().Forget();
        }
        
        private async UniTask StopShowingCurrent()
        {
            await UniTask.WaitUntil(() => !_isShowing);

            _isHiding = true;
            
            await _displayQueueDelegate.DoStopShowing();

            CurrentDisplay = null;
            _isHiding = false;
        }


        
    }
}