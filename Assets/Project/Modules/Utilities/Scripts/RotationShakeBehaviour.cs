using System;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using Project.Scripts.TweenExtensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Popeye.Modules.Utilities
{
    public class RotationShakeBehaviour : MonoBehaviour
    {
        [MinMaxSlider(0f, 10f)] [SerializeField] private Vector2 _randomDelayInterval = new Vector2(0f, 0.5f);
        
        [SerializeField] private TweenPunchConfig _rotationPunch;

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(_randomDelayInterval.x, _randomDelayInterval.y));            
                yield return transform.PunchRotation(_rotationPunch).WaitForCompletion();
            }
        }
        
    }
}