using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabCustomIdLogin
{
    private static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";
    private bool _shouldCreateAccount;
    private string _customID;
    private const string AES_IV_256 = @"mER5Ve6jZ/F8CY%~";
    private const string AES_Key_256 = @"kxvuA&k|WDRkzgG47yAsuhwFzkQZMNf3";

    public async UniTask LoginAsync()
    {
        LoginResult result = null;
        PlayFabError error = null;

        _customID = LoadCustomID();
        Debug.Log("Login: _customID: " + _customID);

        var request = new LoginWithCustomIDRequest { CustomId = _customID, CreateAccount = _shouldCreateAccount };
        PlayFabClientAPI.LoginWithCustomID(request,
            async r => { await OnLoginSuccess(r); result = r; },
            async e => { await OnLoginFailure(e); error = e; });

        await new WaitUntil(() => result != null || error != null);
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
        Debug.Log("OnLoginSuccess!!");
    }

    private async UniTask OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("PlayFabError: \n" + error.GenerateErrorReport());
        switch (error.Error)
        {
            case PlayFabErrorCode.AccountNotFound:
                PlayerPrefs.DeleteKey(CUSTOM_ID_SAVE_KEY);
                await LoginAsync();
                break;
            default:
                Debug.LogError(error.ErrorMessage);
                break;
        }
    }

    private string LoadCustomID()
    {
        var encryptedId = PlayerPrefs.GetString(CUSTOM_ID_SAVE_KEY);
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
        PlayerPrefs.SetString(CUSTOM_ID_SAVE_KEY, base64Str);
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