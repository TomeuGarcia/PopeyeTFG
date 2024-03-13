using NaughtyAttributes;
using Popeye.Modules.AudioSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.Enemies.General.Scripts.EnemySpwner.Audio
{
    [CreateAssetMenu(fileName = "EnemySpawnerAudioConfig", 
        menuName = ScriptableObjectsHelper.ENEMIES_ASSET_PATH + "EnemySpawnerAudioConfig")]
    public class EnemySpawnerAudioConfig : ScriptableObject
    {
        [Expandable] [SerializeField] private OneShotFMODSound _enemyWavesStart;
        [Expandable] [SerializeField] private OneShotFMODSound _enemyWavesCompleted;
        
        public OneShotFMODSound EnemyWavesStart => _enemyWavesStart;
        public OneShotFMODSound EnemyWavesCompleted => _enemyWavesCompleted;
    }
}