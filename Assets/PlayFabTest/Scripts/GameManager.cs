using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    }
}
