using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;


public class FriendFunction
{
    public enum FriendIdType { PlayFabId, Username, Email, DisplayName };
    public async UniTask AddFriend(FriendIdType idType, string friendId, CancellationToken cancellationToken)
    {
        AddFriendResult result = null;
        PlayFabError error = null;
        var request = new AddFriendRequest();

        switch (idType) {
            case FriendIdType.PlayFabId:
                request.FriendPlayFabId = friendId;
                break;
            case FriendIdType.Username:
                request.FriendUsername = friendId;
                break;
            case FriendIdType.Email:
                request.FriendEmail = friendId;
                break;
            case FriendIdType.DisplayName:
                request.FriendTitleDisplayName = friendId;
                break;
        }

        PlayFabClientAPI.AddFriend(request,
        r => { Debug.Log("Result AddFriend"); result = r; },
        e =>
        {
            Debug.Log("Erorr AddFriend:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask RemoveFriend(FriendInfo friendInfo, CancellationToken cancellationToken)
    {
        RemoveFriendResult result = null;
        PlayFabError error = null;

        PlayFabClientAPI.RemoveFriend(new RemoveFriendRequest {
            FriendPlayFabId = friendInfo.FriendPlayFabId
        },
        r => { Debug.Log("Result RemoveFriend"); result = r; },
        e =>
        {
            Debug.Log("Erorr RemoveFriend:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask SearchFriend(string friendPlayFabId, CancellationToken cancellationToken)
    {
        GetPlayerProfileResult result = null;
        PlayFabError error = null;

        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
        {
            PlayFabId = friendPlayFabId,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true,
            }
        },
        r => { Debug.Log("Result SearchFriend"); result = r; },
        e =>
        {
            Debug.Log("Erorr SearchFriend:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask SetFriendTags(FriendInfo friend, List<string> newTags, CancellationToken cancellationToken)
    {
        SetFriendTagsResult result = null;
        PlayFabError error = null;

        PlayFabClientAPI.SetFriendTags(new SetFriendTagsRequest
        {
            FriendPlayFabId = friend.FriendPlayFabId,
            Tags = newTags
        },
        r => { Debug.Log("Result SetFriendTags"); result = r; },
        e =>
        {
            Debug.Log("Erorr SetFriendTags:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }
}
