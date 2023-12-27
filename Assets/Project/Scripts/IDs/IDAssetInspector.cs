using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Popeye.IDSystem
{
    
    [CustomEditor(typeof(IDAsset), true)]
    public class IDAssetInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            
            IDAsset targetIDAsset = (IDAsset)target;

            GUILayout.Space(30);
            if (GUILayout.Button("Reset ID"))
            {
                targetIDAsset.ResetId();
            }
        }
    }
}