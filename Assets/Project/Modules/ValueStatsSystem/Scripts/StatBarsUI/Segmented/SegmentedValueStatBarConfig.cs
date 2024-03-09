using System;
using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

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
        [SerializeField] private bool _fixedCellSize = false;
        
        [ShowIf("_fixedCellSize")]
        [SerializeField] private Vector2 _cellSize = new Vector2(30, 30);
        
        [HideIf("_fixedCellSize")]
        [SerializeField] private Vector2 _spacingBetweenCells = new Vector2(20, 0);
        
        [SerializeField] private RectOffset _paddingCells = new RectOffset();


        
        private ICellComputer _cellComputer;
        

        public void Init()
        {
            _cellComputer = _fixedCellSize
                ? new FixedSizeCellComputer(_spacingBetweenCells, _paddingCells, _cellSize)
                : new HolderRectAdaptCellComputer(_spacingBetweenCells, _paddingCells);
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


        public Vector2 ComputeCellSize(int numberOfSegments, Rect holderRect)
        {
            return _cellComputer.ComputeCellSize(numberOfSegments, holderRect);
        }

        public Vector2 ComputeSpacingBetweenCells(int numberOfSegments, Rect holderRect)
        {
            return _cellComputer.ComputeSpacingBetweenCells(numberOfSegments, holderRect);
        }
        
        public RectOffset ComputePaddingCells(Rect holderRect)
        {
            return _cellComputer.ComputePaddingCells(holderRect);
        }
        
    }
}