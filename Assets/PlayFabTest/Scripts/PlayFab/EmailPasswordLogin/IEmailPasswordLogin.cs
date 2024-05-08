using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IEmailPasswordLogin
{
    UniTask RegisterAsync(string userName, string email, string password);
    UniTask LoginAsync(string email, string password);
}
