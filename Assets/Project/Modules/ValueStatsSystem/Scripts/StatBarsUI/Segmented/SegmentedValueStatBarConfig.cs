using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.ValueStatSystem.Segmented
{
    [CreateAssetMenu(fileName = "SegmentedValueStatBarConfig_NAME", 
        menuName = ScriptableObjectsHelper.VALUESTATS_ASSETS_PATH + "SegmentedValueStatBarConfig")]
    public class SegmentedValueStatBarConfig : ScriptableObject
    {
        [SerializeField, Range(1, 100)] private int _statValueAmountPerUnit = 1;

        [SerializeField] private Vector2 _spacingBetweenCells = new Vector2(20, 0);
        [SerializeField] private RectOffset _paddingCells = new RectOffset();


        public Vector2 SpacingBetweenCells => _spacingBetweenCells;
        public RectOffset PaddingCells => _paddingCells;
        
        

        public int NumberOfSegments(int maxStatValue, out int reminder)
        {
            int numberOfSegments = maxStatValue / _statValueAmountPerUnit;
            reminder = maxStatValue % _statValueAmountPerUnit;
            
            bool exactDivision = reminder == 0;
            if (!exactDivision)
            {
                Debug.LogWarning($"Reminder doesn't equal 0: {maxStatValue} / {_statValueAmountPerUnit} = {numberOfSegments} " +
                                 $"(Reminder of: {reminder})");
            }

            return numberOfSegments;
        }
        
        public int IndexOfSegment(int statValue)
        {
            
            int numberOfSegments = statValue / _statValueAmountPerUnit;
            int reminder = statValue % _statValueAmountPerUnit;

            int indexOfSegments = numberOfSegments - (reminder == 0 ? 1 : 0);
            
            return indexOfSegments;
        }
        
        
    }
}