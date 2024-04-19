
var secretKey = "ZACHADMWXW4C87F49PE4GSRXZBUWJ3WX9GYZH89DQQ68TWDEX4";
var titleId = "62301";

handlers.haratest = function (args, context) {

    var message = "haratest " + currentPlayerId + "!";

    log.info(message);
    var inputValue = null;
    if (args && args.inputValue)
        inputValue = args.inputValue;
    log.debug("helloWorld:", { input: args.inputValue });

    return { messageValue: message };
};

handlers.makeEntityAPICall = function (args, context) {

    var entityProfile = context.currentEntity;

    var apiResult = entity.SetObjects({
        Entity: entityProfile.Entity,
        Objects: [
            {
                ObjectName: "obj1",
                DataObject: {
                    foo: "some server computed value",
                    prop1: args.prop1
                }
            }
        ]
    });

    return {
        profile: entityProfile,
        setResult: apiResult.SetResults[0].SetResult
    };
};

handlers.deleteMasterPlayerAccount = function(args,context) {
    var headers = {
        "X-SecretKey": secretKey
    };
    var body = {
        PlayFabId: args.playfabId
    };
    var url = `https://${titleId}.playfabapi.com/Admin/DeleteMasterPlayerAccount`;
    var content = JSON.stringify(body);
    var httpMethod = "post";
    var contentType = "application/json";
    var response = http.request(url, httpMethod, content, contentType, headers);
    return { responseContent: response };
};

handlers.createGroup = function(args,context) {
    var headers = {
        "X-EntityToken": args.entityToken
    };
    var body = {
        GroupName: args.groupName
    };
    var url = `https://${titleId}.playfabapi.com/Group/CreateGroup`;
    var content = JSON.stringify(body);
    var httpMethod = "post";
    var contentType = "application/json";
    var response = http.request(url, httpMethod, content, contentType, headers);
    return { responseContent: response };
};