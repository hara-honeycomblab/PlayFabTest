using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class PlayerData
{
    public async UniTask SetPlayerData(Player player, CancellationToken cancellationToken)
    {
        UpdateUserDataResult result = null;
        PlayFabError error = null;

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = player.PlayerData()
        },
        r => { Debug.Log("Successfully updated user data"); result = r; },
        e =>
        {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask<Player> GetPlayerData(CancellationToken cancellationToken)
    {
        GetUserDataResult result = null;
        PlayFabError error = null;

        var player = new Player();
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            
        }, r => {
            Debug.Log("Got user data:");
            if (r.Data == null) return;

            //Add Data
            if (r.Data.ContainsKey("exp")) player.exp = r.Data["exp"].Value;
            if (r.Data.ContainsKey("level")) player.level = r.Data["level"].Value;
            if (r.Data.ContainsKey("stamina")) player.stamina = r.Data["stamina"].Value;
            if (r.Data.ContainsKey("skill"))
            {
                var skill = JsonUtility.FromJson<Skill>(r.Data["skill"].Value);
                player.skill.names = skill.names;
            }
            result = r;
        }, (e) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);

        cancellationToken.ThrowIfCancellationRequested();
        return player;
    }
}

[System.Serializable]
public class Player
{
    public string exp;
    public string level;
    public string stamina;
    public Skill skill = new Skill();

    public Dictionary<string, string> PlayerData()
    {
        var skillsJson = JsonUtility.ToJson(skill);

        //Init Data
        var dic = new Dictionary<string, string>()
        {
            {"exp", exp},
            {"level", level},
            {"stamina", stamina},
            {"skill", skillsJson}
        };
        return dic;
    }

    public void AddSkill(Skill.Name name)
    {
        if (skill.names.Contains(name)) return;

        skill.names.Add(name);
    }

    public void RemoveSkill(Skill.Name name)
    {
        if (!skill.names.Contains(name)) return;

        skill.names.Remove(name);
    }
}

[System.Serializable]
public class Skill
{
    public List<Name> names = new List<Name>();

    public enum Name
    {
        Skill1,
        Skill2,
        Skill3
    }
}
