using DG.Tweening;
using NaughtyAttributes;
using Project.Scripts.TweenExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Popeye.Modules.Utilities.Scripts
{
    public class MovePunchBehaviour : MonoBehaviour
    {
        [MinMaxSlider(0f, 10f)][SerializeField] private Vector2 _randomDelayInterval = new Vector2(0f, 0.5f);

        [SerializeField] private TweenPunchConfig _positionPunch;

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(_randomDelayInterval.x, _randomDelayInterval.y));
                yield return transform.PunchPosition(_positionPunch).WaitForCompletion();
            }
        }
    }

}