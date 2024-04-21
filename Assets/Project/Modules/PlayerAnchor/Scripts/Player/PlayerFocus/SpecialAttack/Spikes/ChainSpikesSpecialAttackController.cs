using System;
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

        private Vector3[] _chainPositions;
        private Vector3[] _chainForwards = new Vector3[0];
        private Vector3[] _chainNormals = new Vector3[0];
        
        public bool CanDoSpecialAttack()
        {
            return true;
        }

        public bool SpecialAttackIsBeingPerformed()
        {
            return false;
        }

        public void StartSpecialAttack()
        {
            
        }

        private void LateUpdate()
        {
            _chainPositions = _anchorChain.GetChainPositions();

            if (_chainForwards.Length != _chainPositions.Length)
            {
                _chainForwards = new Vector3[_chainPositions.Length];
                _chainNormals = new Vector3[_chainPositions.Length];
            }
            
            for (int i = 0; i < _chainPositions.Length - 1; ++i)
            {
                Vector3 chainForward = _chainPositions[i + 1] - _chainPositions[i];
                Vector3 chainNormal = Vector3.Cross(chainForward, Vector3.up).normalized;

                _chainNormals[i] = chainNormal;
            }
            Vector3 lastChainForward = _anchorChain.EndBindPosition - _chainPositions[^2];
            Vector3 lastChainNormal = Vector3.Cross(lastChainForward, Vector3.up).normalized;

            _chainNormals[^1] = lastChainNormal;
        }

        private void OnDrawGizmos()
        {
            if (_chainPositions == null) return;
            
            int startIndex = (int)(_chainPositions.Length * _startRatio);
            int endIndex = (int)(_chainPositions.Length * _endRatio);
            
            float indexAmount = endIndex - startIndex;

            float indexStep = indexAmount / _numberOfPoints;
            

            int count = 0;
            for (float f = startIndex; f < endIndex; f += indexStep)
            {
                float t = f % 1f;
                int currentIndex = (int)f;
                int previousIndex = (int)f - 1;

                Vector3 position = Vector3.LerpUnclamped(_chainPositions[previousIndex], _chainPositions[currentIndex], t);
                Vector3 normal = Vector3.LerpUnclamped(_chainNormals[previousIndex], _chainNormals[currentIndex], t);
                
                normal *= (count % 2 == 0) ? 1 : -1;
                
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(position + normal, 0.5f);
                Gizmos.DrawLine(position, position + normal * 6);

                ++count;
            }


        }
        
        
    }
}