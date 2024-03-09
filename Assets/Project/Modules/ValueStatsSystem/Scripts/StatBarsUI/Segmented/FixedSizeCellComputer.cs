using UnityEngine;

namespace Popeye.Modules.ValueStatSystem.Segmented
{
    public class FixedSizeCellComputer : ICellComputer
    {
        private Vector2 _spacingBetweenCells;
        private readonly RectOffset _paddingCells;
        private readonly Vector2 _cellSize;


        public FixedSizeCellComputer(Vector2 spacingBetweenCells, RectOffset paddingCells, Vector2 cellSize)
        {
            _spacingBetweenCells = spacingBetweenCells;
            _paddingCells = paddingCells;
            _cellSize = cellSize;
        }
        
        
        public Vector2 ComputeCellSize(int numberOfSegments, Rect holderRect)
        {
            return _cellSize;
        }

        public Vector2 ComputeSpacingBetweenCells(int numberOfSegments, Rect holderRect)
        {
            float occupiedWidthSpace = (_cellSize.x * numberOfSegments) + _paddingCells.horizontal;
            float emptyWidthSpace = holderRect.width - occupiedWidthSpace;
            
            float widthSpacing = emptyWidthSpace / (numberOfSegments - 1);
            
            float occupiedHeightSpace = _cellSize.y  + _paddingCells.vertical;
            float emptyHeightSpace = holderRect.height - occupiedHeightSpace;
            
            float heightSpacing = emptyHeightSpace / (numberOfSegments - 1);
            

            _spacingBetweenCells.x = widthSpacing;
            _spacingBetweenCells.y = heightSpacing;
            
            return _spacingBetweenCells;
        }
        
        public RectOffset ComputePaddingCells(Rect holderRect)
        {
            return _paddingCells;
        }
    }
}