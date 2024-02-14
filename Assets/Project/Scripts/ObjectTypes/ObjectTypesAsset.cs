using System;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Scripts.ObjectTypes
{
    [CreateAssetMenu(fileName = "ObjectTypesAsset_NAME", 
        menuName = ScriptableObjectsHelper.OBJECTTYPE_ASSETS_PATH + "ObjectTypesAsset")]
    public class ObjectTypesAsset : ScriptableObject
    {
        [SerializeField] private ObjectTypes _objectType;

        public ObjectTypes ObjectType => _objectType;
        
    }
}