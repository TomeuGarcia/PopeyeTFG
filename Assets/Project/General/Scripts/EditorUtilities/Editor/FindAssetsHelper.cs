using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Popeye.Scripts.EditorUtilities
{
    public static class FindAssetsHelper
    {
        public static T[] GetAllAssetsInProject<T>() where T : UnityEngine.Object
        {
            List<string> paths = AssetDatabase.FindAssets($"t:{typeof(T).Name}").ToList()
                .Select(AssetDatabase.GUIDToAssetPath).ToList();
            
            T[] assets = new T[paths.Count];

            for (int i = 0; i < paths.Count; ++i)
            {
                assets[i] = (T)AssetDatabase.LoadAssetAtPath(paths[i], typeof(T));
            }
            
            return assets;
        }
    }
}