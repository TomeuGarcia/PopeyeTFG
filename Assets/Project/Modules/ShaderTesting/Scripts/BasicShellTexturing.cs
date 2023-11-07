using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShellTexturing : MonoBehaviour
{
    [SerializeField] private Material _ShellTexturingMaterial;

    [SerializeField] private MeshRenderer _meshPrefab;


    private MeshRenderer[] _meshes;

    [SerializeField, Range(1, 100)] private int _numberOfMeshes = 10;
    [SerializeField, Range(1, 200)] private int _resolution = 50;
    private int _oldResolution;
    


    private void Awake()
    {
        int meshesBuffer = 100;
        _meshes = new MeshRenderer[meshesBuffer];
        for (int i = 0; i < meshesBuffer; ++i)
        {
            _meshes[i] = Instantiate(_meshPrefab, transform);
            _meshes[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _numberOfMeshes; ++i)
        {
            _meshes[i].gameObject.SetActive(true);
            _meshes[i].material.SetFloat("_Height01", (float)i / _numberOfMeshes);
        }
        UpdateResolution();
    }


    private void Update()
    {
        if (_resolution != _oldResolution)
        {
            UpdateResolution();
        }
    }


    private void UpdateResolution()
    {
        for (int i = 0; i < _numberOfMeshes; ++i)
        {
            _meshes[i].material.SetFloat("_Resolution", _resolution);
        }

        _oldResolution = _resolution;
    }

}
