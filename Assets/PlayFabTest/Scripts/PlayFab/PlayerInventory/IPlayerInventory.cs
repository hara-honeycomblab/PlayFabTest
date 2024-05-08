using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


public interface IPlayerInventory
{
    UniTask GrantItemsToUser(string playfabId, string[] itemIds);
    UniTask<List<ItemInstance>> GetUserInventory();
    UniTask UnlockContainerInstance(string containerItemInstanceId, string keyItemInstanceId = "");
}
