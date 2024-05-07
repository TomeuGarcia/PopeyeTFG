using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Popeye.Core.Services.EventSystem;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.GameState;
using UnityEngine;

public class WaterShoreCapturer : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private MeshRenderer _waterMeshRenderer;
    private IEventSystemService _eventSystemService;

    private void Start()
    {
        _eventSystemService = ServiceLocator.Instance.GetService<IEventSystemService>();
        _eventSystemService.Subscribe<IGameStateEventsDispatcher.OnFinishLoadingScenes>(OnFinishLoadingSceneEvent);
    }

    private void OnFinishLoadingSceneEvent(IGameStateEventsDispatcher.OnFinishLoadingScenes eventData)
    {
        StartCoroutine(LoadedScene());
    }

    private IEnumerator LoadedScene()
    {
        yield return new WaitForSeconds(0.2f);
        
        _camera.gameObject.SetActive(false);
        
        _waterMeshRenderer.material.SetVector("_ShoreTextureOffset", new Vector4(_camera.gameObject.transform.position.x, _camera.gameObject.transform.position.z, 0.0f, 0.0f));
    }
    
    private void OnDestroy()
    {
        _eventSystemService.Unsubscribe<IGameStateEventsDispatcher.OnFinishLoadingScenes>(OnFinishLoadingSceneEvent);
    }
}
