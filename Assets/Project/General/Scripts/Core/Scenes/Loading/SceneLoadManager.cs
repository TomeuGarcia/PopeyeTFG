using System.Collections.Generic;
using Popeye.Core.Services.CommandQueue;
using Popeye.Modules.GameState;


namespace Popeye.Scripts.Core.Scenes
{
    public class SceneLoadManager : ISceneLoadManager
    {
        private readonly ICommandQueueService _commandQueueService;
        private readonly ISceneTransitionScreenFader _screenFader;
        private readonly IGameStateEventsDispatcher _gameStateEventsDispatcher;

        private readonly List<ISceneReference> _persistentScenes;
        private readonly List<ISceneReference> _nonPersistentScenes;

        private ISceneReference _lastLoadedScene;
        

        public SceneLoadManager(ICommandQueueService commandQueueService,
            ISceneTransitionScreenFader screenFader, IGameStateEventsDispatcher gameStateEventsDispatcher)
        {
            _commandQueueService = commandQueueService;
            _screenFader = screenFader;
            _gameStateEventsDispatcher = gameStateEventsDispatcher;
            _persistentScenes = new List<ISceneReference>(5);
            _nonPersistentScenes = new List<ISceneReference>(5);
        }

        public void LoadSceneAdditively(ISceneLoadManager.SceneAdditiveLoadGroup sceneLoadGroup)
        {
            ISceneReference sceneReference = sceneLoadGroup.SceneReference;
            SceneLoadOptions loadOptions = sceneLoadGroup.LoadOptions;
            
            LoadSceneAdditivelyCommand loadSceneCommand = 
                new (sceneReference, loadOptions.DelayBeforeLoading, 
                    OnStartLoadingSceneAdditively, OnFinishLoadingSceneAdditively);
            DoLoadSceneAdditively(sceneReference, loadOptions, loadSceneCommand);
        }
        private void DoLoadSceneAdditively(ISceneReference sceneReference, SceneLoadOptions loadOptions,
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
            ISceneReference sceneReference = sceneLoadGroup.SceneReference;
            SceneLoadOptions loadOptions = sceneLoadGroup.LoadOptions;
            
            LoadSceneCommand loadSceneCommand = 
                new LoadSceneCommand(sceneReference, loadOptions.DelayBeforeLoading,
                    OnStartLoadingAnyScene);
            
            
            DoLoadScene(sceneReference, loadOptions, loadSceneCommand);
        }
        

        private void DoLoadScene(ISceneReference sceneReference, SceneLoadOptions loadOptions,
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


        
        private void UnloadScene(ISceneReference sceneReference)
        {
            DoUnloadScene(sceneReference);
            RemoveSceneReference(sceneReference);
        }
        private void DoUnloadScene(ISceneReference sceneReference)
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

            ISceneReference lastLoadedScene = _nonPersistentScenes[^1];
            SceneLoadOptions additiveLoadOptions = loadOptions.AdditiveSceneLoadOptions;
            UnloadScene(lastLoadedScene);
            
            LoadSceneAdditivelyCommand loadSceneCommand = 
                new (lastLoadedScene, additiveLoadOptions.DelayBeforeLoading, 
                    OnStartLoadingSceneAdditively, OnFinishLoadingSceneAdditively);
            DoLoadSceneAdditively(lastLoadedScene, additiveLoadOptions, loadSceneCommand);
        }
        

        private void SaveSceneReference(ISceneReference sceneReference, bool isPersistentScene)
        {
            if (isPersistentScene)
            {
                _persistentScenes.Add(sceneReference);
            }
            else
            {
                _nonPersistentScenes.Add(sceneReference);
            }

            _lastLoadedScene = sceneReference;
        }

        private void RemoveSceneReference(ISceneReference sceneReference)
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
            
            _lastLoadedScene = null;
        }

        private void UnloadAllAndClear(List<ISceneReference> scenesReferences)
        {
            foreach (ISceneReference sceneReference in scenesReferences)
            {
                DoUnloadScene(sceneReference);
            }
            scenesReferences.Clear();            
        }
        
        
        
        
        private void OnStartLoadingSceneAdditively(ISceneReference sceneReference)
        {
            _gameStateEventsDispatcher.InvokeOnStartLoadingAdditiveScene(sceneReference);
            OnStartLoadingAnyScene(sceneReference);
        }
        private void OnStartLoadingAnyScene(ISceneReference sceneReference)
        {
            _gameStateEventsDispatcher.InvokeOnStartLoadingAnyScene(sceneReference);
        }
        private void OnFinishLoadingSceneAdditively(ISceneReference sceneReference)
        {
            if (sceneReference == _lastLoadedScene)
            {
                _gameStateEventsDispatcher.InvokeOnFinishLoadingScenes();
            }
        }
        private void OnStartUnloadingSceneAdditively(ISceneReference sceneReference)
        {
            _gameStateEventsDispatcher.InvokeOnStartUnloadingScene(sceneReference);
        }

    }
}