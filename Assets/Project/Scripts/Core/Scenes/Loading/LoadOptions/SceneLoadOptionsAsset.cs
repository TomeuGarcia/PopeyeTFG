using Popeye.ProjectHelpers;
using UnityEngine;


namespace Popeye.Scripts.Core.Scenes
{
    [CreateAssetMenu(fileName = "SceneLoadOptions_NAME", 
        menuName = ScriptableObjectsHelper.SCENES_ASSETS_PATH + "SceneLoadOptions")]
    public class SceneLoadOptionsAsset : ScriptableObject
    {
        [SerializeField] private SceneLoadOptions _additiveSceneLoadOptions;

        public SceneLoadOptions AdditiveSceneLoadOptions => _additiveSceneLoadOptions;
    }
}