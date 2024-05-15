using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Zenject;

public class CatalogFunction : AsyncToken, ICatalogFunction
{
    public async UniTask<List<CatalogItem>> GetCatalogData(string catalogVersion)
    {
        var token = GetToken();
        GetCatalogItemsResult result = null;
        PlayFabError error = null;

        var catalogItems = new List<CatalogItem>();

        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
        {
            CatalogVersion = catalogVersion,
        },
        r => { Debug.Log("Result GetCatalogData"); result = r; catalogItems = result.Catalog; },
        e =>
        {
            Debug.Log("Erorr GetCatalogData:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || token.IsCancellationRequested);
        token.ThrowIfCancellationRequested();

        return catalogItems;
    }

    public async UniTask<List<StoreItem>> GetStoreData(string catalogVersion, string storeId)
    {
        var token = GetToken();
        GetStoreItemsResult result = null;
        PlayFabError error = null;

        var storeItems = new List<StoreItem>();

        PlayFabClientAPI.GetStoreItems(new GetStoreItemsRequest()
        {
            CatalogVersion = catalogVersion,
            StoreId = storeId
        },
        r => { Debug.Log("Result GetStoreData"); result = r; storeItems = result.Store; },
        e =>
        {
            Debug.Log("Erorr GetStoreData:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || token.IsCancellationRequested);
        token.ThrowIfCancellationRequested();

        return storeItems;
    }
}
