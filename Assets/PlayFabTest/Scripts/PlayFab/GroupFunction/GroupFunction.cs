using PlayFab;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using PlayFab.GroupsModels;

public class GroupFunction
{
    // public async UniTask CreateGroup(string entityToken, string groupName, CancellationToken cancellationToken)
    // {
    //     ExecuteCloudScriptResult result = null;
    //     PlayFabError error = null;

    //     PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
    //     {
    //         FunctionName = "createGroup",
    //         FunctionParameter = new { entityToken = entityToken, groupName = groupName },
    //         GeneratePlayStreamEvent = true
    //     },
    //     r => { Debug.Log("Result CreateGroup:" + r); result = r; },
    //     e =>
    //     {
    //         Debug.Log("Erorr CreateGroup:" + e.GenerateErrorReport());
    //         error = e;
    //     });

    //     await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
    //     cancellationToken.ThrowIfCancellationRequested();
    // }

    public async UniTask CreateGroup(string groupName, EntityKey entityKey, CancellationToken cancellationToken)
    {
        CreateGroupResponse result = null;
        PlayFabError error = null;

        var request = new CreateGroupRequest { GroupName = groupName, Entity = entityKey };

        PlayFabGroupsAPI.CreateGroup(request,
        r => { Debug.Log("Result CreateGroup:" + r); result = r; },
        e =>
        {
            Debug.Log("Erorr CreateGroup:" + e.GenerateErrorReport());
            error = e;
        });
        
        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }
}
