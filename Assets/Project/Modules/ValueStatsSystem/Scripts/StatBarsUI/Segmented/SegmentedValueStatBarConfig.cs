using System;
using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.UI;

namespace Popeye.Modules.ValueStatSystem.Segmented
{
    [CreateAssetMenu(fileName = "SegmentedValueStatBarConfig_NAME", 
        menuName = ScriptableObjectsHelper.VALUESTATS_ASSETS_PATH + "SegmentedValueStatBarConfig")]
    public class SegmentedValueStatBarConfig : ScriptableObject
    {
        [Header("NUMBER OF CELLS")]
        [SerializeField, Range(1, 100)] private int _statValueAmountPerUnit = 1;

        
        
        [Space(10)]
        [Header("CELLS CONFIGURATION")]
        [SerializeField] private bool _adaptSize = true;

        [ShowIf("_adaptSize")]
        [SerializeField] private Vector2 _spacingBetweenCells = new Vector2(20, 0);
        [ShowIf("_adaptSize")]
        [SerializeField] private RectOffset _paddingCells = new RectOffset();


        
        private ICellComputer _cellComputer;
        

        public void Init()
        {
            _cellComputer = _adaptSize
                ? new HolderRectAdaptCellComputer(_spacingBetweenCells, _paddingCells)
                : new FixedSizeCellComputer();
        }
        
        
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


        public Vector2 ComputeCellSize(int numberOfSegments, Rect holderRect, GridLayoutGroup gridLayoutGroup)
        {
            return _cellComputer.ComputeCellSize(numberOfSegments, holderRect, gridLayoutGroup);
        }

        public Vector2 ComputeSpacingBetweenCells(int numberOfSegments, Rect holderRect, GridLayoutGroup gridLayoutGroup)
        {
            return _cellComputer.ComputeSpacingBetweenCells(numberOfSegments, holderRect, gridLayoutGroup);
        }
        
        public RectOffset ComputePaddingCells(Rect holderRect, GridLayoutGroup gridLayoutGroup)
        {
            return _cellComputer.ComputePaddingCells(holderRect, gridLayoutGroup);
        }
        
    }
}