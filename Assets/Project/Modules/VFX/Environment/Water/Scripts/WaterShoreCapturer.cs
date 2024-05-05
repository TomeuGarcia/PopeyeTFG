using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WaterShoreCapturer : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _shoreMeshes;
    
    private void Start()
    {
        DeactvateCamera();
    }

    public async UniTask DeactvateCamera()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        
        _camera.gameObject.SetActive(false);
        _shoreMeshes.gameObject.SetActive(false);
        
        GameObject.Find("WaterPlane").GetComponent<MeshRenderer>().material.SetVector("_ShoreTextureOffset", new Vector4(_camera.gameObject.transform.position.x, _camera.gameObject.transform.position.z, 0.0f, 0.0f));
    }
}
