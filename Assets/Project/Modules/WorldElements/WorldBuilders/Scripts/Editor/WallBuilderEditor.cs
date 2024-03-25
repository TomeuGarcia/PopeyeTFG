using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldBuilders
{
    [CustomEditor(typeof(WallBuilder))]
    public class WallBuilderEditor : UnityEditor.Editor
    {
        private WallBuilder _wallBuilder;
        private WallBuilder.CornerPointsGroup _baseCornerPointsGroup;
        
        private Transform _handleTransform;
        private Quaternion _handleRotation;
        
        private int _selectedIndex = -1;
        private int _selectedDepth = -1;

        private WallBuilderConfig.EditorViewConfig _editorView;
        private float ButtonPickSize => _editorView.ButtonSize + 0.05f;


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            _wallBuilder = target as WallBuilder;
            _baseCornerPointsGroup = _wallBuilder.BaseCornerPointsGroup;

            GUILayout.Space(20);
            GUILayout.Label("BUTTONS");

            if (GUILayout.Button("Add Base Point"))
            {
                Undo.RecordObject(_wallBuilder, "Add Point");
                _wallBuilder.AddPoint();
                EditorUtility.SetDirty(_wallBuilder);
            }
            
            
            GUILayout.Space(15);            
            if (GUILayout.Button("Center Around Pivot"))
            {
                Undo.RecordObject(_wallBuilder, "Center Around Pivot");
                _wallBuilder.CenterAroundPivot(out Vector2 offset);
                EditorUtility.SetDirty(_wallBuilder);
            }
            
            
            GUILayout.Space(15);
            if (GUILayout.Button("Move Pivot To Center"))
            {
                Undo.RecordObject(_wallBuilder, "Move Pivot To Center");
                _wallBuilder.MovePivotToCenter();
                EditorUtility.SetDirty(_wallBuilder);
            }
            
            
            GUILayout.Space(25);
            if (GUILayout.Button("Bake Walls"))
            {
                Undo.RecordObject(_wallBuilder, "Bake Walls");
                _wallBuilder.BakeWalls();
                EditorUtility.SetDirty(_wallBuilder);
            }
            
            GUILayout.Space(25);
            if (GUILayout.Button("Clear Baked Walls"))
            {
                Undo.RecordObject(_wallBuilder, "Clear Baked Walls");
                _wallBuilder.ClearBakedWalls();
                EditorUtility.SetDirty(_wallBuilder);
            }
            
        }


        private void OnSceneGUI()
        {
            _wallBuilder = target as WallBuilder;

            if (!_wallBuilder.IsReadyForEditor())
            {
                return;
            }
            

            _editorView = _wallBuilder.EditorView;

            _baseCornerPointsGroup = _wallBuilder.BaseCornerPointsGroup;
            
            _handleTransform = _wallBuilder.transform;
            _handleRotation = Tools.pivotRotation == PivotRotation.Local
                ? _handleTransform.rotation
                : Quaternion.identity;
            

            int groupId = 0;
            DrawPointButtons(_baseCornerPointsGroup, ref groupId);
        }


        private void OnDisable()
        {
            _selectedIndex = -1;
            _selectedDepth = -1;
        }

        private void DrawPointButtons(WallBuilder.CornerPointsGroup cornerPointsGroup, ref int groupId)
        {
            groupId += 1;
            for (int i = 0; i < cornerPointsGroup.NumberOfCornerPoints; ++i)
            {
                WallBuilder.CornerPoint cornerPoint = cornerPointsGroup.CornerPoints[i];

                
                Vector3 point = _handleTransform.TransformPoint(cornerPoint.Position);
            
                Handles.color = _editorView.ButtonColor;
                if (Handles.Button(point, _handleRotation, _editorView.ButtonSize, ButtonPickSize, Handles.DotHandleCap))
                {
                    _selectedIndex = i;
                    _selectedDepth = groupId;
                }

                
                if (_selectedIndex == i && _selectedDepth == groupId)
                {
                    DrawMoveButton(point, cornerPoint);
                    DrawPointInformation(point, cornerPointsGroup, i);
                    cornerPoint.SetSelected(true);
                }
                else
                {
                    cornerPoint.SetSelected(false);
                }
                
                DrawPointButtons(cornerPoint.SubPointsGroup, ref groupId);
            }
        }


        private void DrawMoveButton(Vector3 point, WallBuilder.CornerPoint cornerPoint)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, _handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_wallBuilder, "Move point");
                EditorUtility.SetDirty(_wallBuilder);
                cornerPoint.Position = _handleTransform.InverseTransformPoint(point);
            }
        }

        private void DrawPointInformation(Vector3 point, WallBuilder.CornerPointsGroup cornerPointsGroup, int index)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = _editorView.TextColor;
            style.normal.background = _editorView.TextBackground;
            int depth = _baseCornerPointsGroup.GetDepth() - cornerPointsGroup.GetDepth();
            string text = " Depth: " + depth + " \n Index: " + index + " ";
            Handles.Label(point + Vector3.up, text, style);
        }
        
    }
}

