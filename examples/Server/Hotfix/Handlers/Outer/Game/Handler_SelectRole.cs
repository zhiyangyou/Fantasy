using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Component;

namespace Hotfix.Handlers.Outer.Game;

public class Handler_SelectRole : MessageRPC<Send_SelectRole, Rcv_SelectRole> {
    protected override async FTask Run(Session session, Send_SelectRole request, Rcv_SelectRole response, Action reply) {
        var roleComponent = session.Scene.GetComponent<Component_RoleManager>();
        Model_Role modelRole = await roleComponent.GetRole(request.role_uid);
        modelRole.session = session;
        if (modelRole == null) {
            response.ErrorCode = 1204; // 角色不存在
        }
        else {
            response.ErrorCode = 0;
            response.role_data = modelRole.ToRoleData();
            roleComponent.UpdateSelectRole(request.account_id, modelRole);
        }
        await FTask.CompletedTask;
    }
}