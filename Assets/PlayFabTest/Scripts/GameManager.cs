using UnityEngine;
using Zenject;


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

    [Inject]
    private CloudScriptFunction _cloudScriptFunction;

    [Inject]
    private RankingFunction _rankingFunction;

    [Inject]
    private GroupFunction _groupFunction;

    [Inject]
    private PlayerPrefsUtility _playerPrefsUtility;

    private Player _player = new Player();

    private void Start()
    {
        StartAsync();
    }

    private async void StartAsync()
    {
        await _idLogin.LoginAsync();
        // _player = await _playerData.GetPlayerData(_asyncToken.GetToken());
        //await _playerData.DeleteMasterPlayerAccount("A8CAEC9918F33DF8", _asyncToken.GetToken());
        // await _friendFunction.AddFriend(FriendFunction.FriendIdType.PlayFabId, "A8CAEC9918F33DF8", _asyncToken.GetToken());
        // await _cloudScriptFunction.DeleteMasterPlayerAccount("2926E26E3B26A8D5", _asyncToken.GetToken());
        //await _rankingFunction.SetRankingValue(RankingFunction.RankingState.Level, 20, _asyncToken.GetToken());
        //await _rankingFunction.GetRankingValue(RankingFunction.RankingState.Level, _asyncToken.GetToken());
        await _groupFunction.CreateGroup(_playerPrefsUtility.GetEntityToken(), "原チーム", _asyncToken.GetToken());
    }
}
