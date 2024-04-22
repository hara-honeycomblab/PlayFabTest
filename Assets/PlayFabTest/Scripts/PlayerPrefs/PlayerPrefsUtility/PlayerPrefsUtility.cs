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
    private static readonly string ENTITY_ID = "ENTITY_ID";

    public void SetEntityId(string entityId)
    {
        PlayerPrefs.SetString(ENTITY_ID, entityId);
    }

    public string GetEntityId()
    {
        return PlayerPrefs.GetString(ENTITY_ID);
    }
}
