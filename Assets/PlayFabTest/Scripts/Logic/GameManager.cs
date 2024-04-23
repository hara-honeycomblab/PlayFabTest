using UnityEngine;
using Zenject;


public class GameManager : MonoBehaviour
{
    [Inject]
    private ICustomIdLogin _iCustomIdLogin;

    [Inject]
    private PlayerPrefsUtility _playerPrefsUtility;

    [Inject]
    private GroupFunction _groupFunction;

    private Player _player = new Player();

    private void Start()
    {
        StartAsync();
    }

    private async void StartAsync()
    {
        await _iCustomIdLogin.LoginAsync();
        // var entityKey = _playerPrefsUtility.GetEntityKeyForGroups();
        // await _groupFunction.CreateGroup("playfabtest", entityKey, _asyncToken.GetToken());
    }
}
