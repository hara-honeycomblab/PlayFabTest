using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"exp", "5"},
                {"level", "1"},
                {"stamina", "1"},
                {"skills", "Skill1"}
            }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
[System.Serializable]
public class UserModel
{
    public string exp;
    public string level;
    public string stamina;
    public Skill[] skills;
    public enum Skill
    {
        Skill1,
        Skill2,
        Skill3
    }
}
