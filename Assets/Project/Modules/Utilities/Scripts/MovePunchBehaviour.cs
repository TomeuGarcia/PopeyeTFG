using DG.Tweening;
using NaughtyAttributes;
using Project.Scripts.TweenExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Popeye.Modules.Utilities
{
    public class MovePunchBehaviour : MonoBehaviour
    {
        [MinMaxSlider(0f, 10f)][SerializeField] private Vector2 _randomDelayInterval = new Vector2(0f, 0.5f);

        [SerializeField] private TweenPunchConfig _positionPunch;
        private bool _keepPlaying;

        private void Start()
        {
            Resume();
        }


        private IEnumerator Move()
        {
            while (_keepPlaying)
            {
                yield return transform.PunchPosition(_positionPunch).WaitForCompletion();
                yield return new WaitForSeconds(Random.Range(_randomDelayInterval.x, _randomDelayInterval.y));
            }
        }

        public void Stop()
        {
            _keepPlaying = false;
        }
        public void Resume()
        {
            if (_keepPlaying) return;
            
            _keepPlaying = true;
            StartCoroutine(Move());
        }
        
    }

}