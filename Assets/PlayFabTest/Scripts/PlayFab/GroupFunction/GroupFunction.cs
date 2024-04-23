using PlayFab;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using PlayFab.GroupsModels;
using System.Collections.Generic;

public class GroupFunction
{
    public async UniTask CreateGroup(string groupName, EntityKey entityKey, CancellationToken cancellationToken)
    {
        CreateGroupResponse result = null;
        PlayFabError error = null;

        var request = new CreateGroupRequest { GroupName = groupName, Entity = entityKey };

        PlayFabGroupsAPI.CreateGroup(request,
        r => { Debug.Log("Result CreateGroup:" + r); result = r; },
        e =>
        {
            Debug.Log("Erorr CreateGroup:" + e.GenerateErrorReport());
            error = e;
        });
        
        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask ListGroups(EntityKey entityKey, CancellationToken cancellationToken)
    {
        ListMembershipResponse result = null;
        PlayFabError error = null;

        var request = new ListMembershipRequest { Entity = entityKey };
        PlayFabGroupsAPI.ListMembership(request,
        r => { Debug.Log("Result ListGroups:" + r); result = r; },
        e =>
        {
            Debug.Log("Erorr ListGroups:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask DeleteGroup(string groupId, CancellationToken cancellationToken)
    {
        EmptyResponse result = null;
        PlayFabError error = null;

        var request = new DeleteGroupRequest { Group = EntityKeyMaker(groupId) };
        PlayFabGroupsAPI.DeleteGroup(request,
        r => { Debug.Log("Result ListGroups:" + r); result = r; },
        e =>
        {
            Debug.Log("Erorr ListGroups:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask InviteToGroup(string groupId, EntityKey entityKey, CancellationToken cancellationToken)
    {
        InviteToGroupResponse result = null;
        PlayFabError error = null;

        var request = new InviteToGroupRequest { Group = EntityKeyMaker(groupId), Entity = entityKey };
        PlayFabGroupsAPI.InviteToGroup(request,
        async r => { Debug.Log("Result ListGroups:" + r); await OnInvite(r, cancellationToken); result = r; },
        e =>
        {
            Debug.Log("Erorr ListGroups:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask OnInvite(InviteToGroupResponse response, CancellationToken cancellationToken)
    {
        var prevRequest = (InviteToGroupRequest)response.Request;
        EmptyResponse result = null;
        PlayFabError error = null;

        var request = new AcceptGroupInvitationRequest { Group = EntityKeyMaker(prevRequest.Group.Id), Entity = prevRequest.Entity };
        PlayFabGroupsAPI.AcceptGroupInvitation(request,
        r => { Debug.Log("Result ListGroups:" + r); result = r; },
        e =>
        {
            Debug.Log("Erorr ListGroups:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask ApplyToGroup(string groupId, EntityKey entityKey, CancellationToken cancellationToken)
    {
        ApplyToGroupResponse result = null;
        PlayFabError error = null;
        var request = new ApplyToGroupRequest { Group = EntityKeyMaker(groupId), Entity = entityKey };
        PlayFabGroupsAPI.ApplyToGroup(request,
        async r => { Debug.Log("Result ListGroups:" + r); await OnApply(r, cancellationToken); result = r; },
        e =>
        {
            Debug.Log("Erorr ListGroups:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask OnApply(ApplyToGroupResponse response, CancellationToken cancellationToken)
    {
        var prevRequest = (ApplyToGroupRequest)response.Request;
        EmptyResponse result = null;
        PlayFabError error = null;

        var request = new AcceptGroupApplicationRequest { Group = prevRequest.Group, Entity = prevRequest.Entity };
        PlayFabGroupsAPI.AcceptGroupApplication(request,
        r => { Debug.Log("Result ListGroups:" + r); result = r; },
        e =>
        {
            Debug.Log("Erorr ListGroups:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask KickMember(string groupId, EntityKey entityKey, CancellationToken cancellationToken)
    {
        EmptyResponse result = null;
        PlayFabError error = null;
        
        var request = new RemoveMembersRequest { Group = EntityKeyMaker(groupId), Members = new List<EntityKey> { entityKey } };
        PlayFabGroupsAPI.RemoveMembers(request,
        r => { Debug.Log("Result ListGroups:" + r); result = r; },
        e =>
        {
            Debug.Log("Erorr ListGroups:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public static EntityKey EntityKeyMaker(string entityId)
    {
        return new EntityKey { Id = entityId };
    }

    // public async UniTask CreateGroup(string entityToken, string groupName, CancellationToken cancellationToken)
    // {
    //     ExecuteCloudScriptResult result = null;
    //     PlayFabError error = null;

    //     PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
    //     {
    //         FunctionName = "createGroup",
    //         FunctionParameter = new { entityToken = entityToken, groupName = groupName },
    //         GeneratePlayStreamEvent = true
    //     },
    //     r => { Debug.Log("Result CreateGroup:" + r); result = r; },
    //     e =>
    //     {
    //         Debug.Log("Erorr CreateGroup:" + e.GenerateErrorReport());
    //         error = e;
    //     });

    //     await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
    //     cancellationToken.ThrowIfCancellationRequested();
    // }
}
