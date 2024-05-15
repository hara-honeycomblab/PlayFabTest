using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerDataTest : MonoBehaviour
{
    [Inject]
    private IPlayerData _iPlayerData;

    [SerializeField]
    private Button[] _btns;

    private void Start()
    {
        _btns[0].OnClickAsObservable()
        .Subscribe(_ => GetUserData())
        .AddTo(this);
    }

    private async void GetUserData()
    {
        var player = await _iPlayerData.GetPlayerData();
        Debug.Log(player.exp + "\n" + player.skill + "\n" + player.level + "\n" + player.skill);
        foreach (var skillName in player.skill.names)
        {
            Debug.Log(skillName + "\n");
        }
    }
}
