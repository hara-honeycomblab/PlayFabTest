using UnityEngine;
using Zenject;


public class GameManager : MonoBehaviour
{
    [Inject]
    private AsyncToken _asyncToken;

    [Inject]
    private ICustomIdLogin _iCustomIdLogin;

    private Player _player = new Player();

    private void Start()
    {
        StartAsync();
    }

    private async void StartAsync()
    {
        await _iCustomIdLogin.LoginAsync(_asyncToken.GetToken());
    }
}
