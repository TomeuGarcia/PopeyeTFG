using UnityEngine;

namespace Popeye.Scripts.ObjectTypes
{
    public class ObjectTypeBehaviour : MonoBehaviour, IObjectType
    {
        [SerializeField] private ObjectTypeAsset[] _objectTypes;


        public bool IsOfType(ObjectTypeAsset objectTypeToCompare)
        {
            foreach (ObjectTypeAsset objectType in _objectTypes)
            {
                if (objectType == objectTypeToCompare) return true;
            }

            return false;
        }

        public bool IsOfAnyType(ObjectTypeAsset[] objectTypesToCompare)
        {
            foreach (ObjectTypeAsset objectTypeToCompare in objectTypesToCompare)
            {
                if (IsOfType(objectTypeToCompare))
                {
                    return true;
                }
            }

            return false;
        }
    }
}