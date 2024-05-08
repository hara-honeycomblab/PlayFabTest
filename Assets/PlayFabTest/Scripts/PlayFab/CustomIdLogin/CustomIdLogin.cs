using System;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Zenject;

public class CustomIdLogin : AsyncToken, ICustomIdLogin
{
    [Inject]
    private PlayerPrefsUtility _playerPrefsUtility;
    [Inject]
    private AesProcess _aesProcess;
    private bool _shouldCreateAccount;
    private string _customID;

    public async UniTask LoginAsync()
    {
        //_playerPrefsUtility.DeleteCutomId();
        var token = GetToken();
        LoginResult result = null;
        PlayFabError error = null;

        _customID = LoadCustomID();
        Debug.Log("Login: _customID: " + _customID);

        var request = new LoginWithCustomIDRequest { CustomId = _customID, CreateAccount = _shouldCreateAccount };
        PlayFabClientAPI.LoginWithCustomID(request,
            async r => { await OnLoginSuccess(r); result = r; },
            async e => { await OnLoginFailure(e); error = e; });

        await new WaitUntil(() => result != null || error != null || token.IsCancellationRequested);
        token.ThrowIfCancellationRequested();
    }

    private async UniTask OnLoginSuccess(LoginResult result)
    {
        if (_shouldCreateAccount == true && result.NewlyCreated == false)
        {
            Debug.LogWarning("CustomId :" + _customID);
            await LoginAsync();
            return;
        }

        if (result.NewlyCreated == true)
        {
            SaveCustomID();
            Debug.Log("SaveCustomID");
        }
        _playerPrefsUtility.SetEntityToken(result.EntityToken.EntityToken);
        _playerPrefsUtility.SetEntityKey(result.EntityToken.Entity);
        Debug.Log("OnLoginSuccess!!");
    }

    private async UniTask OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("PlayFabError: \n" + error.GenerateErrorReport());
        switch (error.Error)
        {
            case PlayFabErrorCode.AccountNotFound:
                _playerPrefsUtility.DeleteCutomId();
                await LoginAsync();
                break;
            default:
                Debug.LogError(error.ErrorMessage);
                break;
        }
    }

    private string LoadCustomID()
    {
        var encryptedId = _playerPrefsUtility.GetCutomId();
        Debug.Log("LoadCustomID: encryptedId: " + encryptedId);
        _shouldCreateAccount = string.IsNullOrEmpty(encryptedId);

        if (_shouldCreateAccount == true)
        {
            return GenerateCustomID();
        }
        else
        {
            var binaryData = System.Convert.FromBase64String(encryptedId);
            var customId = _aesProcess.DecryptStringFromBytes(binaryData);
            return customId;
        }
    }

    private void SaveCustomID()
    {
        var encrypted = _aesProcess.EncryptStringToBytes(_customID);
        var base64Str = System.Convert.ToBase64String(encrypted);
        _playerPrefsUtility.SetCutomId(base64Str);
    }

    private string GenerateCustomID()
    {
        Guid guid = Guid.NewGuid();

        return guid.ToString("N");
    }
}