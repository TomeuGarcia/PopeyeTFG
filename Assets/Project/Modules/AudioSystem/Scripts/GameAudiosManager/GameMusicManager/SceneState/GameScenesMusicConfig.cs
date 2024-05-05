using System;
using System.Linq;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;

namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    [System.Serializable]
    public class GameScenesMusicConfig
    {
        [System.Serializable]
         public class MusicSoundsGroup
         {
             [SerializeField] private LastingFMODSound[] _sounds;
             
             public LastingFMODSound[] Sounds => _sounds;
         }

         
         [System.Serializable]
         private class MusicSoundsByScene
         {
             [Header("SCENE")]
             [SerializeField] private SceneReferenceAsset _sceneReference;
             
             [Header("SOUNDS")]
             [SerializeField] private MusicSoundsGroup _musicSoundsGroup;
             public MusicSoundsGroup MusicSoundsGroup => _musicSoundsGroup;

             public bool IsScene(ISceneReference sceneReference)
             {
                 return ReferenceEquals(sceneReference, _sceneReference);
             }
         }


         [Header("SCENES")]
         [SerializeField] private SceneReferenceAsset[] _scenesToIgnore;
         [SerializeField] private MusicSoundsGroup _defaultSounds;
         [SerializeField] private MusicSoundsByScene[] _soundsByScene;

         public bool GetSoundForScene(ISceneReference sceneReference, out MusicSoundsGroup musicSoundsGroups)
         {
             if (_scenesToIgnore.Contains(sceneReference))
             {
                 musicSoundsGroups = null;
                 return false;
             }
             
            foreach (MusicSoundsByScene musicSoundsByScene in _soundsByScene)
            {
                if (musicSoundsByScene.IsScene(sceneReference))
                {
                    musicSoundsGroups = musicSoundsByScene.MusicSoundsGroup;
                    return true;
                }
            }
            
            musicSoundsGroups = _defaultSounds;
            return true;
         }
         
         
    }
}