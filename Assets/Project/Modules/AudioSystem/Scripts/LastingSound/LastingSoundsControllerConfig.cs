using Popeye.Core.Pool;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.AudioSystem
{
    [CreateAssetMenu(fileName = "LastingSoundsControllerConfig", 
        menuName = ScriptableObjectsHelper.SOUNDSYSTEM_ASSETS_PATH + "LastingSoundsControllerConfig")]
    public class LastingSoundsControllerConfig : ScriptableObject
    {
        [SerializeField, Range(1, 30)] private int _startNumberOfLastingSounds;
        [SerializeField] private RecyclableObject _lastingSoundEmitterPrefab;
        
        
        public int StartNumberOfLastingSounds => _startNumberOfLastingSounds;
        public RecyclableObject LastingSoundEmitterPrefab => _lastingSoundEmitterPrefab;
    }
}