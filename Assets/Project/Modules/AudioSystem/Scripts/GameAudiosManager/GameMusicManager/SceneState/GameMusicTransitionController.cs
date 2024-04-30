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

        private readonly List<LastingFMODSound.SoundId> _activeSounds;
        private GameScenesMusicConfig.MusicSoundsGroup _currentMusicSoundsGroup;


        public GameMusicTransitionController(
            IFMODAudioManager audioManager, 
            GameObject soundSource,
            GameScenesMusicConfig gameScenesMusicConfig
            )
        {
            _audioManager = audioManager;
            _soundSource = soundSource;
            _gameScenesMusicConfig = gameScenesMusicConfig;
            _activeSounds = new List<LastingFMODSound.SoundId>(2);
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
            _audioManager.StopLastingSounds(_activeSounds.ToArray());
            _activeSounds.Clear();
        }
        private void StartPlayingCurrentSounds()
        {
            foreach (LastingFMODSound lastingSound in _currentMusicSoundsGroup.Sounds)
            {
                _activeSounds.Add(
                    _audioManager.PlayLastingSound(lastingSound, _soundSource)
                );
            }
        }

        
    }
}