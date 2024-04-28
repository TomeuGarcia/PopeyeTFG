using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem.GameAudiosManager
{
    [CreateAssetMenu(fileName = "GameMusicConfig", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "GameMusicConfig")]
    public class GameMusicConfig : ScriptableObject
    {
        [Header("SCENES")]
        [SerializeField] private GameScenesMusicConfig _gameScenesMusicConfig;
        public GameScenesMusicConfig GameScenesMusicConfig => _gameScenesMusicConfig;
        
        [Header("PLAYER STATE")]
        [SerializeField] private PlayerStateMusicConfig _playerStateMusicConfig;
        public PlayerStateMusicConfig PlayerStateMusicConfig => _playerStateMusicConfig;
    }
}