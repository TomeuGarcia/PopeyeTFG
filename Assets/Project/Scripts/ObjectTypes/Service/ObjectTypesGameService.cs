namespace Popeye.Scripts.ObjectTypes
{
    public class ObjectTypesGameService : IObjectTypesGameService
    {
        public ObjectTypeAsset PlayerObjectType { get; private set; }


        public ObjectTypesGameService(ObjectTypeAsset playerObjectType)
        {
            PlayerObjectType = playerObjectType;
        }
    }
}