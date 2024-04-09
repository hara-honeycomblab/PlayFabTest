using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabEmailPasswordLogin : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;
    private const string AES_IV_256 = @"mER5Ve6jZ/F8CY%~";
    private const string AES_Key_256 = @"kxvuA&k|WDRkzgG47yAsuhwFzkQZMNf3";

    public void Start()
    {
        Login();
    }

    private void Login()
    {
        
    }

    private void OnLoginSuccess(LoginResult result)
    {
        
    }

    private void OnLoginFailure(PlayFabError error)
    {
        
    }

    //??????????????????
    public static byte[] EncryptStringToBytes_Aes(string plainText)
    {
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            //AES??????
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

    //????????????????
    public static string DecryptStringFromBytes_Aes(byte[] cipherText)
    {
        string plaintext = null;

        using (Aes aesAlg = Aes.Create())
        {
            //AES??????(??????????)
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