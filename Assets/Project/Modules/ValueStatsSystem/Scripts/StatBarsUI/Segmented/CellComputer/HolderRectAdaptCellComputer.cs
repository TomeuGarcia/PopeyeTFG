using UnityEngine;

namespace Popeye.Modules.ValueStatSystem.Segmented
{
    public class HolderRectAdaptCellComputer : ICellComputer
    {
        private readonly Vector2 _spacingBetweenCells;
        private readonly RectOffset _paddingCells;
        

        public HolderRectAdaptCellComputer(Vector2 spacingBetweenCells, RectOffset paddingCells)
        {
            _spacingBetweenCells = spacingBetweenCells;
            _paddingCells = paddingCells;
        }
        
        
        public Vector2 ComputeCellSize(int numberOfSegments, Rect holderRect)
        {
            float emptyWidthSpace = (_spacingBetweenCells.x * (numberOfSegments - 1)) + 
                                    _paddingCells.horizontal;

            float emptyHeightSpace = _paddingCells.vertical;
            
            float cellWidth = (holderRect.width - emptyWidthSpace) / numberOfSegments;
            float cellHeight = holderRect.height - emptyHeightSpace;

            return new Vector2(cellWidth, cellHeight);
        }

        public Vector2 ComputeSpacingBetweenCells(int numberOfSegments, Rect holderRect)
        {
            return _spacingBetweenCells;
        }
        
        public RectOffset ComputePaddingCells(Rect holderRect)
        {
            return _paddingCells;
        }
    }
}