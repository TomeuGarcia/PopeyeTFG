using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR


namespace Popeye.Modules.WorldElements.PullableBlocks.GridMovement
{

    [CustomEditor(typeof(GridPlaneMovementArea)), CanEditMultipleObjects]

    public class GridPlaneMovementAreaHelperEditor : UnityEditor.Editor
    {
        
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            GridPlaneMovementArea gridPlaneMovementArea = (GridPlaneMovementArea)target;
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("Spawn Area Plane"))
            {
                gridPlaneMovementArea.SpawnAreaPlane();
            }
            
            if (GUILayout.Button("Clear Area Planes"))
            {
                gridPlaneMovementArea.ClearAreaPlanes();
            }
            
            if (GUILayout.Button("Reset Positions"))
            {
                //rectangularAreaPlane.ResetPositions();
            }
        }
        
        
        private void OnSceneGUI()
        {
            GridPlaneMovementArea gridPlaneMovementArea = (GridPlaneMovementArea)target;

            foreach (GridPlaneMovementArea.AreaPlaneWrapper areaPlaneWrapper in gridPlaneMovementArea.AreaPlaneWrappers)
            {
                if (areaPlaneWrapper != null)
                {
                    DrawCorners(areaPlaneWrapper);                    
                }
            }
        }

        private void DrawCorners(GridPlaneMovementArea.AreaPlaneWrapper areaPlaneWrapper)
        {
            IRectangularAreaPlane rectangularAreaPlane = areaPlaneWrapper.RectangularAreaPlane;
            
            
            
            Vector3 newCenter = Handles.PositionHandle(rectangularAreaPlane.Center, Quaternion.identity);
            Vector3 newCornerA = Handles.PositionHandle(rectangularAreaPlane.CornerA, Quaternion.identity);
            Vector3 newCornerB = Handles.PositionHandle(rectangularAreaPlane.CornerB, Quaternion.identity);
            Vector3 newCornerC = Handles.PositionHandle(rectangularAreaPlane.CornerC, Quaternion.identity);
            Vector3 newCornerD = Handles.PositionHandle(rectangularAreaPlane.CornerD, Quaternion.identity);
            
            
            Vector3[] centerAndCorners = new Vector3[5] { 
                newCenter, 
                newCornerA, 
                newCornerB, 
                newCornerC, 
                newCornerD 
            };

            Handles.SnapToGrid(centerAndCorners, SnapAxis.All);
            
            rectangularAreaPlane.Center = newCenter;
            rectangularAreaPlane.CornerA = newCornerA;
            rectangularAreaPlane.CornerB = newCornerB;
            rectangularAreaPlane.CornerC = newCornerC;
            rectangularAreaPlane.CornerD = newCornerD;




            Vector3[] corners = new Vector3[4]
            {
                rectangularAreaPlane.CornerA,
                rectangularAreaPlane.CornerB,
                rectangularAreaPlane.CornerC,
                rectangularAreaPlane.CornerD
            };
            
            Debug.Log(rectangularAreaPlane.CornerA);

            Handles.color = areaPlaneWrapper.DrawColor;
            for (int i = 0; i < corners.Length; ++i)
            {
                Handles.DrawLine(corners[i], corners[(i + 1) % corners.Length], 5.0f);
            }


        }
        
    }
    


}

#endif