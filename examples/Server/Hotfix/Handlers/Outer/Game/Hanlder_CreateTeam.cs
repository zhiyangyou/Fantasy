using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Component;
using ServerShareToClient;

namespace Hotfix.Handlers.Outer.Game;

/// <summary>
/// 创建队伍
/// </summary>
public class Hanlder_CreateTeam : MessageRPC<Send_CreateTeam, Rcv_CreateTeam> {
    protected override async FTask Run(Session session, Send_CreateTeam request, Rcv_CreateTeam response, Action reply) {
        var component = session.Scene.GetComponent<Component_TeamManager>();
        var ret = await component.CreateTeam(request.account_id);

        var success = ret.errorCode == ErrorCode.Success;
        response.ErrorCode = ret.errorCode;
        if (success) {
            response.role_data = ret.modelRole.ToRoleData();
            response.team_id = ret.teamID;
        }
        await FTask.CompletedTask;
    }
}