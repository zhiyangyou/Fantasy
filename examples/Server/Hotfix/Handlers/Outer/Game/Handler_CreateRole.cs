using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Component;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace Hotfix.Handlers.Outer.Game;

public class Handler_CreateRole : MessageRPC<Send_CreateRole, Rcv_CreateRole> {
    protected override async FTask Run(Session session, Send_CreateRole request, Rcv_CreateRole response, Action reply) {
        var roleManager = session.Scene.GetComponent<Component_RoleManager>();
        var tp = await roleManager.CreateRole(request.role_id, request.account_id, request.role_name);
        response.ErrorCode = tp.errorCode;
        if (response.ErrorCode == 0) {
            var modelRole = tp.role;
            response.role_data = modelRole.ToRoleData();
            roleManager.UpdateSelectRole(request.account_id, modelRole);
            modelRole.session = session;
        }
        await FTask.CompletedTask;
    }
}