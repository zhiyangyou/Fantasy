using Fantasy;
using Fantasy.Async;
using Fantasy.Model;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.System;

public class Handler_RegisterAccount : MessageRPC<Send_RegisterAccount, Rcv_RegisterAccount> {
    protected override async FTask Run(Session session, Send_RegisterAccount request, Rcv_RegisterAccount response, Action reply) {
        var accountCmpt = session.Scene.GetComponent<Component_AuthenticationAccount>();
        uint retCode = await accountCmpt.RegisterAccount(request.user_name, request.pass_word);
        response.ErrorCode = retCode;
        
        if (retCode == 0) {
            response.user_name = request.user_name;
            response.pass_word = request.pass_word;
        }
        Log.Info($"注册账号结果:{retCode}");
        await FTask.CompletedTask;
    }
}