using Fantasy;
using Fantasy.Async;
using Fantasy.Model;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.System;

namespace Hotfix.Handlers.Outer.鉴权;

public class Handler_GetLoginToken : MessageRPC<Send_GetLoginToken, Rcv_GetLoginToken> {
    protected override async FTask Run(Session session, Send_GetLoginToken request, Rcv_GetLoginToken response, Action reply) {
        var authenticationAccountComponent = session.Scene.GetComponent<Component_AuthenticationAccount>();
        var result = await authenticationAccountComponent.LoginAccount(request.account_name, request.pass_word);
        var code = result.Item1;
        response.ErrorCode = code;
        if (code == 0) {
            // TODO 生成令牌
            response.login_address = "TODO addr";
            response.token = "TODO token";
            response.account_id = -1;
        }
        await FTask.CompletedTask;
    }
}