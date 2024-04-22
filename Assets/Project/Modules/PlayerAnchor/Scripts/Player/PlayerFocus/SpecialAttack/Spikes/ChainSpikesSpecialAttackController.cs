using System;
using Cysharp.Threading.Tasks;
using Popeye.Modules.PlayerAnchor.Anchor;
using Popeye.Modules.PlayerAnchor.Chain;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spikes
{
    public class ChainSpikesSpecialAttackController : MonoBehaviour, IPlayerSpecialAttackController
    {
        [SerializeField] private AnchorChain _anchorChain;
        [SerializeField, Range(0f, 1f)] private float _startRatio = 0.2f;
        [SerializeField, Range(0f, 1f)] private float _endRatio = 0.8f;
        [SerializeField, Range(0, 20)] private int _numberOfPoints = 6;

        [SerializeField] private ChainSpike _chainSpikePrefab;

        [SerializeField] private float _duration = 1.0f;
        [SerializeField] private float _delay = 0.1f;
        private bool _isActive = false;

        private ChainSpike.SpikePositioning[] _spikesPositioning;
        private ChainSpike[] _spikes;

        private IPlayerFocusSpender _focusSpender;
        private PlayerFocusAttackConfig _focusAttackConfig;
        private IAnchorMediator _anchorMediator;
        
        public void Configure(IPlayerFocusSpender focusSpender, 
            PlayerFocusAttackConfig focusAttackConfig,
            IAnchorMediator anchorMediator)
        {
            _focusSpender = focusSpender;
            _focusAttackConfig = focusAttackConfig;
            _anchorMediator = anchorMediator;
        }

        private void OnValidate()
        {
            _spikesPositioning = new ChainSpike.SpikePositioning[_numberOfPoints];
            _spikes = new ChainSpike[_numberOfPoints];
            
            for (int i = 0; i < _numberOfPoints; ++i)
            {
                _spikesPositioning[i] = new ChainSpike.SpikePositioning();
            }
        }

        private void Awake()
        {
            OnValidate();
        }

        public bool CanDoSpecialAttack()
        {
            return _focusSpender.HasEnoughFocus(_focusAttackConfig.RequiredFocusToPerform) && 
                   !SpecialAttackIsBeingPerformed() &&
                   !_anchorMediator.IsBeingCarried();
        }

        public bool SpecialAttackIsBeingPerformed()
        {
            return _isActive;
        }

        public void StartSpecialAttack()
        {
            _focusSpender.SpendFocus(_focusAttackConfig.RequiredFocusToPerform);
        
            for (int i = 0; i < _numberOfPoints; ++i)
            {
                ChainSpike chainSpike = Instantiate(_chainSpikePrefab, transform);
                chainSpike.Init(_spikesPositioning[i]);
                _spikes[i] = chainSpike;
            }

            Activate().Forget();
        }

        public bool SpecialAttackHasFinished()
        {
            return !_isActive;
        }

        private async UniTaskVoid Activate()
        {
            _isActive = true;

            for (int i = 0; i < _spikes.Length; ++i)
            {
                _spikes[i].PlaySpawnAnimation().Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(_delay));
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(_duration));
            _isActive = false;
        }

        private void LateUpdate()
        {
            if (_isActive)
            {
                UpdateSpikesPositioningState();
            }            
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < _spikesPositioning.Length; ++i)
            {
                Vector3 position = _spikesPositioning[i].position;
                Vector3 normal = _spikesPositioning[i].normal;
                
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(position + normal, 0.5f);
                Gizmos.DrawLine(position, position + normal * 6);
            }
        }
        private void UpdateSpikesPositioningState()
        {
            Vector3[] chainPositions = _anchorChain.GetChainPositions();
            int numberOfChains = chainPositions.Length;
            
            int startIndex = (int)(numberOfChains * _startRatio);
            int endIndex = (int)(numberOfChains * _endRatio);
            
            float indexAmount = endIndex - startIndex;

            float indexStep = indexAmount / _numberOfPoints;
            
            
            int count = 0;
            for (float f = startIndex; f < endIndex; f += indexStep)
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
        
        
    }
}