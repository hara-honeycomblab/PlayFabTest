using PlayFab;
using UnityEngine;


public class PlayerPrefsUtility
{
    // ログイン時のカスタムID
    private static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";

    public void SetCutomId(string customId)
    {
        PlayerPrefs.SetString(CUSTOM_ID_SAVE_KEY, customId);
    }

    public string GetCutomId()
    {
        return PlayerPrefs.GetString(CUSTOM_ID_SAVE_KEY);
    }

    public void DeleteCutomId()
    {
        PlayerPrefs.DeleteKey(CUSTOM_ID_SAVE_KEY);
    }

    // エンティティトークン 
    private static readonly string ENTITY_TOKEN = "ENTITY_TOKEN";

    public void SetEntityToken(string entityToken)
    {
        PlayerPrefs.SetString(ENTITY_TOKEN, entityToken);
    }

    public string GetEntityToken()
    {
        return PlayerPrefs.GetString(ENTITY_TOKEN);
    }

    // エンティティID
    private static readonly string ENTITY_KEY = "ENTITY_KEY";

    public void SetEntityKey(PlayFab.ClientModels.EntityKey entityKey)
    {
        var jsonStr = JsonUtility.ToJson(entityKey);
        PlayerPrefs.SetString(ENTITY_KEY, jsonStr);
    }

    public PlayFab.ClientModels.EntityKey GetEntityKeyForClient()
    {
        var jsonStr = PlayerPrefs.GetString(ENTITY_KEY);
        var entityKey = JsonUtility.FromJson<PlayFab.ClientModels.EntityKey>(jsonStr);
        
        return entityKey;
    }

    public PlayFab.GroupsModels.EntityKey GetEntityKeyForGroups()
    {
        var clientAdminEntity = GetEntityKeyForClient();
        var jsonConverter = PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer);
        var groupAdminEntity = jsonConverter.DeserializeObject<PlayFab.GroupsModels.EntityKey>(jsonConverter.SerializeObject(clientAdminEntity));
        return groupAdminEntity;
    }
}
