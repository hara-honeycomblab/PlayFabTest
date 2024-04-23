using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Zenject;

public class CustomIdLogin : ICustomIdLogin
{
    [Inject]
    private AsyncToken _asyncToken;
    [Inject]
    private PlayerPrefsUtility _playerPrefsUtility;
    [Inject]
    private GroupFunction _groupFunction;
    private bool _shouldCreateAccount;
    private string _customID;
    private const string AES_IV_256 = @"mER5Ve6jZ/F8CY%~";
    private const string AES_Key_256 = @"kxvuA&k|WDRkzgG47yAsuhwFzkQZMNf3";

    public async UniTask LoginAsync(CancellationToken cancellationToken)
    {
        //_playerPrefsUtility.DeleteCutomId();
        LoginResult result = null;
        PlayFabError error = null;

        _customID = LoadCustomID();
        Debug.Log("Login: _customID: " + _customID);

        var request = new LoginWithCustomIDRequest { CustomId = _customID, CreateAccount = _shouldCreateAccount };
        PlayFabClientAPI.LoginWithCustomID(request,
            async r => { await OnLoginSuccess(r); result = r; },
            async e => { await OnLoginFailure(e); error = e; });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    private async UniTask OnLoginSuccess(LoginResult result)
    {
        if (_shouldCreateAccount == true && result.NewlyCreated == false)
        {
            Debug.LogWarning("CustomId :" + _customID);
            await LoginAsync(_asyncToken.GetToken());
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
                await LoginAsync(_asyncToken.GetToken());
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
            return DecryptStringFromBytes_Aes(binaryData);
        }
    }

    private void SaveCustomID()
    {
        var encrypted = EncryptStringToBytes_Aes(_customID);
        var base64Str = System.Convert.ToBase64String(encrypted);
        _playerPrefsUtility.SetCutomId(base64Str);
    }

    private string GenerateCustomID()
    {
        Guid guid = Guid.NewGuid();

        return guid.ToString("N");
    }

    private static byte[] EncryptStringToBytes_Aes(string plainText)
    {
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.BlockSize = 128;
            aesAlg.KeySize = 256;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV_256);
            aesAlg.Key = Encoding.UTF8.GetBytes(AES_Key_256);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        return encrypted;
    }

    public static string DecryptStringFromBytes_Aes(byte[] cipherText)
    {
        string plaintext = null;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.BlockSize = 128;
            aesAlg.KeySize = 256;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV_256);
            aesAlg.Key = Encoding.UTF8.GetBytes(AES_Key_256);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }
}