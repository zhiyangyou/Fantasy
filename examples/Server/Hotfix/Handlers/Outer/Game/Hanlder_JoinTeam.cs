using System.Reflection;
using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Component;
using ServerShareToClient;

namespace Hotfix.Handlers.Outer.Game;

public class Hanlder_JoinTeam : MessageRPC<Send_JoinTeam, Rcv_JoinTeam> {
    protected override async FTask Run(Session session, Send_JoinTeam request, Rcv_JoinTeam response, Action reply) {
        var component = session.Scene.GetComponent<Component_TeamManager>();
        var ret = await component.JoinTeam(request.account_id, request.team_id);
        response.ErrorCode = ret.errorCode;
        var success = ret.errorCode == ErrorCode.Success;
        if (success) {
            response.team_id = ret.teamID;
            response.team_role_list = ret.modelRoles.Select(role => role.ToRoleData()).ToList();

            // 通知玩家,有新队友加入了
            {
                Msg_TeamStateChanged msgTeamStateChanged = new Msg_TeamStateChanged();
                msgTeamStateChanged.role_data = ret.curModelRole.ToRoleData();
                msgTeamStateChanged.team_state = (int)TeamOpStatus.MemberJoined;

                foreach (var modelRole in ret.modelRoles) {
                    if (modelRole.account_id != request.account_id) {
                        modelRole.session.Send(msgTeamStateChanged);
                    }
                }
            }
        }
        await FTask.CompletedTask;
    }
}