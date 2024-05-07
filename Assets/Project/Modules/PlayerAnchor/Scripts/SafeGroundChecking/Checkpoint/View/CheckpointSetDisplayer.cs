using System;
using System.Collections;
using AYellowpaper;
using Cysharp.Threading.Tasks;
using Popeye.Core.Services.InformationDisplay;
using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint.View
{
    public class CheckpointSetDisplayer : MonoBehaviour
    {
        [Header("CONFIG")]
        [SerializeField] private CheckpointSetDisplayerConfig _config;
        
        [Header("TEXT DISPLAYER")]
        [SerializeField] private InterfaceReference<ITextDisplayer, MonoBehaviour> _textDisplayer;

        [Header("EVENT CHANNEL")]
        [SerializeField] private InterfaceReference<IEmptyEventChannelListenEntry, ScriptableObject> _checkpointSetChannel;
        
        private ITextDisplayer TextDisplayer => _textDisplayer.Value;
        private IEmptyEventChannelListenEntry CheckpointSetChannel => _checkpointSetChannel.Value;

        private Coroutine _displayingCheckpointCoroutine;

        private void OnDestroy()
        {
            if (_displayingCheckpointCoroutine != null)
            {
                StopCoroutine(_displayingCheckpointCoroutine);
            }
        }

        private void OnEnable()
        {
            CheckpointSetChannel.Subscribe(OnCheckpointSetEvent);
        }
        private void OnDisable()
        {
            CheckpointSetChannel.Unsubscribe(OnCheckpointSetEvent);
        }

        private void OnCheckpointSetEvent()
        {
            _displayingCheckpointCoroutine = StartCoroutine(DisplayCheckpointSet());
        }

        private IEnumerator DisplayCheckpointSet()
        {
            yield return new WaitForSeconds(_config.DisplayDelay);
            TextDisplayer.StartShowing(_config.CheckpointSetTextDisplay);

            yield return new WaitForSeconds(_config.DisplayDuration);
            TextDisplayer.StopShowing(_config.CheckpointSetTextDisplay);

            _displayingCheckpointCoroutine = null;
        }
        
        
        

    }
}