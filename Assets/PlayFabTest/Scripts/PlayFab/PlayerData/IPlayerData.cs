using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public interface IPlayerData
{
    UniTask SetPlayerData(Player player);
    UniTask<Player> GetPlayerData();
    UniTask DeleteMasterPlayerAccount(string playfabId);
    UniTask GetUserMoney();
}
