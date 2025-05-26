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


        SendOtherPlayers_MapEnterExit(hallPlayerComponent,
            request.cur_map,
            request.map_type,
            request.player_id,
            curSelectRole.role_id,
            hallPlayer);

        await FTask.CompletedTask;
    }

    private void SendOtherPlayers_MapEnterExit(
        Component_HallPlayerManager hallPlayerManager,
        int lastMapTypeID,
        int curMapTypeID,
        long curAccountID,
        int curRoleID,
        Model_HallPlayer curPlayer) {
        
        // TODO 不应该还有一个 , 将当前客户端同步给当前地图的其他玩家的操作吗? 2025年5月26日18:49:05
        
        //   将其他玩家的信息推送给当前客户端
        {
            var curList = hallPlayerManager.GetHallPlayersInMap(curMapTypeID, curAccountID);
            Msg_OtherPlayerStateSync msgIn = new Msg_OtherPlayerStateSync();
            msgIn.role_data = new StateSyncData() { };
            foreach (var player in curList) {
                if (player != null) {
                    msgIn.role_data.role_id = player.role.role_id;
                    msgIn.role_data.player_map_status = (int)PlayerMapStatus.InMap;
                    msgIn.role_data.player_id = player.player_id;
                    msgIn.role_data.position = player.position.ToCSVector3();
                    msgIn.role_data.input_dir = new CSVector3();  // TODO
                    curPlayer.session.Send(msgIn);
                }
            }
        }

        // 广播上一张地图上的所有玩家, 当前玩家离开了
        {
            var lastList = hallPlayerManager.GetHallPlayersInMap(lastMapTypeID, curAccountID);
            Msg_OtherPlayerStateSync msgOut = new Msg_OtherPlayerStateSync();
            msgOut.role_data = new StateSyncData() {
                map_type = lastMapTypeID,
                player_id = curAccountID,
                player_map_status = (int)PlayerMapStatus.OutMap,
                role_id = curRoleID,
            };


            foreach (var player in lastList) {
                if (player != null) {
                    player.session.Send(msgOut);
                }
            }
        }
    }
}