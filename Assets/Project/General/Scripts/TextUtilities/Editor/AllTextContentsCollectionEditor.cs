using System.Collections.Generic;
using Popeye.Scripts.EditorUtilities;
using Popeye.Scripts.TextUtilities;
using UnityEditor;
using UnityEngine;


namespace Popeye.Scripts.Core.Scenes.Editor
{
    [CustomEditor(typeof(AllTextContentsCollection), true)]
    public class AllTextContentsCollectionEditor : UnityEditor.Editor
    {
        private static readonly string[] _excludedProperties = { "m_Script" };
   
        private AllTextContentsCollection _sceneReferencesInspected;
        
        private int _textContentsTotal = 0;
        private int _incompletedTextContentsCounter;
        private string _incompletedTextString;
        private GUIStyle _headerLabelStyle;
        private GUIStyle _incompleteHeaderLabelStyle;

        private void OnEnable()
        {
            _sceneReferencesInspected = target as AllTextContentsCollection;
            UpdateCollection();
            InitializeGuiStyles();
        }
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space(10);
                    
            EditorGUILayout.LabelField($"Total TextContents ({_textContentsTotal}): ", _headerLabelStyle);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Incomplete TextContents: ", _headerLabelStyle);
            EditorGUILayout.TextField(_incompletedTextString, _incompleteHeaderLabelStyle);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            if (GUILayout.Button("Update TextContents"))
            {
                OnUpdateTextContentsPressed();
            }
            
            EditorGUILayout.Space(10);
            
            DrawPropertiesExcluding(serializedObject, _excludedProperties);    
        }
        
        private void InitializeGuiStyles()
        {
            _headerLabelStyle = new GUIStyle(EditorStyles.largeLabel)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 14,
                fixedHeight = 40.0f
            };
            
            _incompleteHeaderLabelStyle = new GUIStyle(EditorStyles.largeLabel)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 14,
                fixedHeight = 40.0f
            };

            UpdateLabels();
        }

        private void UpdateLabels()
        {
            bool existIncompletedTextContents = _incompletedTextContentsCounter > 0;
                        
            _incompleteHeaderLabelStyle.normal.textColor = existIncompletedTextContents
                ? Color.red
                : Color.green;

            _incompletedTextString = existIncompletedTextContents
                ? _incompletedTextContentsCounter.ToString()
                : "All completed";
        }

        private void OnUpdateTextContentsPressed()
        {
            UpdateCollection();
            UpdateLabels();
        }
        
        private void UpdateCollection()
        {
            TextContent[] textContents = FindAssetsHelper.GetAllAssetsInProject<TextContent>();
            _sceneReferencesInspected.UpdateTextContents(textContents, FilterIncompleteTextContents(textContents));
        }

        private TextContent[] FilterIncompleteTextContents(TextContent[] textContents)
        {
            List<TextContent> incompleteTextContents = new List<TextContent>(textContents.Length);
            
            foreach (TextContent textContent in textContents)
            {
                if (!textContent.HasAllFieldsCompleted())
                {
                    incompleteTextContents.Add(textContent);
                }
            }
            _textContentsTotal = textContents.Length;
            _incompletedTextContentsCounter = incompleteTextContents.Count;

            return incompleteTextContents.ToArray();
        }
        
    }
}