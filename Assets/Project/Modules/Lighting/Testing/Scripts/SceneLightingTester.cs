using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using RenderSettings = UnityEngine.RenderSettings;

namespace Popeye.Modules.VFX.Testing
{
    public class SceneLightingTester : MonoBehaviour
    {
        [Header("LIGHTING COOKIE")]
        [SerializeField] private Transform _directionalLight;
        [SerializeField] private Vector3 _cookieScale;
        [SerializeField] private Vector3 _cookieScrollSpeed;
        
        [Header("GENERAL LIGHTING COLORS")]
        [SerializeField] private List<Color> _sceneLightingColors = new();
        private int _currentColorIndex = 0;

        private void Awake()
        {
            _sceneLightingColors.Insert(0, RenderSettings.ambientLight);
            Debug.Log(RenderSettings.ambientLight);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                UpdateLight(-1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                UpdateLight(1);
            }

            _directionalLight.position += _cookieScrollSpeed * Time.deltaTime;

            if (_directionalLight.position.x >= _cookieScale.x)
            {
                _directionalLight.position += Vector3.left * _cookieScale.x;
            }
            if (_directionalLight.position.z >= _cookieScale.z)
            {
                _directionalLight.position += Vector3.left * _cookieScale.z;
            }
        }

        private void UpdateLight(int indexDisplacement)
        {
            _currentColorIndex = (_currentColorIndex + indexDisplacement) % _sceneLightingColors.Count;
            if (_currentColorIndex == -1) { _currentColorIndex = _sceneLightingColors.Count - 1; }
            Debug.Log(_currentColorIndex);
            RenderSettings.ambientLight = _sceneLightingColors[_currentColorIndex];
        }
    }
}