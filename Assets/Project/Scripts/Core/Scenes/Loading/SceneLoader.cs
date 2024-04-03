using System.Collections.Generic;

using Popeye.Core.Services.CommandQueue;

namespace Popeye.Scripts.Core.Scenes
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICommandQueueService _commandQueueService;
        private readonly ISceneTransitionScreenFader _screenFader;

        private readonly HashSet<SceneReferenceAsset> _persistentScenes;
        private readonly HashSet<SceneReferenceAsset> _nonPersistentScenes;
        

        public SceneLoader(ICommandQueueService commandQueueService, ISceneTransitionScreenFader screenFader)
        {
            _commandQueueService = commandQueueService;
            _screenFader = screenFader;
            _persistentScenes = new HashSet<SceneReferenceAsset>(5);
            _nonPersistentScenes = new HashSet<SceneReferenceAsset>(5);
        }

        public void LoadSceneAdditively(SceneReferenceAsset sceneReference, SceneLoadOptions loadOptions)
        {
            SaveSceneReference(sceneReference, loadOptions.IsPersistentScene);
            UnloadAllScenes(loadOptions.UnloadPreviousPersistentScenes, loadOptions.UnloadPreviousScenes);
            
            LoadSceneAdditivelyCommand loadSceneCommand = new (sceneReference.BuiltInSceneIndex);
            _commandQueueService.AddCommand(loadSceneCommand);

            _screenFader.FadeScreen(() => loadSceneCommand.FinishedLoading);
        }

        public void UnloadScene(SceneReferenceAsset sceneReference)
        {
            RemoveSceneReference(sceneReference);

            UnloadSceneCommand unloadSceneCommand = new(sceneReference.BuiltInSceneIndex);
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
            }
            
            if (_nonPersistentScenes.Contains(sceneReference))
            {
                _nonPersistentScenes.Remove(sceneReference);
            }
            
        }
        
        private void UnloadAllScenes(bool unloadPersistentScenes, bool unloadNonPersistentScenes)
        {
            if (unloadPersistentScenes)
            {
                foreach (SceneReferenceAsset sceneReference in _persistentScenes)
                {
                    UnloadScene(sceneReference);
                }
            }
            
            if (unloadNonPersistentScenes)
            {
                foreach (SceneReferenceAsset sceneReference in _nonPersistentScenes)
                {
                    UnloadScene(sceneReference);
                }
            }
        }

    }
}