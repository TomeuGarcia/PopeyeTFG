using System;
using System.Collections;
using System.Threading.Tasks;
using Popeye.Scripts.GizmosUtilities;
using Popeye.Timers;
using UnityEditor;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldInteractors.Editor
{
    [CustomEditor(typeof(Barrier))]
    public class BarrierEditor : UnityEditor.Editor
    {
        private bool _showPreview = false;
        private bool _showingPreview = false;
        private float _previewAnimationT = 0;
        private Barrier _target;

        private float _previewDuration = 1.0f;
        private float _previewTimeCounter = 0.0f;

        private static Color _defaultColor = new Color(1, 149f / 255f, 0);
        private static Color _previewColor = new Color(0, 135f / 255f, 1);
        private static float _arrowThickness = 3f;
        private static float _previewStartStopTime = 0.25f;
        private static float _previewEndStopTime = 0.5f;

        private void OnDisable()
        {
            _target = (Barrier)target;
            ResetBarrierState();
        }

        private void ResetBarrierState()
        {
            _target.BarrierTransform.position = _target.ActivatedStateSpot.position;
            _target.BarrierTransform.rotation = _target.ActivatedStateSpot.rotation;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _target = (Barrier)target;

            _previewDuration = _target.StartActivated ? _target.DeactivateDuration : _target.ActivateDuration;
            
            
            GUILayout.Space(40);
            GUILayout.Label("EDITOR");
            _showPreview = GUILayout.Toggle(_showPreview, "Show Preview");
            _arrowThickness = EditorGUILayout.Slider("Arrow Thickness", _arrowThickness, 0, 5);
            _defaultColor = EditorGUILayout.ColorField("Deactivated to Activated", _defaultColor);
            _previewColor = EditorGUILayout.ColorField("Preview", _previewColor);
            _previewStartStopTime = EditorGUILayout.Slider("Preview Stop Time",_previewStartStopTime, 0, 5);
            _previewEndStopTime = EditorGUILayout.Slider("Preview Stop Time",_previewEndStopTime, 0, 5);
            
            if (_showPreview && !_showingPreview)
            {
                _showingPreview = true;
                StartPreview();
            }
            else if (!_showPreview && _showingPreview)
            {
                _showingPreview = false;
                ResetBarrierState();
            }
        }
        
        private void OnSceneGUI()
        {
            _target = (Barrier)target;
            
            
            Vector3 cameraPosition = SceneView.currentDrawingSceneView.camera.transform.position;
            if (Vector3.Distance(cameraPosition, _target.transform.position) > 30) return;
            
            GizmosExtensions.HandlesDrawArrow(
                _target.DeactivatedStateSpot.position, 
                _target.ActivatedStateSpot.position, 
                _defaultColor,
                thickness: _arrowThickness);

            if (!_showPreview)
            {
                return;
            }
            
            Transform start = _target.StartActivated ? _target.ActivatedStateSpot : _target.DeactivatedStateSpot;
            Transform end = _target.StartActivated ? _target.DeactivatedStateSpot : _target.ActivatedStateSpot;
            Vector3 arrowEnd = Vector3.Lerp(start.position, end.position, _previewAnimationT);

            _target.BarrierTransform.position = arrowEnd;
            _target.BarrierTransform.rotation = Quaternion.Lerp(start.rotation, end.rotation, _previewAnimationT);
            
            GizmosExtensions.HandlesDrawArrow(
                start.position, 
                arrowEnd, 
                _previewColor,
                thickness: _arrowThickness);
        }

        private async void StartPreview()
        {                        
            while (_showingPreview)
            {
                _previewAnimationT = 0;
                _previewTimeCounter = 0;
                
                await Task.Delay(TimeSpan.FromSeconds(_previewStartStopTime));

                while (_previewTimeCounter < _previewDuration && _showingPreview)
                {
                    await Task.Yield();
                    _previewTimeCounter += Time.smoothDeltaTime * 0.5f;
                    
                    _previewAnimationT = _previewTimeCounter / _previewDuration;
                }

                await Task.Delay(TimeSpan.FromSeconds(_previewEndStopTime));
            }            
        }
        

    }
    
}