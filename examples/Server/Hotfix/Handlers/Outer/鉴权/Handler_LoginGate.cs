using Fantasy;
using Fantasy.Async;
using Fantasy.Model;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Component;
using Hotfix.Model;
using Hotfix.System;

namespace Hotfix.Handlers.Outer.鉴权;

public class Handler_LoginGate : MessageRPC<Send_LoginGate, Rcv_LoginGate> {
    protected override async FTask Run(Session session, Send_LoginGate request, Rcv_LoginGate response, Action reply) {
        var authenComponent = session.Scene.GetComponent<Component_RSAEncrypt>();
        var tokenIsRight = authenComponent.VerifyToken(request.token, request.account_id, request.scene_config_id);
        if (tokenIsRight) {
            var db = session.Scene.World.DataBase;
            var modelAccount = await db.First<Model_Account>(account => account.Id == request.account_id);
            if (modelAccount != null) {
                var roleDatas = await session.Scene.GetComponent<Component_RoleManager>().GetRoleDatas(request.account_id);
                response.ErrorCode = 0;
                response.account_id = request.account_id;
                response.diamond = modelAccount.diamonds;
                response.level = modelAccount.level;
                response.gold = modelAccount.golds;
                response.role_datas = roleDatas;
                Component_SessionDispose componentSessionDispose = session.AddComponent<Component_SessionDispose>();
                componentSessionDispose.account_id = request.account_id;
            }
            else {
                response.ErrorCode = 1002;
            }
        }
        else {
            response.ErrorCode = 1001;
        }
        await FTask.CompletedTask;
    }
}