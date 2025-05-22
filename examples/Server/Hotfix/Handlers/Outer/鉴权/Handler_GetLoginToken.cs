using Fantasy;
using Fantasy.Async;
using Fantasy.Model;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Share;
using Hotfix.System;

namespace Hotfix.Handlers.Outer.鉴权;

public class Handler_GetLoginToken : MessageRPC<Send_GetLoginToken, Rcv_GetLoginToken> {
    protected override async FTask Run(Session session, Send_GetLoginToken request, Rcv_GetLoginToken response, Action reply) {
        var authenticationAccountComponent = session.Scene.GetComponent<Component_AuthenticationAccount>();
        var result = await authenticationAccountComponent.LoginAccount(request.account_name, request.pass_word);
        var code = result.Item1;
        response.ErrorCode = code;

        // 分配gate服务
        var (address, sceneConfigId) = session.Scene.GetComponent<Component_SceneConfig>().GetGate(response.account_id);
        // 生成token
        string token = session.Scene.GetComponent<Component_RSAEncrypt>().GenerateToken(response.account_id, sceneConfigId);
        
        if (code == 0) {
            response.login_address = address;
            response.token = token;
            response.account_id = response.account_id;
        }
        await FTask.CompletedTask;
    }
}