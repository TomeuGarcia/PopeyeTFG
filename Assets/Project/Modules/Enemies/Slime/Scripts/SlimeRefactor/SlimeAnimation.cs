using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimation : ISlimeComponent
{
    
    [SerializeField] private GameObject _explosionParticles;
    
    [SerializeField] private float _squashAmount = 0.8f;
    [SerializeField] private float _stretchAmount = 1.2f;
    [SerializeField] private float _animationSpeed = 5f;

    private bool _doSquashing = true;
    private bool _isSquashing = false;


    private void Update()
    {
        if (_doSquashing)  
        {
            if (!_isSquashing)
            {
                StartCoroutine(SquashAndStretch());
            }
        }
    }

    public void SpawnExplosionParticles()
    {
        Instantiate(_explosionParticles, transform.position, Quaternion.identity);
    }

    public void ActivateAnimation()
    {
        _doSquashing = true;
    }

    public void DeactivateAnimation()
    {
        _doSquashing = false;
    }
    
    
    IEnumerator SquashAndStretch()
    {
        _isSquashing = true;

        // Squash
        while (transform.localScale.y > _squashAmount)
        {
            transform.localScale -= new Vector3(0f, Time.deltaTime * _animationSpeed, 0f);
            transform.localScale += new Vector3(Time.deltaTime * _animationSpeed/2, 0, Time.deltaTime * _animationSpeed/2);
            yield return null;
        }
        
        // Stretch
        while (transform.localScale.y < _stretchAmount)
        {
            transform.localScale += new Vector3(0f, Time.deltaTime * _animationSpeed, 0f);
            transform.localScale -= new Vector3(Time.deltaTime * _animationSpeed/2, 0, Time.deltaTime * _animationSpeed/2);
            yield return null;
        }


        _isSquashing = false;
    }
}
