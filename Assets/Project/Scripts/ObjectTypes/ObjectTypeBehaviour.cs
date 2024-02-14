using UnityEngine;

namespace Popeye.Scripts.ObjectTypes
{
    public class ObjectTypeBehaviour : MonoBehaviour, IObjectType
    {
        [SerializeField] private ObjectTypesAsset _objectType;


        public bool IsOfType(ObjectTypes typeToCompare)
        {
            return _objectType.ObjectType.HasFlag(typeToCompare);
        }
    }
}