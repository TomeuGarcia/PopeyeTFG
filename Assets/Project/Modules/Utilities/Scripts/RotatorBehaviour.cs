using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorBehaviour : MonoBehaviour
{
    [SerializeField, Range(-100f, 100f)] private float _rotationSpeed = 10.0f;
    [SerializeField] private Vector3 _rotationAxis = Vector3.up;
    private Vector3 _localRotationAxis;
    private float _angle;

    private Quaternion _startRotation;


    private void OnValidate()
    {
        _rotationAxis = _rotationAxis.normalized;
        _localRotationAxis = transform.TransformDirection(_rotationAxis);
    }

    private void Awake()
    {
        OnValidate();
        _angle = 0;
        _startRotation = transform.localRotation;        
    }


    void Update()
    {
        _angle += Time.deltaTime * _rotationSpeed;
        transform.localRotation = Quaternion.AngleAxis(_angle, _localRotationAxis) * _startRotation;
    }
}
