using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Cysharp.Threading.Tasks;
using System.Threading;

public class CloudScriptFunction
{
    public async UniTask StartCloudHelloWorld(CancellationToken cancellationToken)
    {
        ExecuteCloudScriptResult result = null;
        PlayFabError error = null;

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "haratest",
            FunctionParameter = new { inputValue = "YOUR NAME" },
            GeneratePlayStreamEvent = true,
        },
        r => { Debug.Log("Result StartCloudHelloWorld"); result = r; },
        e =>
        {
            Debug.Log("Erorr StartCloudHelloWorld:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }
}
