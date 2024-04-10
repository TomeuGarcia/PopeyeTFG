namespace Popeye.Scripts.ObjectTypes
{
    public interface IObjectType
    {
        bool IsOfType(ObjectTypeAsset objectTypeToCompare);
        bool IsOfAnyType(ObjectTypeAsset[] objectTypesToCompare);
    }
}