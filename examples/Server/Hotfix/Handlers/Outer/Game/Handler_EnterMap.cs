using Fantasy;
using Fantasy.Async;
using Fantasy.Network;
using Fantasy.Network.Interface;
using Hotfix.Component;
using Hotfix.Model.Hall;
using ServerShareToClient;

namespace Hotfix.Handlers.Outer.Game;

public class Handler_EnterMap : MessageRPC<Send_EnterMap, Rcv_EnterMap> {
    protected override async FTask Run(Session session, Send_EnterMap request, Rcv_EnterMap response, Action reply) {
        var roleComponent = session.Scene.GetComponent<Component_RoleManager>();
        var account_id = request.player_id;
        var curSelectModelRole = roleComponent.GetCurSelectRole(account_id);
        if (curSelectModelRole == null) {
            response.ErrorCode = ErrorCode.EnterMap_NotSelectRole; // 当前没有选择的角色
            return;
        }
        var hallPlayerComponent = session.Scene.GetComponent<Component_HallPlayerManager>();
        Log.Info($"request.map_type:{request.map_type}");
        var configCheckRet = hallPlayerComponent.VerifyHallRoleEnterMap(request.map_type);
        if (configCheckRet != ErrorCode.Success) {
            response.ErrorCode = configCheckRet;
            return;
        }

        var curSelectRole = roleComponent.GetCurSelectRole(account_id);
        if (curSelectRole == null) {
            response.ErrorCode = ErrorCode.EnterMap_RoleNotFound;
            return;
        }

        // 上一张地图中,移除该玩家
        hallPlayerComponent.RemoveHallPlayerFromMap(account_id, request.cur_map);

        // 下一张地图, 添加该玩家 
        Model_HallPlayer hallPlayer = hallPlayerComponent.AddHallPlayerToMap(account_id, session, request.map_type, curSelectRole);

        if (hallPlayer == null) {
            response.ErrorCode = ErrorCode.EnterMap_Failed;
            return;
        }
        response.ErrorCode = ErrorCode.Success;
        response.map_type = request.map_type;
        response.player_id = account_id;
        response.role_init_pos = hallPlayer.position.ToCSVector3();

        await FTask.CompletedTask;
    }
}