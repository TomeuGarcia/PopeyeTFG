using System.Collections.Generic;
using Popeye.Core.Services.CommandQueue;
using Popeye.Core.Services.EventSystem;
using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    public class SceneLoadManager : ISceneLoadManager
    {
        private readonly ICommandQueueService _commandQueueService;
        private readonly IEventSystemService _eventSystemService;
        private readonly ISceneTransitionScreenFader _screenFader;

        private readonly List<SceneReferenceAsset> _persistentScenes;
        private readonly List<SceneReferenceAsset> _nonPersistentScenes;
        

        public SceneLoadManager(ICommandQueueService commandQueueService, IEventSystemService eventSystemService, 
            ISceneTransitionScreenFader screenFader)
        {
            _commandQueueService = commandQueueService;
            _eventSystemService = eventSystemService;
            _screenFader = screenFader;
            _persistentScenes = new List<SceneReferenceAsset>(5);
            _nonPersistentScenes = new List<SceneReferenceAsset>(5);
        }

        public void LoadSceneAdditively(ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup)
        {
            SceneReferenceAsset sceneReference = sceneLoadGroup.sceneReference;
            SceneLoadOptions loadOptions = sceneLoadGroup.loadOptions.AdditiveSceneLoadOptions;
            
            LoadSceneAdditivelyCommand loadSceneCommand = 
                new (sceneReference, loadOptions.DelayBeforeLoading, OnStartLoadingSceneAdditively);
            DoLoadSceneAdditively(sceneReference, loadOptions, loadSceneCommand);
        }
        private void DoLoadSceneAdditively(SceneReferenceAsset sceneReference, SceneLoadOptions loadOptions,
            ISceneLoadCommand sceneLoadCommand)
        {
            _commandQueueService.AddCommand(sceneLoadCommand);

            if (loadOptions.FadeScreen)
            {
                _screenFader.FadeScreen(() => sceneLoadCommand.FinishedLoading);
            }
            
            
            UnloadAndClearCurrentScenes(
                loadOptions.UnloadPreviousPersistentScenes, 
                loadOptions.UnloadPreviousNonPersistentScenes);
                
            SaveSceneReference(sceneReference, loadOptions.IsPersistentScene);
        }

        
        public void LoadScene(ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup)
        {
            SceneReferenceAsset sceneReference = sceneLoadGroup.sceneReference;
            SceneLoadOptions loadOptions = sceneLoadGroup.loadOptions.AdditiveSceneLoadOptions;
            
            LoadSceneCommand loadSceneCommand = 
                new LoadSceneCommand(sceneReference, loadOptions.DelayBeforeLoading);
            
            
            DoLoadScene(sceneReference, loadOptions, loadSceneCommand);
        }
        private void DoLoadScene(SceneReferenceAsset sceneReference, SceneLoadOptions loadOptions,
            ISceneLoadCommand sceneLoadCommand)
        {
            _commandQueueService.AddCommand(sceneLoadCommand);

            if (loadOptions.FadeScreen)
            {
                _screenFader.FadeScreen(() => sceneLoadCommand.FinishedLoading);
            }
            
            _persistentScenes.Clear();
            _nonPersistentScenes.Clear();
                
            SaveSceneReference(sceneReference, loadOptions.IsPersistentScene);
        }


        
        public void UnloadScene(SceneReferenceAsset sceneReference)
        {
            DoUnloadScene(sceneReference);
            RemoveSceneReference(sceneReference);
        }
        private void DoUnloadScene(SceneReferenceAsset sceneReference)
        {
            UnloadSceneCommand unloadSceneCommand = 
                new (sceneReference, OnStartUnloadingSceneAdditively);
            _commandQueueService.AddCommand(unloadSceneCommand);
        }

        
        public void ReloadCurrentScene(SceneLoadOptionsAsset loadOptions)
        {
            if (_nonPersistentScenes.Count < 1)
            {
                return;
            }

            SceneReferenceAsset lastLoadedScene = _nonPersistentScenes[^1];
            UnloadScene(lastLoadedScene);
            LoadSceneAdditively(new ISceneLoadManager.SceneAdditiveLoadGroup
            {
                sceneReference = lastLoadedScene,
                loadOptions = loadOptions
            });
        }
        

        private void SaveSceneReference(SceneReferenceAsset sceneReference, bool isPersistentScene)
        {
            if (isPersistentScene)
            {
                _persistentScenes.Add(sceneReference);
            }
            else
            {
                _nonPersistentScenes.Add(sceneReference);
            }
        }

        private void RemoveSceneReference(SceneReferenceAsset sceneReference)
        {
            if (_persistentScenes.Contains(sceneReference))
            {
                _persistentScenes.Remove(sceneReference);
                return;
            }
            
            if (_nonPersistentScenes.Contains(sceneReference))
            {
                _nonPersistentScenes.Remove(sceneReference);
            }
            
        }
        
        private void UnloadAndClearCurrentScenes(bool unloadPersistentScenes, bool unloadNonPersistentScenes)
        {
            if (unloadPersistentScenes)
            {
                UnloadAllAndClear(_persistentScenes);
            }
            
            if (unloadNonPersistentScenes)
            {
                UnloadAllAndClear(_nonPersistentScenes);
            }
        }

        private void UnloadAllAndClear(List<SceneReferenceAsset> scenesReferences)
        {
            foreach (SceneReferenceAsset sceneReference in scenesReferences)
            {
                DoUnloadScene(sceneReference);
            }
            scenesReferences.Clear();
        }
        
        
        
        
        private void OnStartLoadingSceneAdditively(ISceneReference sceneReference)
        {
            _eventSystemService.Dispatch<ISceneLoadManager.OnStartLoadingAdditiveSceneEvent>
                (new ISceneLoadManager.OnStartLoadingAdditiveSceneEvent(sceneReference));
        }
        private void OnStartUnloadingSceneAdditively(ISceneReference sceneReference)
        {
            _eventSystemService.Dispatch<ISceneLoadManager.OnStartUnloadingSceneEvent>
                (new ISceneLoadManager.OnStartUnloadingSceneEvent(sceneReference));
        }

    }
}