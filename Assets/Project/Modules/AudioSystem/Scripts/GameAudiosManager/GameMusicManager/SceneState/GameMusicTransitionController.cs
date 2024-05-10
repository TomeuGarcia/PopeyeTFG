using System.Collections.Generic;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;

namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    public class GameMusicTransitionController : IGameMusicTransitionController
    {
        private readonly IFMODAudioManager _audioManager;
        private readonly GameObject _soundSource;
        private readonly GameScenesMusicConfig _gameScenesMusicConfig;

        private GameScenesMusicConfig.MusicSoundsGroup _currentMusicSoundsGroup;
        private GameMusicEmitter _currentMusicEmitter;

        public GameMusicTransitionController(
            IFMODAudioManager audioManager, 
            GameObject soundSource,
            GameScenesMusicConfig gameScenesMusicConfig
            )
        {
            _audioManager = audioManager;
            _soundSource = soundSource;
            _gameScenesMusicConfig = gameScenesMusicConfig;
            _currentMusicSoundsGroup = null;
        }

        

        public void TransitionToSceneMusic(ISceneReference sceneReference)
        {
            if (_gameScenesMusicConfig.GetSoundForScene(sceneReference,
                    out GameScenesMusicConfig.MusicSoundsGroup musicSoundsGroup))
            {
                if (_currentMusicSoundsGroup == musicSoundsGroup)
                {
                    return;
                }
                
                TransitionMusic(musicSoundsGroup);
            }
        }
        
        private void TransitionMusic(GameScenesMusicConfig.MusicSoundsGroup musicSoundsGroup)
        {
            StopPlayingCurrentSounds();
            _currentMusicSoundsGroup = musicSoundsGroup;
            StartPlayingCurrentSounds();
        }
        
        private void StopPlayingCurrentSounds()
        {
            GameObject.Destroy(_currentMusicEmitter);
        }
        private void StartPlayingCurrentSounds()
        {
            _currentMusicEmitter = GameObject.Instantiate(_gameScenesMusicConfig.GameMusicEmitterPrefab, _soundSource.transform);
            _currentMusicEmitter.Init(_currentMusicSoundsGroup.Sounds);
        }

        
    }
}