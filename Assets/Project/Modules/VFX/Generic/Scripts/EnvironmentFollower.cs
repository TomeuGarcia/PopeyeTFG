using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentFollower : MonoBehaviour
{
    [System.Serializable]
    public struct EnvironmentElement
    {
        public Transform _transform;
        public float _desiredWorldHeight;
    }
    
    public Transform _followTarget;
    public List<EnvironmentElement> _environmentElements = new();

    void Update()
    {
        foreach (var element in _environmentElements)
        {
            element._transform.position = new Vector3(_followTarget.position.x, element._desiredWorldHeight, _followTarget.position.z);
        }
    }
}
