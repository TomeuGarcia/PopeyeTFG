using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.AnimationDeath
{
    public class PlayerDeathAnimationSequencer : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private PlayerDeathChain[] _playerDeathChains;


        private void Awake()
        {
            ResetState();
        }

        private void ResetState()
        {
            foreach (PlayerDeathChain playerDeathChain in _playerDeathChains)
            {
                playerDeathChain.ResetState();
            }
        }
        
        [Button()]
        private void PlayDeathAnimation()
        {
            ResetState();
            StartCoroutine(DeathAnimation());
        }

        private IEnumerator DeathAnimation()
        {

            foreach (PlayerDeathChain playerDeathChain in _playerDeathChains)
            {
                yield return StartCoroutine(playerDeathChain.MoveToTarget(_player));
            }
        }
    }
}