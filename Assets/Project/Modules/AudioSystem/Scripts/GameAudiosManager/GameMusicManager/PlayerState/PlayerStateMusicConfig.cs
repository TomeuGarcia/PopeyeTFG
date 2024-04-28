using UnityEngine;

namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    [System.Serializable]
    public class PlayerStateMusicConfig
    {
        [Header("PARAMETER")]
        [SerializeField] private SoundParameter _playerGameStatusParameter;
        
        [Header("VALUES")]
        [SerializeField] private SoundParameterTransition _toExploration;
        [SerializeField] private SoundParameterTransition _toBattle;
        [SerializeField] private SoundParameterTransition _toDeath;
        
        
        public void TransitionToExploration()
        {
            _toExploration.Transition(_playerGameStatusParameter).Forget();
        }

        public void TransitionToBattle()
        {
            _toBattle.Transition(_playerGameStatusParameter).Forget();
        }

        public void TransitionToDeath()
        {
            _toDeath.Transition(_playerGameStatusParameter).Forget();
        }

    }
}