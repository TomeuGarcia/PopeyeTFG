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
             [SerializeField] private LastingFMODSound _ambientSound;
             [SerializeField] private LastingFMODSound _musicSound;
             
             public LastingFMODSound AmbientSound => _ambientSound;
             public LastingFMODSound MusicSound => _musicSound;
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
         [SerializeField] private MusicSoundsByScene[] _soundsByScene;

         public bool GetSoundForScene(ISceneReference sceneReference, out MusicSoundsGroup musicSoundsGroup)
         {
            foreach (MusicSoundsByScene musicSoundsByScene in _soundsByScene)
            {
                if (musicSoundsByScene.IsScene(sceneReference))
                {
                    musicSoundsGroup = musicSoundsByScene.MusicSoundsGroup;
                    return true;
                }
            }
            
            musicSoundsGroup = null;
            return false;
         }
         
         
    }
}