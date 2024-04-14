using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Popeye.Scripts.Core.Scenes.Editor
{
    [CustomEditor(typeof(SceneReferenceAsset), true)]
    public class SceneReferenceAssetEditor : UnityEditor.Editor
    {
        private const string NO_SCENES_WARNING =
            "There is no Scene associated to this location yet." +
            "Add a new scene with the dropdown below";

        private GUIStyle _headerLabelStyle;
        private static readonly string[] _excludedProperties = { "m_Script", "_sceneName" };

        private string[] _sceneList;
        private SceneReferenceAsset _sceneAssetInspected;


        private void OnEnable()
        {
            _sceneAssetInspected = target as SceneReferenceAsset;
            PopulateScenePicker();
            InitializeGuiStyles();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Scene information", _headerLabelStyle);
            EditorGUILayout.Space();
            DrawScenePicker();
            DrawPropertiesExcluding(serializedObject, _excludedProperties);
        }


        private void PopulateScenePicker()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            _sceneList = new string[sceneCount];
            for (int i = 0; i < sceneCount; ++i)
            {                
                _sceneList[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }
        }
        
        private void InitializeGuiStyles()
        {
            _headerLabelStyle = new GUIStyle(EditorStyles.largeLabel)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 18,
                fixedHeight = 70.0f
            };
        }
        
        private void DrawScenePicker()
        {
            string sceneName = _sceneAssetInspected.SceneName;
            
            EditorGUI.BeginChangeCheck();

            int selectedSceneIndex = _sceneList.ToList().IndexOf(sceneName);

            if (selectedSceneIndex < 0)
            {
                EditorGUILayout.HelpBox(NO_SCENES_WARNING, MessageType.Warning);
            }
            selectedSceneIndex = EditorGUILayout.Popup("Scene", selectedSceneIndex, _sceneList);
            
            if (EditorGUI.EndChangeCheck())
            {   
                Undo.RecordObject(target, "Changed selected scene");
                _sceneAssetInspected.SetSceneName(_sceneList[selectedSceneIndex]);
                MarkAllDirty();
            }
        }

        private void MarkAllDirty()
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkAllScenesDirty();
        }

    }
}