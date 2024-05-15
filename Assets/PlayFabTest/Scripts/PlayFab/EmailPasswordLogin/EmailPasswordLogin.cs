using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Zenject;
public class EmailPasswordLogin : AsyncToken, IEmailPasswordLogin
{
    [Inject]
    private PlayerPrefsUtility _playerPrefsUtility;
    [Inject]
    private AesProcess _aesProcess;
    private bool _shouldCreateAccount;
    private string _customID;

    public async UniTask RegisterAsync(string userName, string email, string password)
    {
        var token = GetToken();
        RegisterPlayFabUserResult result = null;
        PlayFabError error = null;

        var RegisterData = new RegisterPlayFabUserRequest()
        {
            Username = userName,
            Email = email,
            Password = password
        };

        PlayFabClientAPI.RegisterPlayFabUser(RegisterData,
        r => { Debug.Log("Result RegisterAsync"); result = r; },
        e =>
        {
            Debug.Log("Erorr RegisterAsync:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || token.IsCancellationRequested);
        token.ThrowIfCancellationRequested();
    }
    
    public async UniTask LoginAsync(string email, string password)
    {
        //_playerPrefsUtility.DeleteCutomId();
        var token = GetToken();
        LoginResult result = null;
        PlayFabError error = null;

        var request = new LoginWithEmailAddressRequest { Email = email, Password = password };
        PlayFabClientAPI.LoginWithEmailAddress(request,
        r => { Debug.Log("Result LoginAsync"); OnLoginSuccess(r); result = r; },
        e =>
        {
            Debug.Log("Erorr LoginAsync:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || token.IsCancellationRequested);
        token.ThrowIfCancellationRequested();
    }

    private void OnLoginSuccess(LoginResult result)
    {
        _playerPrefsUtility.SetEntityToken(result.EntityToken.EntityToken);
        _playerPrefsUtility.SetEntityKey(result.EntityToken.Entity);
        _playerPrefsUtility.SetPlayFabId(result.PlayFabId);
        Debug.Log("OnLoginSuccess!!");
    }
}