using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

//https://learn.microsoft.com/ja-jp/gaming/playfab/features/playerdata/player-inventory
public class PlayerInventory : AsyncToken, IPlayerInventory
{
    public async UniTask GrantItemsToUser(string playfabId, string[] itemIds)
    {
        var token = GetToken();
        ExecuteCloudScriptResult result = null;
        PlayFabError error = null;

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "grantItemsToUser",
            FunctionParameter = new { playfabId = playfabId, itemIds = itemIds },
            GeneratePlayStreamEvent = true
        },
        r => { Debug.Log("Result GrantItemsToUser"); result = r; },
        e =>
        {
            Debug.Log("Erorr GrantItemsToUser:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || token.IsCancellationRequested);
        token.ThrowIfCancellationRequested();
    }

    public async UniTask<List<ItemInstance>> GetUserInventory()
    {
        var token = GetToken();
        GetUserInventoryResult result = null;
        PlayFabError error = null;
        List<ItemInstance> userInventry = new List<ItemInstance>();

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest
        {},
        r => 
        { 
            Debug.Log("Result UnlockContainerInstance"); 
            userInventry = r.Inventory;
            foreach(var item in userInventry)
            {
                Debug.Log(item.DisplayName + item.ItemInstanceId);
            }
            result = r;
        },
        e =>
        {
            Debug.Log("Erorr UnlockContainerInstance:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || token.IsCancellationRequested);
        token.ThrowIfCancellationRequested();

        return userInventry;
    }

    public async UniTask UnlockContainerInstance(string containerItemInstanceId, string keyItemInstanceId = "")
    {
        var token = GetToken();
        UnlockContainerItemResult result = null;
        PlayFabError error = null;

        PlayFabClientAPI.UnlockContainerInstance(new UnlockContainerInstanceRequest
        {
            ContainerItemInstanceId = containerItemInstanceId,
            KeyItemInstanceId = keyItemInstanceId
        },
        r => { Debug.Log("Result UnlockContainerInstance"); result = r; },
        e =>
        {
            Debug.Log("Erorr UnlockContainerInstance:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || token.IsCancellationRequested);
        token.ThrowIfCancellationRequested();
    }
}
