using UnityEngine;
using Zenject;


public class GameManager : MonoBehaviour
{
    [Inject]
    private ICustomIdLogin _iCustomIdLogin;

    [Inject]
    private IEmailPasswordLogin _iEmailPasswordLogin;

    [Inject]
    private IPlayerInventory _iPlayerInventory;

    [Inject]
    private PlayerData _playerData;

    private Player _player = new Player();

    private void Start()
    {
        StartAsync();
    }

    private async void StartAsync()
    {
        await _iEmailPasswordLogin.LoginAsync("hara@honeycomb-lab.co.jp", "harahara");
        await _iPlayerInventory.GetUserInventory();
    }
}
