using PlayFab.ClientModels;

public class EntityKeyMaker
{
    public static EntityKey Set(string entityId)
    {
        return new EntityKey { Id = entityId };
    }
}
