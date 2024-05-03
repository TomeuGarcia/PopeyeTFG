using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Modules.PlayerAnchor.Chain;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spikes
{
    public class ChainSpikesSpecialAttackController : MonoBehaviour, IPlayerSpecialAttackController
    {
        private ChainSpikesAttackConfig _config;
        private AnchorChain _anchorChain;

        private IPlayerFocusSpender _focusSpender;
        private PlayerFocusAttackConfig _focusAttackConfig;
        private IAnchorMediator _anchorMediator;

        private bool _isBeingPerformed = false;

        private ChainSpike.SpikePositioning[] _spikesPositioning;
        private ChainSpike[] _spikes;

        private int NumberOfPoints => _config.NumberOfSpikePoints;

        public float PreparationDuration => 0;
        
        public void Configure(
            ChainSpikesAttackConfig config,
            AnchorChain anchorChain,
            IPlayerFocusSpender focusSpender, 
            PlayerFocusAttackConfig focusAttackConfig,
            IAnchorMediator anchorMediator)
        {
            _config = config;
            _anchorChain = anchorChain;
            _focusSpender = focusSpender;
            _focusAttackConfig = focusAttackConfig;
            _anchorMediator = anchorMediator;

            OnValuesChanged();
            _config.OnValuesChanged += OnValuesChanged;
        }

        private void OnDestroy()
        {
            _config.OnValuesChanged -= OnValuesChanged;
        }

        private void OnValuesChanged()
        {
            _spikesPositioning = new ChainSpike.SpikePositioning[NumberOfPoints];
            _spikes = new ChainSpike[_config.NumberOfSpikePoints];
            
            for (int i = 0; i < NumberOfPoints; ++i)
            {
                _spikesPositioning[i] = new ChainSpike.SpikePositioning();
            }
        }
        

        public bool CanDoSpecialAttack()
        {
            return _focusSpender.HasEnoughFocus(_focusAttackConfig.RequiredFocusToPerform) && 
                   !SpecialAttackIsBeingPerformed() &&
                   !_anchorMediator.IsBeingCarried();
        }

        private bool SpecialAttackIsBeingPerformed()
        {
            return _isBeingPerformed;
        }

        public void StartSpecialAttack()
        {
            _focusSpender.SpendFocus(_focusAttackConfig.RequiredFocusToPerform);
        
            for (int i = 0; i < NumberOfPoints; ++i)
            {
                ChainSpike chainSpike = Instantiate(_config.ChainSpikePrefab , transform);
                chainSpike.Init(_spikesPositioning[i], _anchorMediator);
                _spikes[i] = chainSpike;
            }

            Activate().Forget();
        }

        public bool SpecialAttackHasFinished()
        {
            return !_isBeingPerformed;
        }
        
        public void ForceStopSpecialAttack()
        {
            // Not sure how I should stop the spikes [Tomeu]
        }

        private async UniTaskVoid Activate()
        {
            _isBeingPerformed = true;

            for (int i = 0; i < _spikes.Length; ++i)
            {
                _spikes[i].PlaySpawnAnimation().Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(_config.Delay));
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(_config.TotalDuration));
            _isBeingPerformed = false;
        }

        
        
        private void LateUpdate()
        {
            if (_isBeingPerformed)
            {
                UpdateSpikesPositioningState();
            }            
        }


        private void UpdateSpikesPositioningState()
        {
            UpdateSpikesPositioningState_Double();
        }
        private void UpdateSpikesPositioningState_Alternating()
        {
            Vector3[] chainPositions = _anchorChain.GetChainPositions();
            int numberOfChains = chainPositions.Length;
            
            int startIndex = (int)(numberOfChains * _config.FirstSpikePositionRatio);
            int endIndex = (int)(numberOfChains * _config.LastSpikePositionRatio);
            
            float indexAmount = endIndex - startIndex;

            float indexStep = indexAmount / NumberOfPoints;
            
            
            int count = 0;
            for (float f = startIndex; f < endIndex && count < NumberOfPoints; f += indexStep)
            {
                float t = f % 1f;
                int currentIndex = (int)f;
                int previousIndex = (int)f - 1;

                Vector3 previousPosition = chainPositions[previousIndex];
                Vector3 currentPosition = chainPositions[currentIndex];
                
                Vector3 position = Vector3.LerpUnclamped(previousPosition, currentPosition, t);
                Vector3 normal = Vector3.Cross((currentPosition - previousPosition), Vector3.up).normalized;
                normal *= (count % 2 == 0) ? 1 : -1;

                _spikesPositioning[count].position = position;
                _spikesPositioning[count].normal = normal;

                ++count;
            }
        }
        private void UpdateSpikesPositioningState_Double()
        {
            Vector3[] chainPositions = _anchorChain.GetChainPositions();
            int numberOfChains = chainPositions.Length;
            
            int startIndex = (int)(numberOfChains * _config.FirstSpikePositionRatio);
            int endIndex = (int)(numberOfChains * _config.LastSpikePositionRatio);
            
            float indexAmount = endIndex - startIndex;

            float indexStep = indexAmount / NumberOfPoints;
            
            
            int count = 0;
            for (float f = startIndex; f < endIndex && count < NumberOfPoints; f += indexStep * 2)
            {
                float t = f % 1f;
                int currentIndex = (int)f;
                int previousIndex = (int)f - 1;

                Vector3 previousPosition = chainPositions[previousIndex];
                Vector3 currentPosition = chainPositions[currentIndex];
                
                Vector3 position = Vector3.LerpUnclamped(previousPosition, currentPosition, t);
                Vector3 normal = Vector3.Cross((currentPosition - previousPosition), Vector3.up).normalized;

                _spikesPositioning[count].position = position;
                _spikesPositioning[count].normal = normal;

                ++count;
                
                _spikesPositioning[count].position = position;
                _spikesPositioning[count].normal = -normal;

                ++count;
            }
        }
        
        
    }
}