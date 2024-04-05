using System.Collections.Generic;

using Popeye.Core.Services.CommandQueue;
using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    public class SceneLoadManager : ISceneLoadManager
    {
        private readonly ICommandQueueService _commandQueueService;
        private readonly ISceneTransitionScreenFader _screenFader;

        private readonly HashSet<SceneReferenceAsset> _persistentScenes;
        private readonly HashSet<SceneReferenceAsset> _nonPersistentScenes;
        

        public SceneLoadManager(ICommandQueueService commandQueueService, ISceneTransitionScreenFader screenFader)
        {
            _commandQueueService = commandQueueService;
            _screenFader = screenFader;
            _persistentScenes = new HashSet<SceneReferenceAsset>(5);
            _nonPersistentScenes = new HashSet<SceneReferenceAsset>(5);
        }

        public void LoadSceneAdditively(ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup)
        {
            SceneReferenceAsset sceneReference = sceneLoadGroup.sceneReference;
            SceneLoadOptions loadOptions = sceneLoadGroup.loadOptions.AdditiveSceneLoadOptions;
            
            LoadSceneAdditivelyCommand loadSceneCommand = 
                new LoadSceneAdditivelyCommand(sceneReference.SceneName, loadOptions.DelayBeforeLoading);
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
                new LoadSceneCommand(sceneReference.SceneName, loadOptions.DelayBeforeLoading);
            
            
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
                new UnloadSceneCommand(sceneReference.SceneName);
            _commandQueueService.AddCommand(unloadSceneCommand);
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

        private void UnloadAllAndClear(HashSet<SceneReferenceAsset> scenesReferences)
        {
            foreach (SceneReferenceAsset sceneReference in scenesReferences)
            {
                DoUnloadScene(sceneReference);
            }
            scenesReferences.Clear();
        }
    }
}