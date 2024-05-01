using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    [System.Serializable]
    public class PlayerStateMusicConfig
    {
        [System.Serializable]
        private class GameStateTransitions
        {
            [Header("PARAMETER")]
            [SerializeField] private SoundParameter _playerGameStatusParameter;
        
            [Header("VALUES")]
            [SerializeField] private SoundParameterTransition _toExploration;
            [SerializeField] private SoundParameterTransition _toBattle;
            [SerializeField] private SoundParameterTransition _toDeath;
            
            public void ToExploration()
            {
                _toExploration.Transition(_playerGameStatusParameter).Forget();
            }

            public void ToBattle()
            {
                _toBattle.Transition(_playerGameStatusParameter).Forget();
            }

            public void ToDeath()
            {
                _toDeath.Transition(_playerGameStatusParameter).Forget();
            }
        }
        
        [System.Serializable]
        private class TakeDamageTransitions
        {
            [Header("PARAMETER")]
            [SerializeField] private SoundParameter _playerTakeDamageParameter;
        
            [Header("VALUES")]
            [SerializeField] private SoundParameterTransition _startTakingDamage;
            [SerializeField] private SoundParameterTransition _finishTakingDamage;
            
            public async UniTaskVoid ToTakingDamage()
            {
                await _startTakingDamage.Transition(_playerTakeDamageParameter);
                _finishTakingDamage.Transition(_playerTakeDamageParameter).Forget();
            }
        }
        
        
        [SerializeField] private GameStateTransitions _gameState;
        [Space(20)]
        [SerializeField] private TakeDamageTransitions _takeDamage;
        
        
        public void TransitionToExploration()
        {
            _gameState.ToExploration();
        }

        public void TransitionToBattle()
        {
            _gameState.ToBattle();
        }

        public void TransitionToDeath()
        {
            _gameState.ToDeath();
        }
        
        public void TransitionToTakeDamage()
        {
            _takeDamage.ToTakingDamage().Forget();
        }

    }
}