using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShellTexturing : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Material _ShellTexturingMaterial;
    [SerializeField] private MeshRenderer _meshPrefab;

    private MeshRenderer[] _meshes;
    private const int MESHES_BUFFER = 100;

    [SerializeField, Range(1, 100)] private int _numberOfMeshes = 10;
    private int _oldNumberOfMeshes;

    [SerializeField, Range(1, 200)] private int _resolution = 50;
    private int _oldResolution;
    



    private void Awake()
    {
        _meshPrefab.sharedMaterial = _ShellTexturingMaterial;
        _meshPrefab.gameObject.SetActive(false);

        _meshes = new MeshRenderer[MESHES_BUFFER];
        for (int i = 0; i < MESHES_BUFFER; ++i)
        {
            _meshes[i] = Instantiate(_meshPrefab, transform);
        }

        UpdateNumberOfMeshes();
        UpdateResolution();
    }


    private void Update()
    {
        if (_numberOfMeshes != _oldNumberOfMeshes)
        {
            UpdateNumberOfMeshes();
        }
        if (_resolution != _oldResolution)
        {
            UpdateResolution();
        }
        
    
    }


    private void UpdateNumberOfMeshes()
    {
        for (int i = 0; i < _numberOfMeshes; ++i)
        {
            _meshes[i].gameObject.SetActive(true);

            float height01 = (float)i / _numberOfMeshes;
            _meshes[i].material.SetFloat("_Height01", height01);
        }
        for (int i = _numberOfMeshes; i < MESHES_BUFFER; ++i)
        {
            _meshes[i].gameObject.SetActive(false);
        }

        _oldNumberOfMeshes = _numberOfMeshes;
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
