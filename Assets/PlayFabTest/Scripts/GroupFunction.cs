using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class GroupFunction
{
    public async UniTask CreateGroup(string entityToken, string groupName, CancellationToken cancellationToken)
    {
        ExecuteCloudScriptResult result = null;
        PlayFabError error = null;

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "createGroup",
            FunctionParameter = new { entityToken = entityToken, groupName = groupName },
            GeneratePlayStreamEvent = true
        },
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
