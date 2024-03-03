using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.WorldElements.WorldBuilders
{
    [CreateAssetMenu(fileName = "WallBuilderDataTracker", 
        menuName = ScriptableObjectsHelper.WALLBUILDER_ASSETS_PATH + "WallBuilderDataTracker")]
    public class WallBuilderDataTracker : ScriptableObject, IWallBuilderDataTracker
    {
        [System.Serializable] 
        private class WallsAndSeconds
        {
            [SerializeField] private int _numberOfWalls = 5;
            [SerializeField] private float _secondsToPlace = 30f;
            
            public float WallPerSecondRatio => _numberOfWalls / _secondsToPlace;

            public float TheoreticalSecondsForNumberOfWalls(int numberOfWalls)
            {
                return (_secondsToPlace / _numberOfWalls) * numberOfWalls;
            }
        }

        [SerializeField] private WallsAndSeconds _handPlacedWalls; 
        [SerializeField] private WallsAndSeconds _wallBuilderPlacedWalls; 
        
        [ShowNonSerializedField] private int _instantiatedWallsCounter;
        
        [ShowNonSerializedField] private float _theoreticalHandPlacedSeconds;
        [ShowNonSerializedField] private float _theoreticalWallBuilderPlacedSeconds;
        [ShowNonSerializedField] private float _savedSeconds;
        [ShowNonSerializedField] private float _savedMinutes;
        [ShowNonSerializedField] private float _savedHours;

        
        
        [Button()]
        private void ResetInstantiatedWallsCounter()
        {
            _instantiatedWallsCounter = 0;
            _savedSeconds = 0;
        }
        
        [Button()]
        private void UpdateSavedTime()
        {
            _theoreticalHandPlacedSeconds = 
                _handPlacedWalls.TheoreticalSecondsForNumberOfWalls(_instantiatedWallsCounter);
            
            _theoreticalWallBuilderPlacedSeconds = 
                _wallBuilderPlacedWalls.TheoreticalSecondsForNumberOfWalls(_instantiatedWallsCounter);

            _savedSeconds = _theoreticalHandPlacedSeconds - _theoreticalWallBuilderPlacedSeconds;
            _savedMinutes = _savedSeconds / 60f;
            _savedHours = _savedMinutes / 60f;
        }
        
        
        public void OnWallInstantiated()
        {
            ++_instantiatedWallsCounter;
        }
    }
}