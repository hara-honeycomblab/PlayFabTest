using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using GoMap;
using GoShared;
using System;
using Zenject;

public class PoiTouch : MonoBehaviour
{
    [Inject]
    private IPlayerInventory _iPlayerInventory;
    [Inject]
    private PlayerPrefsUtility _playerPrefsUtility;
    private void Start()
    {
        this.ObserveEveryValueChanged(_ => Input.touchCount)
            .Where(touchCount => touchCount > 0)
            .Subscribe(touchCount =>
            {
                PoiTouchedBank();
            })
            .AddTo(this);
    }

    private void PoiTouchedBank()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log(touch.position);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {         
                foreach (var poiKind in Enum.GetValues(typeof(GOPOIKind)))
                {
                    if (hit.transform.tag == poiKind.ToString())
                    {
                        var playFabId = _playerPrefsUtility.GetPlayFabId();
                        string[] src = {poiKind.ToString()};
                        _iPlayerInventory.GrantItemsToUser(playFabId, src);
                    }
                }
                
            }
        }
    }
}
