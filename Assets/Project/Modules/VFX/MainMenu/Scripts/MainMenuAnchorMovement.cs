using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.TweenExtensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Popeye.Modules.VFX.MainMenu
{

    public class MainMenuAnchorMovement : MonoBehaviour
    {
        private float _minTime = 10.0f;
        private float _maxTime = 20.0f;

        private void Start()
        {
            StartCoroutine(DoMove(Vector3.up, 0.25f));
            StartCoroutine(DoRotate(Vector3.up, 10.0f));
            StartCoroutine(DoRotate(Vector3.right, 5.0f));
        }

        private IEnumerator DoMove(Vector3 axis, float strength)
        {
            while (true)
            {
                float randomTime = Random.Range(_minTime, _maxTime);
                float randomStrength = Random.Range(-strength, strength);
                transform.DOPunchPosition(axis * randomStrength, randomTime, 0, 0).SetEase(Ease.InOutSine);
                yield return new WaitForSeconds(randomTime);
            }
        }

        private IEnumerator DoRotate(Vector3 axis, float strength)
        {
            while (true)
            {
                float randomTime = Random.Range(_minTime, _maxTime);
                float randomStrength = Random.Range(-strength, strength);
                transform.DOBlendablePunchRotation(axis * randomStrength, randomTime, 0, 0).SetEase(Ease.InOutSine);
                yield return new WaitForSeconds(randomTime);
            }
        }
    }

}