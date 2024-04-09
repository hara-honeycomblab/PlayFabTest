using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public void SetPlayerData(Player player)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = player.PlayerData()
        },
        result => Debug.Log("Successfully updated user data"),
        error =>
        {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public Player GetPlayerData()
    {
        var player = new Player();
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null) return;

            //????????????????
            if (result.Data.ContainsKey("exp")) player.exp = result.Data["exp"].Value;
            if (result.Data.ContainsKey("level")) player.level = result.Data["level"].Value;
            if (result.Data.ContainsKey("stamina")) player.stamina = result.Data["stamina"].Value;
            if (result.Data.ContainsKey("skills"))
            {
                var skillsArray = JsonUtility.FromJson<List<Player.Skill>>(result.Data["skills"].Value);
                player.skills = skillsArray;
            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });

        return player;
    }
}

[System.Serializable]
public class Player
{
    public string exp;
    public string level;
    public string stamina;
    public List<Skill> skills;

    public enum Skill
    {
        Skill1,
        Skill2,
        Skill3
    }

    public Dictionary<string, string> PlayerData()
    {
        var skillsJson = JsonUtility.ToJson(skills);
        var dic = new Dictionary<string, string>()
        {
            {"exp", exp},
            {"level", level},
            {"stamina", stamina},
            {"skills", skillsJson}
        };
        return dic;
    }
}
