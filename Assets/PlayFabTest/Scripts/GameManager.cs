using UnityEngine;
using Zenject;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class GameManager : MonoBehaviour
{
    [Inject]
    private PlayFabCustomIdLogin _idLogin;

    [Inject]
    private PlayerData _playerData;

    [Inject]
    private AsyncToken _asyncToken;

    [Inject]
    private FriendFunction _friendFunction;

    private Player _player = new Player();

    private void Start()
    {
        StartAsync();
    }

    private async void StartAsync()
    {
        await _idLogin.LoginAsync();
        _player = await _playerData.GetPlayerData(_asyncToken.GetToken());
        await _playerData.SetPlayerData(_player, _asyncToken.GetToken());
        _friendFunction.AddFriend(FriendFunction.FriendIdType.PlayFabId, "", _asyncToken.GetToken());
    }

    private static void StartCloudHelloWorld()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "haratest",
            FunctionParameter = new { inputValue = "YOUR NAME" },
            GeneratePlayStreamEvent = true,
        }, result =>
        {
            var jsonResult = (JsonObject)result.FunctionResult;
            jsonResult.TryGetValue("messageValue", out object messageValue);
            Debug.Log(messageValue);
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
