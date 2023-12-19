using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TODELETE : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private Transform _circle;

    [System.Serializable]
    public class VFXInterpolateData
    {
        public float _startScale;
        public float _endScale;
        public float _fadeOutDelay;
        public float _fadeOutTime;
        public float _totalTime => _fadeOutDelay + _fadeOutTime;
    }

    [Header("CIRCLE")]
    public VFXInterpolateData _cIData;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _particles.gameObject.SetActive(true);
            FlashEffect().Forget();
        }
    }
    
    
    private async UniTaskVoid FlashEffect()
    {
        _circle.gameObject.SetActive(true);
        MeshRenderer circleMR = _circle.gameObject.GetComponent<MeshRenderer>();
        circleMR.material.SetFloat("_Alpha", 1.0f);

        _circle.localScale = new Vector3(_cIData._startScale, _cIData._startScale, _cIData._startScale);
        _circle.DOScale(new Vector3(_cIData._endScale, _cIData._endScale, _cIData._endScale), _cIData._totalTime);
        await UniTask.Delay(TimeSpan.FromSeconds(_cIData._fadeOutDelay));
        circleMR.material.DOFloat(0.0f, "_Alpha", _cIData._fadeOutTime);
        
        await UniTask.Delay(TimeSpan.FromSeconds(_cIData._fadeOutTime + 0.2f));
        _circle.gameObject.SetActive(false);
    }
}
