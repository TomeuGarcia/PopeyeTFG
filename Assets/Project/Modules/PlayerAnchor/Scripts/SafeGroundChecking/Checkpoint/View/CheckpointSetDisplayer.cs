using System;
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
            DisplayCheckpointSet().Forget();
        }

        private async UniTaskVoid DisplayCheckpointSet()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.DisplayDelay));
            
            TextDisplayer.StartShowing(_config.CheckpointSetTextDisplay);

            await UniTask.Delay(TimeSpan.FromSeconds(_config.DisplayDuration));
            
            TextDisplayer.StopShowing(_config.CheckpointSetTextDisplay);
        }
        
        
        

    }
}