using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    private static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";
    private bool _shouldCreateAccount;
    private string _customID;
    private const string AES_IV_256 = @"mER5Ve6jZ/F8CY%~";
    private const string AES_Key_256 = @"kxvuA&k|WDRkzgG47yAsuhwFzkQZMNf3";

    public void Start()
    {
        Login();
    }

    private void Login()
    {
        _customID = LoadCustomID();
        Debug.Log("Login: _customID: " + _customID);
        var request = new LoginWithCustomIDRequest { CustomId = _customID, CreateAccount = _shouldCreateAccount };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        if (_shouldCreateAccount == true && result.NewlyCreated == false)
        {
            Debug.LogWarning("CustomId :" + _customID + "は既に使われています。");
            Login();
            return;
        }

        if (result.NewlyCreated == true)
        {
            SaveCustomID();
            Debug.Log("新規作成成功");
        }

        Debug.Log("ログイン成功!!");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("PlayFabのログインに失敗\n" + error.GenerateErrorReport());
        switch (error.Error)
        {
            case PlayFabErrorCode.AccountNotFound:
                PlayerPrefs.DeleteKey(CUSTOM_ID_SAVE_KEY);
                Login();
                break;
            default:
                Debug.LogError("ログインエラー: " + error.ErrorMessage);
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

    //暗号化のための関数
    public static byte[] EncryptStringToBytes_Aes(string plainText)
    {
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            //AESの設定
            aesAlg.BlockSize = 128;
            aesAlg.KeySize = 256;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV_256);
            aesAlg.Key = Encoding.UTF8.GetBytes(AES_Key_256);

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    //復号のための関数
    public static string DecryptStringFromBytes_Aes(byte[] cipherText)
    {
        string plaintext = null;

        using (Aes aesAlg = Aes.Create())
        {
            //AESの設定(暗号と同じ)
            aesAlg.BlockSize = 128;
            aesAlg.KeySize = 256;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV_256);
            aesAlg.Key = Encoding.UTF8.GetBytes(AES_Key_256);

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }
}