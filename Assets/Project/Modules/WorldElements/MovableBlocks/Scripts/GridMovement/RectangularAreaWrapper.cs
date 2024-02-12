#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Popeye.Modules.WorldElements.MovableBlocks.GridMovement
{
    [System.Serializable]
    public class RectangularAreaWrapper
    {
        [SerializeField] private bool _showArea = true;
        [SerializeField] private Color _drawColor;
        [SerializeField] private RectangularArea _rectangularArea;

        public RectangularArea RectangularArea => _rectangularArea;
        
#if UNITY_EDITOR
        private const float INVISIBLE_DISTANCE = 40;
        private const float START_FADING_DISTANCE = 15;
#endif
        
        
        public RectangularAreaWrapper(RectangularArea rectangularArea)
        {
            _rectangularArea = rectangularArea;
            _drawColor = Color.red;
        }

        public void OnValidateUpdateState()
        {
            _rectangularArea.UpdateState();
        }

        public void DrawGizmos()
        {
            if (!_showArea) return;
            
            Vector3[] corners = new Vector3[4]
            {
                _rectangularArea.Corner_RightForward,
                _rectangularArea.Corner_RightBack,
                _rectangularArea.Corner_LeftBack,
                _rectangularArea.Corner_LeftForward
            };

            Color drawColor = _drawColor;
            
#if UNITY_EDITOR
            float distanceToEditorCamera = 
                Vector3.Distance(SceneView.currentDrawingSceneView.camera.transform.position, _rectangularArea.Center);

            if (distanceToEditorCamera > INVISIBLE_DISTANCE)
            {
                return;
            }
            
            if (distanceToEditorCamera > START_FADING_DISTANCE)
            {
                float transparency = 1f - ((distanceToEditorCamera - START_FADING_DISTANCE) /
                                            (INVISIBLE_DISTANCE - START_FADING_DISTANCE));

                drawColor.a = transparency;
            }
#endif
            
            
            Handles.color = drawColor;
            for (int i = 0; i < corners.Length; ++i)
            {
                Handles.DrawLine(corners[i], corners[(i+1)% corners.Length], 3.0f);
            }


            DrawOutline(corners, 3.0f);
            DrawGrid(2.0f, 2.0f);
        }

        private void DrawOutline(Vector3[] corners, float thickness)
        {
            for (int i = 0; i < corners.Length; ++i)
            {
                Handles.DrawLine(corners[i], corners[(i+1)% corners.Length], thickness);
            }
        }
        
        private void DrawGrid(float gridSize, float thickness)
        {
            gridSize = Mathf.Max(gridSize, 0.5f);
            
            float sizeOffset = gridSize;
            float distance = Mathf.Abs(_rectangularArea.Corner_RightForward.x - _rectangularArea.Corner_LeftForward.x);

            while (sizeOffset < distance)
            {
                Vector3 from = _rectangularArea.Corner_RightForward + (Vector3.left * sizeOffset);
                Vector3 to = _rectangularArea.Corner_RightBack + (Vector3.left * sizeOffset);
                
                Handles.DrawLine(from, to, thickness);

                sizeOffset += gridSize;
            }
            
            sizeOffset = gridSize;
            distance = Mathf.Abs(_rectangularArea.Corner_RightForward.z - _rectangularArea.Corner_RightBack.z);
            while (sizeOffset < distance)
            {
                Vector3 from = _rectangularArea.Corner_RightForward + (Vector3.back * sizeOffset);
                Vector3 to = _rectangularArea.Corner_LeftForward + (Vector3.back * sizeOffset);
                
                Handles.DrawLine(from, to, thickness);

                sizeOffset += gridSize;
            }
        }
        
    }
}